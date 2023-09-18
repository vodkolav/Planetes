using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using GameObjects.Model;
using Newtonsoft.Json;
using System.Windows;

namespace GameObjects
{
    public class GameServer 
    {
        private Random R;

        private readonly static Lazy<GameServer> _instance = new Lazy<GameServer>(() => new GameServer());
        public static GameServer Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private Thread thrdGameLoop { get; set; }

        private IHubContext hubContext { get; set; }

        public GameState gameObjects { get; set; }

        public Match Match { get; set; }

        private Dictionary<Tuple<int, Notification>,int> notificationTracking { get; set; }

        public List<Bot> Bots { get; set; }

        //the Tuple holds: int ids of player to send message to, enum Notification type, and string the message
        public BlockingCollection<Tuple<string, Notification, string>> messageQ { get; set; }

        public string URL { get; private set; }

        IDisposable webapp;
        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);

        public GameServer()
        {
            messageQ = new BlockingCollection<Tuple<string, Notification, string>>();
            hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            notificationTracking = new Dictionary<Tuple<int, Notification>, int>();
            R = new Random();
            _ = Task.Run(DispatchMessages);
        }

        public void NewGame()
        {
            gameObjects = new GameState(GameConfig.WorldSize);
            gameObjects.GameOn = GameStatus.Lobby;
            Bots = new List<Bot>(); // TODO: remove old bots connections from hub (I think, that's done already) 
            thrdGameLoop = new Thread(GameLoop)
            {
                Name = "GameLoop"
            };
        }

        public void Listen(int port)
        {
            try
            {
                URL = "http://" + GetLocalIPAddress() + ":" + port;
                // to allow doing this, run in cmd as administrator:
                // netsh http add urlacl http://*:2861/ user=host\user
                webapp = WebApp.Start<Startup>("http://*:" + port);
                Logger.Log(string.Format("Lobby open at {0}", URL), LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, LogLevel.Debug);
            }
        }

        public int Join(string ConnectionID, PlayerInfo playerInfo)
        {

            lock (gameObjects)
            {
                if (gameObjects.Players.Count >= 9)
                {
                    throw new InvalidOperationException("There can only be up to 9 players in a game ");
                }

                if (gameObjects.GameOn != GameStatus.Lobby)
                {
                    throw new InvalidOperationException("can't join game, lobby is not open");
                }

                int playerID = R.Next(1_000_000, 9_999_999); //GetHashCode();
                Player newplayer = new Player(playerID, ConnectionID, playerInfo, gameObjects);
                gameObjects.Players.Add(newplayer);
                //string gobj = JsonConvert.SerializeObject(gameObjects); //only for debugging - to check what got serialized
                gameObjects.Entities.Add(newplayer.Jet);
                return playerID;
            }
        }        

        public void Kick(Player kickedone)
        {
            Notify(kickedone, Notification.Kicked, "You were kicked by server");          
            Leave(kickedone);            
        }

        internal void Leave(string connectionID)
        {
            try
            {
                Player quitter = gameObjects.Players.Single(p => p.ConnectionID == connectionID);
                Leave(quitter);
            }
            catch (ArgumentNullException ex)
            {
                //should happen when client failed to join server
                Logger.Log(ex, LogLevel.Warning);
            }
        }

        internal void Leave(Player pl)
        {
            GameConfig.ReturnColor(pl.Color);        
            gameObjects.Entities.RemoveAll(e => e.OwnedBy(pl));
            gameObjects.Players.RemoveAll(p => p.ID == pl.ID);
            hubContext.Clients.All.UpdateLobby(gameObjects);
            hubContext.Clients.Client(pl.ConnectionID).Leave();
        }

        private void GameOver()
        {
            //tell all clients to end game
            hubContext.Clients.All.UpdateModel(gameObjects);
            hubContext.Clients.All.GameOver();
            gameObjects.Players.ForEach(p => GameConfig.ReturnColor(p.Color));
            // Make sure to resume any paused threads
            _pauseEvent.Set();
        }

        /// <summary>
        /// Game forcefully terrminated by user
        /// </summary>
        public void Terminate(string message = "Game aborted by server")
        {
            switch (gameObjects.GameOn)
            {
                case GameStatus.On:
                    // Server terminated mid-game
                    _shutdownEvent.Set();
                    break;

                case GameStatus.Lobby:
                    // server terminated in Lobby
                    NotifyAll(Notification.GameOver, message);
                    hubContext.Clients.All.Leave();
                    break;

                default: 
                    Logger.Log(message, LogLevel.Info);
                    NotifyAll(Notification.GameOver, message);
                    hubContext.Clients.All.Leave();
                    break;

            }
            // Wait for the thread to exit
            thrdGameLoop.Join();
            gameObjects.GameOn = GameStatus.Cancelled;
        }

        /// <summary>
        /// Adds Bot1 by default
        /// </summary>
        public void AddBot()
        {
            AddBot<Bot1>();
        }

        /// <summary>
        /// Allows to chose what type of bot you want it to be
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void AddBot<T>() where T : Bot, new()
        {
            try
            {
                Bot DMYSYS = new T();
                DMYSYS.joinNetworkGame(URL);
                //DMYSYS.Me.Name = "Rei";
                //DMYSYS.Me.Jet.Color = Color.White;
                //DMYSYS.UpdateMe();
                Bots.Add(DMYSYS);
                // TODO: maybe make bots inherit from LocalClient,
                // so that they don't consume network trafic.
                //  Actually, make the a sort of UI 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bot reports error: " + ex.Message);
                Logger.Log(ex, LogLevel.Debug);
            }
        }

