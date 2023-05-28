using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using PolygonCollision;

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

        public Player Me { get { return gameObjects.players.SingleOrDefault(p => p.ID == PlayerId); } }

        public bool GameOn { get { return gameObjects != null && gameObjects.GameOn; } }

        public ViewPort viewPort { get { return Me.viewPort; } }

        public GameClient(IUI owner)
        {
            PlayerName = "Human";
            UI = owner;
        }

        public async void joinNetworkGame(string URL, Vector windowSize)
        {
            try
            {
                Conn = new HubConnection(URL);
                Proxy = Conn.CreateHubProxy("GameHub");
                Proxy.On<GameState>("UpdateModel", updateGameState);
                Proxy.On<GameState>("UpdateLobby", (go) => UpdateLobby(go));
                Proxy.On<int>("JoinedLobby", (pID) => PlayerId = pID);
                Proxy.On<Notification, string>("Notify", Notify);
                Proxy.On("Start", Start);
                await Conn.Start();

               
                PlayerInfo info = new PlayerInfo() {
                    PlayerName = PlayerName,
                    VisorSize = windowSize
                };

                await Proxy.Invoke<GameState>("JoinLobby", new object[] { info });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task LeaveLobby()
        {
            await Proxy.Invoke<GameState>("LeaveLobby", new object[] { PlayerId });
        }
        public void updateGameState(GameState go)
        {
            gameObjects = go;
            if (PlayerName == "Human")
            Console.Write("\r frameNum: " + gameObjects.frameNum);// + "| " + tdiff.ToString()
        }

        public void UpdateLobby(GameState go)
        {
            UI.UpdateLobby(go);
            updateGameState(go);
        }

        public async void UpdateMe()
        {
            await Proxy.Invoke<Player>("UpdateMe", new object[] { Me });
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
        }
    }
}
