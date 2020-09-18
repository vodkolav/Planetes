using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace GameObjects
{
    public class GameClient
    {
        public IUI UI { get; set; }

        public GameServer Srv { get; set; }

        public IHubProxy Proxy { get; set; }

        public HubConnection Conn { get; set; }

        public int PlayerId { get; set; }

        public ControlPanel Yoke { get; set; }

        public GameState gameObjects { get; set; }

        public bool GameOn { get { return gameObjects != null && gameObjects.GameOn; } }
        public GameClient(IUI owner)
        {
            UI = owner;
        }
        public string hostNetworkGame()
        {
            string URL = "http://127.0.0.1:8030";

            Srv = GameServer.Instance;

            Srv.Listen(URL);

            return URL;
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
                await Proxy.Invoke<GameState>("JoinLobby", new object[] { PlayerId });


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
                            Yoke.unbind();
                            UI.AnnounceDeath();
                            //new BillBoard().Show(UI);
                            break;
                        }
                    case Notification.Message:
                        {
                            UI.Notify(message);
                            //MessageBox.Show(message);
                            break;
                        }
                }

            }
        }
        public async Task StartServer()
        {
            await Proxy.Invoke("Start");
        }

        private void Start()
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

        public void TerminateServer()
        {
            if (Srv != null)
            {
                Srv.Stop();
            }
            else
            {
                Console.WriteLine("You are not the server, you can't stop it");
            }
        }
    }
}
