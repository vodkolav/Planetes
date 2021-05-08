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

namespace GameObjects
{
    public class GameServer
    {
        private Random R ;

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

        public List<Bot> Bots { get; set; }

        //the Tuple holds: int ids of player to send message to, enum Notification type, and string the message
        public BlockingCollection<Tuple<string, Notification, string>> messageQ { get; set; }

        public string URL { get; private set; }

        IDisposable webapp;
        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);

        public GameServer()
        {
            gameObjects = new GameState(GameConfig.WorldSize);
            Bots = new List<Bot>();
            messageQ = new BlockingCollection<Tuple<string, Notification, string>>();
            hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            R = new Random();
        }
        public void Listen(int port)
        {
            try
            {
                URL = "http://" + GetLocalIPAddress() + ":" + port;
                // to allow doing this, run in cmd as administrator:
                // netsh http add urlacl http://*:8030/ user=host\user
                webapp = WebApp.Start<Startup>("http://*:" + port);
                Console.WriteLine(string.Format("Lobby open at {0}", URL));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

        public int Join( string ConnectionID, string PlayerName)
        {
            int playerID = R.Next(1_000_000, 9_999_999); //GetHashCode();
            Player newplayer = new Player(playerID, ConnectionID, PlayerName, gameObjects);
            gameObjects.players.Add(newplayer);
            //string gobj = JsonConvert.SerializeObject(gameObjects); //only for debugging - to check what got serialized

            return playerID;
        }

        public void AddBot()
        {
            if (gameObjects.players.Count > 8)
            {
                throw new IndexOutOfRangeException("There can only be 9 players in a game ");
            }
            else
            {  
                DummyPlug Rei = new DummyPlug();
                Bot DMYSYS = new Bot1(Rei);
                DMYSYS.joinNetworkGame(URL);
                //DMYSYS.Me.Name = "Rei";
                //DMYSYS.Me.Jet.Color = Color.White;
                //DMYSYS.UpdateMe();
                Bots.Add(DMYSYS);
            }
        }

        public void Kick(Player kickedone)
        {
            Notify(kickedone, Notification.Kicked, "You were kicked by server");
            Leave(kickedone);
            _ = Task.Run(DispatchMessages);
        }

        internal void Leave(int playerID)
        {
            Player pl = gameObjects.players.Single(p => p.ID == playerID);
            Leave(pl);
        }

        internal void Leave(Player pl)
        {
            GameConfig.ReturnColor(pl.Color);
            gameObjects.players.RemoveAll(p => p.ID == pl.ID);
            hubContext.Clients.All.UpdateLobby(gameObjects);
        }

        public void Notify(Player player, Notification about, string message)
        {
            messageQ.Add(new Tuple<string, Notification, string>(player.ConnectionID, about, message));
        }

        public void DispatchMessages()
        {
            if (messageQ.Count != 0)
            {
                foreach (var mes in messageQ.GetConsumingEnumerable())
                {
                    string to = mes.Item1;
                    Notification type = mes.Item2;
                    string mess = mes.Item3;
                    hubContext.Clients.Client(to).Notify(type, mess);
                }
            }
        }

        public void Start()
        {
            gameObjects.InitFeudingParties();
            thrdGameLoop = new Thread(GameLoop)
            {
                Name = "GameLoop"
            };
            thrdGameLoop.Start();
            Console.WriteLine("Fight!");
            gameObjects.GameOn = true;
            hubContext.Clients.All.Start();
        }

        /// <summary>
        /// Define enemies for each player
        /// </summary>

        public void Stop(string message = "Game aborted")
        {
            // Signal the shutdown event
            _shutdownEvent.Set();
            Console.WriteLine(message);

            gameObjects.GameOn = false;
            //if (thrdGameLoop != null)
            //	thrdGameLoop.Abort();
            webapp.Dispose();

            // Make sure to resume any paused threads
            _pauseEvent.Set();

            // Wait for the thread to exit
            thrdGameLoop.Join();
        }

        public void Pause()
        {
            _pauseEvent.Reset();
            Console.WriteLine("Game paused");
        }

        public void Resume()
        {
            _pauseEvent.Set();
            Console.WriteLine("Game resumed");
        }

        private async void GameLoop()
        {
            try
            {
                DateTime dt;// maybe replace this with stopwatch
                TimeSpan tdiff;
                Player loser;
                while (gameObjects.GameOn)
                {
                    _pauseEvent.WaitOne(Timeout.Infinite);

                    if (_shutdownEvent.WaitOne(0))
                        break;

                    dt = DateTime.UtcNow;

                    //reap dead losers, if any
                    if ((loser = gameObjects.Reap()) != null)
                    {
                        Notify(loser, Notification.DeathNotice, "YOU DIED");
                    }
                    gameObjects.Frame();

                    //string gobj = JsonConvert.SerializeObject(gameObjects); only for debugging - to check what got serialized
                    await hubContext.Clients.All.UpdateModel(gameObjects);
                    _ = Task.Run(DispatchMessages);

                    tdiff = DateTime.UtcNow - dt;
                    Thread.Sleep((GameState.FrameInterval - tdiff).Duration()); //this is bad. There should be timer instead
                    //Console.WriteLine("frameNum: " + gameObjects.frameNum);// + "| " + tdiff.ToString()
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
