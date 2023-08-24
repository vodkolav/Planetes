using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using PolygonCollision;
using Microsoft.AspNet.SignalR.Client.Transports;
using System.IO;
using GameObjects.Model;

namespace GameObjects
{
    /// <summary>
    /// Local Client references server's gameObjects directly, disregarding SignalR communication.
    /// Useful in debugging when you want to rule out the network latency.
    /// Not really fully implemented yet. the control still goes through SignalR.
    /// </summary>
    public class LocalClient : GameClient
    {
        
        public LocalClient(IUI owner) : base(owner)
        {

        }

        public override GameState gameObjects
        {
            get { return GameServer.Instance.gameObjects; }
        }

        public override void updateGameState(GameState go)
        {
           
        }

        public override void UpdateLobby(GameState go)
        {
            //gameObjects = go;
            World = go.World;
            UI.UpdateLobby(go);
        }

    }


    public class GameClient : IDisposable
    {
        protected IUI UI { get; set; }

        private IHubProxy Proxy { get; set; }

        private HubConnection Conn { get; set; }

        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public ControlPanel Yoke { get; set; }

        public virtual GameState gameObjects { get; set; }

        public Map World { get; set; }

        public Player Me { get { return gameObjects.Players.Single(p => p.ID == PlayerId); } }

        public bool GameOn { get { return gameObjects != null && gameObjects.GameOn== GameStatus.On; } }

        private int LastDrawnFrame { get; set; } = 1;

        StreamWriter writer;

        public GameClient(IUI owner)
        {
            UI = owner;
        }
        
        public async void joinNetworkGame(string URL)
        {
            try
            {
                Conn = new HubConnection(URL);

                // use this code to investigate problems when signalR ceases to receive model updates from server
                Conn.TraceLevel = TraceLevels.All;                
                Conn.TraceWriter = Logger.TraceInterceptor; 
                Conn.Error += (e) =>  Logger.Log(e, LogLevel.Debug);

                Proxy = Conn.CreateHubProxy("GameHub");
                Proxy.On<GameState>("UpdateModel", updateGameState);
                Proxy.On<GameState>("UpdateLobby", UpdateLobby);
                Proxy.On<int>("JoinedLobby", Joined);
                Proxy.On<Notification, string>("Notify", Notify);
                Proxy.On("Start", Start);
                Proxy.On("GameOver", GameOver);
                Proxy.On("Leave", Leave); 
                await Conn.Start(new WebSocketTransport());               
                PlayerInfo info = new PlayerInfo() {
                    PlayerName = PlayerName,
                    VisorSize = UI.VisorSize 
                };

                await Proxy.Invoke("Join", new object[] { info });
                
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }

        public void Joined(int pID)
        {
            PlayerId = pID;
            Yoke = new ControlPanel(Proxy, PlayerId);
        }

        public void Disconnect()
        {
            Logger.Log(Me.Name + " has disconnected", LogLevel.Info);
            Conn.Stop(new TimeSpan(1000));   
        }

        public void Leave()
        {
            Proxy.Invoke("Leave");
            Disconnect();
        }

        public virtual void GameOver()
        {
            Yoke.unbind();
            UI.GameOver();
            Leave();            
        }

        public virtual void updateGameState(GameState go)
        {
            //Logger.Log("received model for frame " + go.frameNum, LogLevel.Status);
            lock (gameObjects)
            {
                gameObjects = go;
            }            
        }

        public virtual void UpdateLobby(GameState go) 
        {
            //TODO: merge this with updateGameState, they essentially do the same 
            gameObjects = go;
            World = gameObjects.World;
            UI.UpdateLobby(gameObjects);
        }

        public void Notify(Notification type, string message)
        {
            //Logger.Log(message, LogLevel.Info);
            switch (type)
            {
                case Notification.Death:
                {
                    UI.AnnounceDeath(message);
                        break;
                }
                case Notification.Respawn:
                {
                    UI.AnnounceRespawn(message);
                    break;
                }
                case Notification.Message:
                {
                    UI.Notify(type,message);
                    break;
                }
                case Notification.Kicked:
                {
                    UI.Notify(type,message);
                    UI.CloseLobby();
                    break;
                }
                case Notification.Won:
                {
                    UI.Notify(type, message);                 
                    break;
                }
                case Notification.Lost:
                {
                    UI.Notify(type, message);
                    break;
                }
            }
        }

        public async Task StartServer()
        {
            await Proxy.Invoke("Start");
        }

        public virtual void Start()
        {
            Logger.Log("C.PlayerId: " + PlayerId, LogLevel.Debug);
            Logger.Log("ID: " + Me.ID + " |Name: " + Me.Name + " |Coonection: "  + Me.ConnectionID , LogLevel.Debug);
            Yoke.bindWASD();
            Yoke.bindMouse();
            UI.Start();
        }

        public void Draw()
        {
            //Logger.Log("Draw FPS: " + gameObjects.frameNum / (DateTime.Now - gameObjects.StartTime).TotalSeconds, LogLevel.Status);
            //Logger.Log("Me.Pos " + Me.Jet.Pos + " |VP: " + Me.viewPort, LogLevel.Status);
            //Logger.Log("drawing frame " + gameObjects.frameNum, LogLevel.Status);


            if (LastDrawnFrame >= gameObjects.frameNum)
            {
                return;
            }

            lock (gameObjects)
            {
                try
                {
                    LastDrawnFrame = gameObjects.frameNum;

                    // This code may be used to track a specific game object over time and then plot the data.
                    // useful when diagnosing fps issues
                    try
                    {
                        Jet debugged = gameObjects.Players.Single(p => p.Name.Contains("Bot2")).Jet; // WPFplayer
                        float dt = (float)(DateTime.UtcNow - gameObjects.StartTime).TotalSeconds;
                        Logger.Log($"{gameObjects.frameNum},{GameTime.DeltaTime:F4}, " +
                                   $"{dt:F4}, {debugged.LastOffset.Magnitude:F4}, {debugged.Pos.Magnitude}, " +
                                   $"{debugged.Pos.X}, {debugged.Pos.Y}, Draw", LogLevel.CSV);
                    }
                    catch
                    { }

                    DrawingContext.GraphicsContainer.ViewPortOffset = -Me.viewPort.Origin;

                    World.Draw();

                    foreach (Star s in World.Stars.Where(s => Me.viewPort.Collides(s).Intersect))
                    {
                        s.Draw();
                    }


                    //TODO: Make Wall also collidable
                    foreach (Wall w in World.Walls.Where(w => Me.viewPort.Collides(w).Intersect))
                    {
                        w.Draw();
                    }

                    foreach (ICollideable j in gameObjects.Entities.Where(e => Me.viewPort.Collides(e).Intersect))
                    {
                        j.Draw();
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(e,LogLevel.Debug);
                }
            }
        }

        public void SetViewPort(Vector s)
        {
            Yoke.Do(Model.Action.SetViewPort, s);
            DrawingContext.GraphicsContainer.UpdateBitmap((int)s.X, (int)s.Y);
            //Logger.Log(s.ToString(), LogLevel.Debug);
        }

        public void Dispose()
        {
            Conn.Dispose();
        }
    }
}
