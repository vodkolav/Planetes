using GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planetes
{
    public partial class Game : Form, IUI
    {
        internal bool isServer { get => C.Srv != null; }

        public GameClient C { get; set; }

        public List<Bot> Bots { get; set; }

        public Bitmap B { get; set; }

        public Graphics G { get; set; }

        public Ilobby L { get; set; }

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
            Bots = new List<Bot>();
        }

        public Game(string AutoStartgametype) : this()
        {
            Show();
            if (AutoStartgametype == "hostNetworkGame")
            {
                Text += " (Server)";
                _ = hostNetworkGame();
            }
            if (AutoStartgametype == "joinNetworkGame")
            {
                Text += " (Client)";
                C.joinNetworkGame($"http://127.0.0.1:8030/");
            }
            Text += "Planetes: " + C.PlayerId;
        }


        public void StartGraphics()
        {
            B = new Bitmap(pbxWorld.Width, pbxWorld.Height);
            G = Graphics.FromImage(B);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            pbxWorld.Image = B;

            bindHUDS(C.gameObjects);
            timerDraw.Interval = (int)(GameState.FrameInterval.TotalMilliseconds * 0.25);
            timerDraw.Start();
        }

        public void bindHUDS(GameState gameObjects)
        {
            foreach (Player p in gameObjects.players)
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
            //should replace this with buffered image of the map
            G.FillRectangle(Brushes.Black, new Rectangle(0, 0, pbxWorld.Width, pbxWorld.Height));
            C.gameObjects.Draw(G);
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
            DummyPlug Rei = new DummyPlug();
            Bot DMYSYS = new Bot1(Rei);
            DMYSYS.joinNetworkGame(C.Srv.URL);
            //DMYSYS.Me.Name = "Rei";
            //DMYSYS.Me.Jet.Color = Color.White;
            //DMYSYS.UpdateMe();
            Bots.Add(DMYSYS);
        }




        #region Piloting
        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (C.GameOn)
            {
                C.Yoke.Press(e.KeyData);
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Release(e.KeyData);
        }


        private void pbxWorld_MouseMove(object sender, MouseEventArgs e)
        {
            if (C.GameOn)
                C.Yoke.Aim(e.Location);
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

        public async Task hostNetworkGame()
        {
            string URL = C.hostNetworkGame();
            // Server application is also client for player1.
            await joinNetworkGame(URL);

        }



        public async Task joinNetworkGame(string URL)
        {
            C.joinNetworkGame(URL);
            bool GameStarted = L.OpenLobby_WaitForGuestsAndBegin();

            if (GameStarted)
            {
                await C.StartServer();
            }
            else
            {
                C.TerminateServer();
            }
        }

        public void Notify(string message)
        {
            //should make a separate, non-blocking notification window.
            MessageBox.Show(message);
        }

        void IUI.AnnounceDeath()
        {
            new BillBoard().Show(this, "Tough luck");
        }

        void IUI.CloseLobby()
        {
            L.Close();
        }

        void IUI.UpdateLobby(GameState go)
        {
            L.UpdateLobby(go);
        }

        private async void hostToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            await hostNetworkGame();
        }

        private async void JoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IpConnectionDialog vd = new IpConnectionDialog();
            if (vd.ShowDialog() == DialogResult.OK)
                try
                {
                    if (vd.IP == "...")
                        await joinNetworkGame($"http://127.0.0.1:8030/");
                    else
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


