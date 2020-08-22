using GameObjects;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Planetes
{
    public partial class Game : Form
	{ 
		public Bitmap Bmp { get; set; }

		public Graphics G { get; set; }

		public GameServer Srv { get; set; }

		public IHubProxy Proxy { get; set; }

		public HubConnection Conn { get; set; }

        private ControlPanel Yoke { get; set; }

		public GameState gameObjects { get; set; }

        private bool GameOn { get { return gameObjects != null && gameObjects.GameOn; } }
		
		delegate void RefreshBitmapDelegate();
		//delegate void RefreshProgessBarDelegate();

		public Game()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.FixedHeight | ControlStyles.FixedWidth | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
		}
		
		public Game(string gametype) : this()
		{			
			if  (gametype == "hostNetworkGame")
            {
				hostNetworkGame();
            }
			if (gametype == "joinNetworkGame")
			{
				joinNetworkGame($"http://127.0.0.1:8030/", "Player2" );
			}
			
		}

		public void StartLocalGame()
		{
			Srv.Start();
			StartGraphics();
		}

		public void StartGraphics()
		{ 
			Bmp = new Bitmap(pbxWorld.Width, pbxWorld.Height);
			G = Graphics.FromImage(Bmp);
			G.SmoothingMode = SmoothingMode.AntiAlias;
			pbxWorld.Image = Bmp;
			hudLeft.bind(this, gameObjects.player1);
			hudRight.bind(this, gameObjects.player2);
			timerDraw.Interval = (int)(GameState.FrameInterval.TotalMilliseconds*0.5);
			timerDraw.Start();
		}		
		
		public void timerDraw_Tick(object sender, EventArgs e)
		{
			if (GameOn)
			{
				DrawGraphics();
			}
			else
			{
                timerDraw.Stop();
                try
                {
                    if (gameObjects.Winner != null)
                    {
                        MessageBox.Show(this, gameObjects.Winner.Name + " wins!", @"Game Over! ");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
		}		

		public void DrawGraphics()
		{			
			G.FillRectangle(Brushes.Black, new Rectangle(0, 0, pbxWorld.Width, pbxWorld.Height));			
			hudLeft.Draw();
			hudRight.Draw();
			gameObjects.Draw(G);
			pbxWorld.Image = Bmp;
			if (pbxWorld != null)
				pbxWorld.Invoke(new RefreshBitmapDelegate(pbxWorld.Refresh));

		}
			

		#region Piloting
		private void Game_KeyDown(object sender, KeyEventArgs e)
		{
			if (GameOn)
			{		
				Yoke.Press(e.KeyData);			
			}
		}

		private void Game_KeyUp(object sender, KeyEventArgs e)
		{
			if (GameOn)
				Yoke.Release(e.KeyData);
		}


		private void pbxWorld_MouseMove(object sender, MouseEventArgs e)
		{
			if (GameOn)
				Yoke.Aim(e.Location);
		}

		private void pbxWorld_MouseDown(object sender, MouseEventArgs e)
		{
			if (GameOn)
			{
				Yoke.Press(e.Button);
			}
		}

		private void pbxWorld_MouseUp(object sender, MouseEventArgs e)
		{
			if (GameOn)
				Yoke.Release(e.Button);
		}

		#endregion

		private void Game_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (GameOn)
				Yoke.StopGame();
			//should implement proper network conections termination 
		}

		#region Local Game

		private void humanVsHumanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Srv = GameServer.Instance;
			StartLocalGame();
		}

		private void humanVsBotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Srv = GameServer.Instance;
			Point p2Start = new Point(gameObjects.WinSize.Width - 100, gameObjects.WinSize.Height / 2);
			gameObjects.ReplacePlayer2(new Bot4("Bot4", 100, 300, p2Start, Color.Orange, gameObjects));
			StartLocalGame();
		}

		private void botVsBotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Srv =  GameServer.Instance;

			Point p1Start = new Point(100, gameObjects.WinSize.Height / 2);
			gameObjects.ReplacePlayer1(new Bot4("Bot41", 200, 300, p1Start, Color.Green, gameObjects));

			Point p2Start = new Point(gameObjects.WinSize.Width - 100, gameObjects.WinSize.Height / 2);
			gameObjects.ReplacePlayer2(new Bot4("Bot4", 200, 300, p2Start, Color.Orange, gameObjects));

			StartLocalGame();
		}

		#endregion

		#region Network Game

		private void hostNetworkGame()
		{			
			string URL = "http://127.0.0.1:8030";

			Srv = GameServer.Instance;

			Srv.Listen(URL);

			// Server application is also client for player1.
			//It will join game only after player to connects and Listen() returns
			Text += " (Server)";
			joinNetworkGame(URL, "Player1");			
		}  

        private async void joinNetworkGame(string URL, string asPlayer)
		{			
			Conn = new HubConnection(URL); 
			Proxy = Conn.CreateHubProxy("GameHub");
			
			Proxy.On<GameState>("UpdateModel", go => gameObjects = go);
			await Conn.Start();
			while (gameObjects == null)
            {
                Console.WriteLine("Waiting for game data");
            }
			Yoke = new ControlPanel(Proxy, gameObjects.players.Single(p=>p.Name == asPlayer));

			Yoke.bindWASD();
			Yoke.bindMouse(MouseButtons.Left, HOTAS.Shoot);
			Text = "Planetes: " + asPlayer; 
			StartGraphics();
		}

	
		private void hostToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			hostNetworkGame();
		}

		private void JoinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IpConnectionDialog vd = new IpConnectionDialog();
			if (vd.ShowDialog() == DialogResult.OK)
				try
				{
					if (vd.IP == "...")
						joinNetworkGame($"http://127.0.0.1:8030/", "Player2");
					else
						joinNetworkGame(vd.URL, "Player2");
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


