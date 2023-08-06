using GameObjects;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GameObjects.Model;

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

        public GameServer S { get; set; }

        public Billboard BB { get; set; }

        public RecorderController RC { get; set; }

        public string PlayerName = "WPFplayer";

        public UI()
        {
            InitializeComponent();
            C = new GameClient(this)
            {
                PlayerName = PlayerName
            };
            L = new Lobby(this);
            BB = new Billboard();
            Logger.Instance.Init($"..\\..\\..\\Logs\\{C}_Log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv");
            PolygonCollision.DrawingContext.GraphicsContainer = new WPFGraphicsContainer();
            DataContext = (WPFGraphicsContainer)PolygonCollision.DrawingContext.GraphicsContainer;
        }

        /// <summary>
        /// Used when starting a game automatically as soon as UI is loaded 
        /// </summary>
        /// <param name="AutoStartgametype"></param>
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
                        C.joinNetworkGame($"http://192.168.1.11:2861/");
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
     
        /// <summary>
        /// Convert from device-independent 1/60th inch WPF units to amount of pixels
        /// </summary>
        public PolygonCollision.Size VisorSize
        {
            get
            {
                var cell = (Grid)VisualTreeHelper.GetParent(Visor);
                double w = cell.ColumnDefinitions[Grid.GetColumn(Visor)].ActualWidth;
                double h = cell.RowDefinitions[Grid.GetRow(Visor)].ActualHeight;
                var source = PresentationSource.FromVisual(this);
                Matrix transformToDevice = source.CompositionTarget.TransformToDevice;
                Vector wpfSize = new Vector(w, h);
                Size pixelSize = (Size)transformToDevice.Transform(wpfSize);
                PolygonCollision.Size v = new PolygonCollision.Size((int)pixelSize.Width, (int)pixelSize.Height);
                return v;
            }
        }

        public void AnnounceDeath(string message)
        {
            Dispatcher.Invoke(() =>
            {
                BB.Show(this,message);
            });
        }

        public void AnnounceRespawn(string message)
        {
            Dispatcher.Invoke(() =>
            {
                BB.Hide();
            });
        }

        public void bindHUDS()
        {            
            foreach (Player p in C.gameObjects.Players)
            {
                if (p.ID == C.PlayerId)
                {
                    hudLeft.bind(C, p);
                }
                else
                {
                    HUD newHUD = new HUD();
                    newHUD.bind(C, p);
                    wpHUDs.Children.Add(newHUD);
                }
            }
        }

        public void CloseLobby()
        {
            Dispatcher.BeginInvoke(new System.Action(() =>
            {
                L.Close();
            }
            ));
        }

        internal void AddBot()
        {
            if (isServer)
            {
                S.AddBot();
                return;
            }
            throw new UnauthorizedAccessException("Only the host can add players");
        }

        public void KickPlayer(Player kickedone)
        {
            if (isServer)
            {
                S.Kick(kickedone);
            }
            else
            {
                Logger.Log("You can only kick yourself", LogLevel.Info);
            }
        }

        public void DrawGraphics()
        {
            // here are 3 options of alternative approaches for drawing in WPF:

            //DrawingVisual, here'is a good examplws site:
            //http://windowspresentationfoundationinfo.blogspot.com/2014_07_01_archive.html?view=classic

            // consider using SkiaSharp instead of DrawableBitmapEx
            // example here : https://github.com/swharden/Csharp-Data-Visualization/tree/main/dev/old/2019-09-08-SkiaSharp-openGL

            // or SFML: 
            // https://www.sfml-dev.org/download/bindings.php
            
            ((WPFGraphicsContainer) PolygonCollision.DrawingContext.GraphicsContainer).Draw(C);
            
            foreach (var hud in wpHUDs.Children)
            {
                ((HUD)hud).Draw();
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
            Dispatcher.Invoke(
                new System.Action(() =>
                {
                    StartGraphics();
                    CloseLobby();
                }));
        }

        public void StartGraphics()
        {
            ((WPFGraphicsContainer) PolygonCollision.DrawingContext.GraphicsContainer).UpdateBitmap(VisorSize.Width, VisorSize.Height);
            
            RC = new RecorderController((WPFGraphicsContainer)PolygonCollision.DrawingContext.GraphicsContainer);
            bindHUDS();
            CompositionTarget.Rendering += (s, e) => DrawGraphics();            
            CompositionTarget.Rendering += (s, e) => RC.AddFrame(C.gameObjects.frameNum); //TODO: Add this only when recording
            Closing += (s, e) => RC.End();
            Closing += (s, e) => Logger.Instance.Dispose();
        }
        
        public void UpdateLobby(GameState go)
        {
            L.UpdateLobby(go);
        }

        public string hostNetworkGame()
        {
            Text += " (Server)";
            S = GameServer.Instance;

            S.Listen(2861);
            return S.URL;
        }

        /// <summary>
        /// Used when joining lobby manually
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public async Task joinNetworkGame(string URL)
        {
            Text += " (Client)";
            C.joinNetworkGame(URL);
            
            bool GameStarted = L.OpenLobby_WaitForGuestsAndBegin(this);

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
                Logger.Log("You are not the server, you can't stop it", LogLevel.Info);
            }
        }

        public async Task HostSingleplayer()
        {
            var URL = hostNetworkGame();
            
            S.AddBot(); 
            S.AddBot<Bot3>();
            await joinNetworkGame(URL);
            //await C.StartServer();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            IpDialog ipd = new IpDialog();
            var dr = ipd.ShowDialog();
            if (dr.HasValue && dr.Value)
                try
                {
                    await joinNetworkGame(ipd.URL);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not connect to server!\n" + ex.Message);
                }


            //await joinNetworkGame(url);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoStart("SinglePlayer");// "HostSingleplayer");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AutoStart("SinglePlayer");
        }

        #region piloting 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (C.GameOn)
            {            
                if (e.Key == Key.R)
                {
                    RC.Start();
                }
                C.Yoke.Press(e.Key);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (C.GameOn)
            {
                if (e.Key == Key.R)
                {     
                    RC.End();
                }               
                C.Yoke.Release(e.Key);
            }
        }

        private System.Windows.Forms.MouseButtons fromMouseButton(MouseButton btn)
        {
            System.Windows.Forms.MouseButtons btns;
            Enum.TryParse(btn.ToString(), out btns);
            return btns;
        }

        private void World_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (C.GameOn)
            {
                C.Yoke.Press(e.ChangedButton);
            }
        }

        private void World_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (C.GameOn)
            {
                C.Yoke.Release(e.ChangedButton);
            }             
        }

        private PolygonCollision.Vector FromPoint(Point p)
        {
            return new PolygonCollision.Vector((float)p.X, (float)p.Y);
        }

        private void World_MouseMove(object sender, MouseEventArgs e)
        {
            if (C.GameOn)
            {
                C.Yoke.Aim(FromPoint(e.GetPosition(Visor))); 
            }
        }
        #endregion
        
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (C.GameOn)
            {
                C.SetViewPort(VisorSize);
            }
        }
    }
}
