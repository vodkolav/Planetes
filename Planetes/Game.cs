using GameObjects;
using PolygonCollision;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameObjects.Model;

namespace Planetes
{
    public partial class Game : Form, IUI
    {
        internal bool isServer { get => S != null; }

        private GameServer S { get; set; }

        public GameClient C { get; set; }

        public Bitmap B { get; set; }

        public Lobby L { get; set; }

        public BillBoard Billboard { get; set; }

        public PolygonCollision.Size VisorSize
        {
            get => new PolygonCollision.Size(pbxWorld.Width, pbxWorld.Height);
        }

        public string PlayerName = "FormsPlayer";

        public Game()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.FixedHeight |
                     ControlStyles.FixedWidth |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            C = new GameClient(this);
            C.PlayerName = PlayerName;
            L = new Lobby(this);
            Billboard = new BillBoard(this);
        }

        public Game(string AutoStartgametype) : this()
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
                        C.joinNetworkGame($"http://127.0.0.1:2861/");
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


        public virtual void Start()
        {
            
            if (InvokeRequired)
            {
                Invoke(new System.Action(Start));
            }
            else
            {
                try
                {                    
                    StartGraphics();
                    CloseLobby();
                }
                catch (Exception e)
                {
                    Logger.Log(e, LogLevel.Info);
                }
            }
        }

        public void StartGraphics()
        {
            B = new Bitmap(pbxWorld.Width, pbxWorld.Height);

            Graphics G = Graphics.FromImage(B);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            DrawingContext.GraphicsContainer = new WFGraphicsContainer(G);

            pbxWorld.Image = B;

            bindHUDS();
            timerDraw.Interval = (int)GameState.FrameInterval.TotalMilliseconds;// * 0.25);
            timerDraw.Start();
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
                    flpOtherPlayers.Controls.Add(newHUD);
                }
            }
        }

        public async Task LeaveLobby()
        {
            await C.LeaveLobby();
        }

        public void timerDraw_Tick(object sender, EventArgs e)
        {
            if (C.GameOn)
            {
                DrawGraphics();
            }
        }
              
        public void DrawGraphics()
        {
            C.Draw();
            pbxWorld.Image = B;
            if (pbxWorld != null)
                pbxWorld.Invoke(new System.Action(pbxWorld.Refresh));
            hudLeft.Draw();
            foreach (Control c in flpOtherPlayers.Controls)
            {
                ((HUD)c).Draw();
            }       
        }

        public void AddBot()
        {
            if (isServer)
            {
                S.AddBot();
            }
            else
            {
                Logger.Log("You are not the server, you can't add bots", LogLevel.Info);
            }

        }

        public void KickPlayer(Player kickedone)
        {
            if(isServer)
            {
                S.Kick( kickedone);
            }
            else
            {
                Logger.Log("You can only kick yourself", LogLevel.Info);
            }
        }

        #region Piloting

        private System.Windows.Input.MouseButton toWPF(MouseButtons btn)
        {
            System.Windows.Input.MouseButton btns;
            Enum.TryParse(btn.ToString(), out btns);
            return btns;
        }

        private System.Windows.Input.Key toWPF(Keys key)
        {
            return System.Windows.Input.KeyInterop.KeyFromVirtualKey((int)key);
        }        

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {            
            if (C.GameOn)
                C.Yoke.Press(toWPF(e.KeyData));
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Release(toWPF(e.KeyData));
        }

        private void pbxWorld_MouseMove(object sender, MouseEventArgs e)
        {            
            if (C.GameOn)
                C.Yoke.Aim( WFGraphicsContainer.Point2Vector(e.Location));
        }

        private void pbxWorld_MouseDown(object sender, MouseEventArgs e)
        {
            if (C.GameOn)
            {
                C.Yoke.Press(toWPF(e.Button));
            }
        }

        private void pbxWorld_MouseUp(object sender, MouseEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Release(toWPF(e.Button));
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.StopGame();
            //should implement proper network conections termination 
        }
        #endregion

        #region Local Game

        private void humanVsHumanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //Srv = GameServer.Instance;
            //StartLocalGame();
        }

        private void humanVsBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //Srv = GameServer.Instance;
            //Point p2Start = new Point(gameObjects.WinSize.Width - 100, gameObjects.WinSize.Height / 2);
            //gameObjects.ReplacePlayer2(new Bot4("Bot4", 100, 300, p2Start, Color.Orange, gameObjects));
            //StartLocalGame();
        }

        private void botVsBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //Srv =  GameServer.Instance;
            //Point p1Start = new Point(100, gameObjects.WinSize.Height / 2);
            //gameObjects.ReplacePlayer1(new Bot4("Bot41", 200, 300, p1Start, Color.Green, gameObjects));
            //Point p2Start = new Point(gameObjects.WinSize.Width - 100, gameObjects.WinSize.Height / 2);
            //gameObjects.ReplacePlayer2(new Bot4("Bot4", 200, 300, p2Start, Color.Orange, gameObjects));
            //StartLocalGame();
        }

        #endregion

        #region Network Game

        public string hostNetworkGame()
        {
            Text += " (Server)";
            //string URL = "http://127.0.0.1:2861";

            S = GameServer.Instance;
            S.Listen(2861);
            return S.URL;
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
                Logger.Log("You are not the server, you can't stop it", LogLevel.Info);
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

        public void Notify(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(Notify), new object[] { message });
            }
            else
            {
                //should make a separate, non-blocking notification window.
                MessageBox.Show(message);
            }
        }

        public void AnnounceDeath(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AnnounceDeath),message);
            }
            else
            {
                Billboard.Show(this, message);               
            }           
        }


        public void AnnounceRespawn(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AnnounceRespawn), message);
            }
            else
            {
                Billboard.Hide();
            }
        }

        public void CloseLobby()
        {
            L.Close();
        }

        public void UpdateLobby(GameState go)
        {
            L.UpdateLobby(go);
        }

        private void hostToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            hostNetworkGame();
        }

        private async void JoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IpConnectionDialog vd = new IpConnectionDialog();
            if (vd.ShowDialog() == DialogResult.OK)
                try
                {
                    await joinNetworkGame(vd.URL);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not connect to server!\n" + ex.Message);
                    Application.Exit();
                }
        }

        private void Game_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void LocalGameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}


