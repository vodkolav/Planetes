using GameObjects;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PlanetesWPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : Window, IUI
    {
        internal bool isServer { get => S != null; }

        public string Text { get => Title; set => Title = value; }

        public Lobby L { get; set; }

        public GameClient C { get; set; }

        public GameServer S { get; private set; }

        public WriteableBitmap B { get; set; }

        public UI()
        {
            InitializeComponent();
            C = new GameClient(this);
            L = new Lobby();//todo later: set UI as owner to lobby
        }

        public void AutoStart(string AutoStartgametype)
        {
            Show();
            switch (AutoStartgametype)
            {
                case "hostNetworkGame":
                    {
                        var URL = hostNetworkGame();
                        //string URL = C.hostNetworkGame();
                        // Server application is also client for player1.
                        _ = joinNetworkGame(URL);
                        break;
                    }
                case "joinNetworkGame":
                    {
                        C.joinNetworkGame($"http://127.0.0.1:8030/");
                        break;
                    }
                case "SinglePlayer":
                    {
                        _ = HostSingleplayer();
                        break;
                    }
            }
            Text += "Planetes: " + C.PlayerId;
        }
     
        public void AnnounceDeath()
        {
            Console.WriteLine("YOU DIED");
        }

        public void bindHUDS(GameState gameObjects)
        {
            Console.WriteLine("bind HUDS");
            //throw new NotImplementedException();
        }

        public void CloseLobby()
        {
            L.Close();
        }

        public Point fromVector(PolygonCollision.Vector vec)
        {
            return new Point(vec.X, vec.Y);
        }    

        public void DrawGraphics()
        {
            if (B != null)
            {
                // Wrap updates in a GetContext call, to prevent invalidation and nested locking/unlocking during this block
                using (B.GetBitmapContext())
                {
                    C.gameObjects.Draw();
                }
            }
        }     

        public async Task LeaveLobby()
        {
            await C.LeaveLobby();
        }

        public void Notify(string message)
        {
            //should make a separate, non-blocking notification window.
            MessageBox.Show(message);
        }

        public void Start()
        {
            Dispatcher.BeginInvoke(
                new System.Action(() =>
                {
                    StartGraphics();
                    CloseLobby();
                }));
        }

        public void StartGraphics()
        {
            B = BitmapFactory.New(C.gameObjects.WinSize.Width, C.gameObjects.WinSize.Height);
            World.Source = B;
            PolygonCollision.DrawingContext.GraphicsContainer = new WPFGraphicsContainer(B);
            CompositionTarget.Rendering += (s, e) => DrawGraphics();
        }

        public void UpdateLobby(GameState go)
        {
            L.UpdateLobby(go);
        }

        public string hostNetworkGame()
        {
            Text += " (Server)";
            string URL = "http://127.0.0.1:8030";

            S = GameServer.Instance;

            S.Listen(URL);
            return URL;
        }


        public async Task joinNetworkGame(string URL)
        {
            Text += " (Client)";
            C.joinNetworkGame(URL);

            bool GameStarted = L.OpenLobby_WaitForGuestsAndBegin();

            if (GameStarted)
            {
                await C.StartServer();
            }
            else
            {
                TerminateServer();
            }
        }

        public void TerminateServer()
        {
            if (isServer)
            {
                S.Stop();
            }
            else
            {
                Console.WriteLine("You are not the server, you can't stop it");
            }
        }

        public async Task HostSingleplayer()
        {
            var URL = hostNetworkGame();

            S.AddBot();
            S.AddBot();
            await joinNetworkGame(URL);
            //await C.StartServer();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            await joinNetworkGame($"http://127.0.0.1:8030/");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AutoStart("SinglePlayer");
        }
    }
}
