﻿using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using PolygonCollision;
using Microsoft.AspNet.SignalR.Client.Transports;
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
            ModelStore = go.ModelStore;
            Resources.LoadFrom(ModelStore);
            UI.UpdateLobby(go);
        }

        public override void Dispose()
        {
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

        public Resources ModelStore { get; set; }

        public Player Me { get { return gameObjects.Players.Single(p => p.ID == PlayerId); } }

        public bool GameOn { get { return gameObjects != null && gameObjects.GameOn== GameStatus.On; } }

        private int LastDrawnFrame { get; set; } = 1;


        public GameClient(IUI owner)
        {
            UI = owner;
        }
        
        public async void joinNetworkGame(string URL)
        {
            try
            {
                Conn = new HubConnection(URL);

                Logger.TraceConnection(Conn);
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
            Resources.LoadFrom(gameObjects.ModelStore);
            UI.UpdateLobby(gameObjects);
        }

        public void Notify(Notification type, string message)
        {
            //Logger.Log(message, LogLevel.Info);
            //TODO: move this logic to UI and get rid of AnnounceDeath and AnnounceRespawn
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
            gameObjects.Track("player", "header");
            await Proxy.Invoke("Start");
        }

        public virtual void Start()
        {
            Logger.Log("C.PlayerId: " + PlayerId, LogLevel.Debug);
            Logger.Log("ID: " + Me.ID + " |Name: " + Me.Name + " |Connection: "  + Me.ConnectionID , LogLevel.Debug);
            Yoke.bindWASD();
            Yoke.bindMouse();
            UI.Start();
            ClientTime.StartTime = gameObjects.StartTime;
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
                    ClientTime.Tick();
                    LastDrawnFrame = gameObjects.frameNum;
                    gameObjects.Track("player", "Draw");
           
                    //TODO: thoroughly test whether this Client-side prediction improves smoothness of movement.
                    // currently it seems that it makes movement jerky.
                    // on the other hand, judjing from the tracking data, the predicted position of jet is more 
                    // faithful to the actual position the jet is supposed to be at a given moment.
                    // More research required.

                    //Me.Jet.Move(ClientTime.DeltaTime);//*0.5f
                    //Me.Jet.upToDate = false;

                    DrawingContext.GraphicsContainer.ViewPortOffset = -Me.viewPort.Origin;
                    gameObjects.Track("player", "CSP");


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
                       // j.Move(ClientTime.DeltaTime);
                       // j.upToDate = false;
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

        public virtual void Dispose()
        {
            if(Conn != null)
            Conn.Dispose();
        }
    }
}
