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


    public class GameClient
    {
        protected IUI UI { get; set; }

        private IHubProxy Proxy { get; set; }

        private HubConnection Conn { get; set; }

        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public ControlPanel Yoke { get; set; }

        public virtual GameState gameObjects { get; set; }

        public Map World { get; set; }

        public Player Me { get { return gameObjects.Players.SingleOrDefault(p => p.ID == PlayerId); } }

        public bool GameOn { get { return gameObjects != null && gameObjects.GameOn; } }

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
                /*Conn.TraceLevel = TraceLevels.All;
                writer = new StreamWriter($"..\\..\\ClientLog_{PlayerName}.txt");
                Conn.TraceWriter = writer;*/

                Proxy = Conn.CreateHubProxy("GameHub");
                Proxy.On<GameState>("UpdateModel", updateGameState);
                Proxy.On<GameState>("UpdateLobby", UpdateLobby);
                Proxy.On<int>("JoinedLobby", (pID) => PlayerId = pID);
                Proxy.On<Notification, string>("Notify", Notify);
                Proxy.On("Start", Start);
                await Conn.Start(new WebSocketTransport());               
                PlayerInfo info = new PlayerInfo() {
                    PlayerName = PlayerName,
                    VisorSize = UI.VisorSize 
                };

                await Proxy.Invoke("JoinLobby", new object[] { info });

            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }

        public async Task LeaveLobby()
        {
            await Proxy.Invoke<GameState>("LeaveLobby", new object[] { PlayerId });
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
            gameObjects = go;
            World = gameObjects.World;
            UI.UpdateLobby(gameObjects);
        }

        public void Notify(Notification type, string message)
        {
            Logger.Log(message, LogLevel.Info);
            switch (type)
            {
                case Notification.Death:
                {
                    Die(message);
                    break;
                }
                case Notification.Respawn:
                {
                    Respawn(message);
                    break;
                }
                case Notification.Message:
                {
                    UI.Notify(message);
                    break;
                }
                case Notification.Kicked:
                {
                    UI.Notify(message);
                    UI.CloseLobby();
                    break;
                }
            }
        }

        private void Respawn(string message)
        {
            UI.AnnounceRespawn(message);
        }

        protected virtual void Die(string message)
        {
            UI.AnnounceDeath(message);
        }

        public async Task StartServer()
        {
            await Proxy.Invoke("Start");
        }

        public virtual void Start()
        {
            Yoke = new ControlPanel(Proxy, PlayerId);
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
                LastDrawnFrame = gameObjects.frameNum;
                Jet debugged = gameObjects.Players.Single(p => p.Name.Contains("Bot1")).Jet; // WPFplayer
                float dt = (float)(DateTime.UtcNow - gameObjects.StartTime).TotalSeconds;
                Logger.Log($"{gameObjects.frameNum},{GameTime.DeltaTime:F4}, " +
                           $"{dt:F4}, {debugged.LastOffset.Magnitude:F4}, {debugged.Pos.Magnitude}, " +
                           $"{debugged.Pos.X}, {debugged.Pos.Y}, Draw", LogLevel.CSV);


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
        }

        public void SetViewPort(Vector s)
        {
            Yoke.Do(Model.Action.SetViewPort, s);
            DrawingContext.GraphicsContainer.UpdateBitmap((int)s.X, (int)s.Y);
            //Logger.Log(s.ToString(), LogLevel.Debug);
        }
    }
}
