using GameObjects;
using PolygonCollision;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public Game()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.FixedHeight |
                     ControlStyles.FixedWidth |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            C = new GameClient(this);
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
                    Console.WriteLine(e.Message);
                }
            }
        }


        public void StartGraphics()
        {
            B = new Bitmap(C.gameObjects.WinSize.Width, C.gameObjects.WinSize.Height);

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
            foreach (Player p in C.gameObjects.players)
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
            C.gameObjects.Draw();
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
            S.AddBot();
        }

        public void KickPlayer(Player kickedone)
        {
            if(isServer)
            {
                S.Kick( kickedone);
            }
            else
            {
                Console.WriteLine("You can only kick yourself");
            }
        }

        #region Piloting
        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Press(e.KeyData);
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Release(e.KeyData);
        }


        private void pbxWorld_MouseMove(object sender, MouseEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Aim(Vector.FromPoint(e.Location));
        }

        private void pbxWorld_MouseDown(object sender, MouseEventArgs e)
        {
            if (C.GameOn)
            {
                C.Yoke.Press(e.Button);
            }
        }

        private void pbxWorld_MouseUp(object sender, MouseEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Release(e.Button);
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
            //string URL = "http://127.0.0.1:8030";
            
            S = GameServer.Instance;
            S.Listen(8030);
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


