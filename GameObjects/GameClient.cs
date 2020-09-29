using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        protected Player Me { get { return gameObjects.players.SingleOrDefault(p => p.ID == PlayerId); } }

        public bool GameOn { get { return gameObjects != null && gameObjects.GameOn; } }

        public GameClient(IUI owner)
        {
            PlayerName = "Human";
            UI = owner;
        }

        public async void joinNetworkGame(string URL)
        {
            try
            {
                PlayerId = new Random().Next(1_000_000, 9_999_999); //GetHashCode();
                Conn = new HubConnection(URL);
                Proxy = Conn.CreateHubProxy("GameHub");
                Proxy.On<GameState>("UpdateModel", updateGameState);
                Proxy.On<GameState>("UpdateLobby", (go) => UpdateLobby(go));
                Proxy.On<Notification, string>("Notify", Notify);
                Proxy.On("Start", Start);
                await Conn.Start();
                await Proxy.Invoke<GameState>("JoinLobby", new object[] { PlayerId, PlayerName });

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
            Console.WriteLine("frameNum: " + gameObjects.frameNum);// + "| " + tdiff.ToString()
        }

        public void UpdateLobby(GameState go)
        {
            UI.UpdateLobby(go);
        }

        public async void UpdateMe()
        {
            await Proxy.Invoke<Player>("UpdateMe", new object[] { Me });
        }

        public void Notify(Notification type, string message)
        {
            if (UI.InvokeRequired)
            {
                UI.Invoke(new Action<Notification, string>(Notify), new object[] { type, message });
            }
            else
            {
                switch (type)
                {
                    case Notification.DeathNotice:
                        {
                            Die();
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
        }
         
        protected virtual void Die()
        {
            Yoke.unbind();
            UI.AnnounceDeath();
        }

        public async Task StartServer()
        {
            await Proxy.Invoke("Start");
        }

        public virtual void Start()
        {
            if (UI.InvokeRequired)
            {
                UI.Invoke(new System.Action(Start));
            }
            else
            {
                try
                {
                    Yoke = new ControlPanel(Proxy, PlayerId);
                    Yoke.bindWASD();
                    Yoke.bindMouse();
                    UI.StartGraphics();
                    UI.CloseLobby();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
              
    }
}