        public void Start()
        {
            gameObjects.Start();
            Match = new Match(gameObjects);
            Match.InitFeudingParties();
            Logger.Log("Fight!", LogLevel.Info);
            thrdGameLoop.Start();
            hubContext.Clients.All.Start();
        }

        public void Pause()
        {
            _pauseEvent.Reset();
            Logger.Log("Game paused", LogLevel.Info);
        }

        public void Resume()
        {
            _pauseEvent.Set();
            Logger.Log("Game resumed", LogLevel.Info);
        }

        private async void GameLoop()
        {
            // TODO: implement quad-trees for spacial indexing :
            // https://badecho.com/index.php/2023/01/14/fast-simple-quadtree/

            // TODO: tweak SignalR performance:
            //https://learn.microsoft.com/en-us/aspnet/signalr/overview/performance/signalr-performance
            //https://learn.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-high-frequency-realtime-with-signalr
            try
            {
                GameTime.TotalElapsedSeconds = (float)(DateTime.UtcNow - gameObjects.StartTime).TotalSeconds;
                
                string CSVheader = "frame, DeltaTime, UtcNow, JetSpeed, JetPosMag, JetPosX, JetPosY , Source";
                Logger.Log(CSVheader, LogLevel.CSV);                

                while (gameObjects.GameOn == GameStatus.On)
                {
                    _pauseEvent.WaitOne(Timeout.Infinite);

                    if (_shutdownEvent.WaitOne(0))
                        break;

                    if (gameObjects.frameNum % 3 == 0) //for performance - send only every 4th frame to the clients
                    {
                        await hubContext.Clients.All.UpdateModel(gameObjects);
                    }

                    float dt = (float)(DateTime.UtcNow - gameObjects.StartTime).TotalSeconds;

                    GameTime.DeltaTime = (dt - GameTime.TotalElapsedSeconds);
                    GameTime.TotalElapsedSeconds = dt;

                    try
                    {
                        if (GameConfig.loglevels.Contains(LogLevel.CSV))
                        {
                            Jet debugged = gameObjects.Players.Single(p => p.Name.ToLower().Contains("player")).Jet;//WPFplayer
                            string csvLine = $"{gameObjects.frameNum},{GameTime.DeltaTime:F4}, " +
                                             $"{dt:F4}, {debugged.LastOffset.Magnitude:F4}, " +
                                             $"{debugged.Pos.Magnitude}, {debugged.Pos.X}, {debugged.Pos.Y}, GameLoop";
                            Logger.Log(csvLine, LogLevel.CSV);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e, LogLevel.Warning);
                    }

                    lock (gameObjects) 
                    {
                        gameObjects.Frame();
                    }

                    lock (gameObjects)
                    {
                        Match.CheckGame(this);
                    }

                    //string gobj = JsonConvert.SerializeObject(gameObjects); // only for debugging - to check what got serialized
                    //Logger.Log("frameNum: " + gameObjects.frameNum, LogLevel.Status);// + "| " + tdiff.ToString()
                }

                GameOver();
                // game over 
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }

        public void testSerialization(GameState go)
        {
            string json = JsonConvert.SerializeObject(go, Formatting.Indented);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        #region notifications

        /// <summary>
        /// Notifies Player player about message
        /// </summary>
        /// <param name="player"></param>
        /// <param name="about"></param>
        /// <param name="message"></param>
        public void Notify(Player player, Notification about, string message)
        {
            if (gameObjects.GameOn == GameStatus.Over || gameObjects.GameOn == GameStatus.Cancelled)
                return;

            Tuple<int, Notification> val = new Tuple<int, Notification>(player.ID, about);
            int lastNotif;
            // To prevent server from spamming clients with same type of notification every frame
            // we save the rounded second when a notification type was last sent to a player.
            // and disregard all same notifications that come, until the next second comes
            // So, every player gets notified about a type of event once every second at most.
            if (notificationTracking.TryGetValue(val, out lastNotif))
            {
                if (lastNotif != (int)GameTime.TotalElapsedSeconds)
                {
                    messageQ.Add(new Tuple<string, Notification, string>(player.ConnectionID, about, message));
                    notificationTracking[val] = (int)GameTime.TotalElapsedSeconds;
                }
                else
                {
                    //wait
                }
            }
            else
            {
                messageQ.Add(new Tuple<string, Notification, string>(player.ConnectionID, about, message));
                notificationTracking.Add(val, (int)GameTime.TotalElapsedSeconds);
            }

        }

        /// <summary>
        /// Notifies all clients of the same
        /// </summary>
        /// <param name="about"></param>
        /// <param name="message"></param>
        public void NotifyAll(Notification about, string message)
        {
            hubContext.Clients.All.Notify(about, message);
        }

        public void DispatchMessages()
        {
            foreach (var mes in messageQ.GetConsumingEnumerable())
            {
                string to = mes.Item1;
                Notification type = mes.Item2;
                string mess = mes.Item3;
                try
                {                    
                    hubContext.Clients.Client(to).Notify(type, mess);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex, LogLevel.Debug);
                }
            }
        }
        #endregion
    }
}
