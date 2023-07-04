using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using PolygonCollision;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace GameObjects
{
    public class GameClient
    {
        private IUI UI { get; set; }

        private IHubProxy Proxy { get; set; }

        private HubConnection Conn { get; set; }

        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public ControlPanel Yoke { get; set; }

        public GameState gameObjects { get; set; }

        public Map World { get; set; }

        public Player Me { get { return gameObjects.Players.SingleOrDefault(p => p.ID == PlayerId); } }

        public bool GameOn { get { return gameObjects != null && gameObjects.GameOn; } }

        DateTime StartTime { get; set; }

        // StreamWriter writer;

        public GameClient(IUI owner)
        {
            UI = owner;
        }

        public async void joinNetworkGame(string URL, Vector windowSize)
        {
            try
            {
                Conn = new HubConnection(URL);
                Proxy = Conn.CreateHubProxy("GameHub");
                Proxy.On<GameState>("UpdateModel", updateGameState);
                Proxy.On<GameState>("UpdateLobby", UpdateLobby);
                Proxy.On<int>("JoinedLobby", (pID) => PlayerId = pID);
                Proxy.On<Notification, string>("Notify", Notify);
                Proxy.On("Start", Start);
                await Conn.Start(new WebSocketTransport());               
                PlayerInfo info = new PlayerInfo() {
                    PlayerName = PlayerName,
                    VisorSize = windowSize
                };

                await Proxy.Invoke<GameState>("JoinLobby", new object[] { info });

                 // use this code to investigate problems when signalR ceases to receive model updates from server
/*               Conn.TraceLevel = TraceLevels.All;
                 writer = new StreamWriter($"..\\..\\ClientLog_{Me.ID}.txt");                
                 Conn.TraceWriter = writer;*/
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

        public void updateGameState(GameState go)
        {
            //Logger.Log("received model for frame " + go.frameNum, LogLevel.Status);
            lock (gameObjects)
            {
                gameObjects = go;
            }            
        }

        public void UpdateLobby(GameState go)
        {
            gameObjects = go;
            World = gameObjects.World;
            UI.UpdateLobby(gameObjects);
        }

        public void Notify(Notification type, string message)
        {

            switch (type)
            {
                case Notification.DeathNotice:
                    {
                        Die(message);
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

        protected virtual void Die(string message)
        {
            Yoke.unbind();
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
            StartTime = DateTime.Now;
        }

        public void Draw()
        {
            //TODO: draw only the objects that are in the ViewPort.
            // for this i need to replace Collides function with Rectangle.intersect - as it's supposed to be computationally cheaper.
            /*  Walls = gameState.World.Walls.Where(w => w.Body.Collides(Body, velocity).Intersect).ToList();
            Players = gameState.players.Where(p => p.Jet.Collides(Body) || p.Bullets.Any(b => Body.Collides(b.Pos))).ToList();
            Astroids = gameState.Astroids.Where(a => !Body.Collides(a.Body)).ToList(); // TODO: understand why Collides here is supposed to be negated? */
            //Logger.Log("Draw FPS: " + gameObjects.frameNum / (DateTime.Now - StartTime).TotalSeconds, LogLevel.Status);
            //Logger.Log("Me.Pos " + Me.Jet.Pos + " |VP: " + Me.viewPort, LogLevel.Status);
            //Logger.Log("drawing frame " + gameObjects.frameNum, LogLevel.Status);

            lock (gameObjects)
            {
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
    }
}
