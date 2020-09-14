using GameObjects;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
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

		public int PlayerId { get; set; }

		private ControlPanel Yoke { get; set; }

        public GameState gameObjects { get; set; }

 		public Lobby L { get; set; }       
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
				joinNetworkGame($"http://127.0.0.1:8030/" );
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

			bindHUDS(gameObjects);
			timerDraw.Interval = (int)(GameState.FrameInterval.TotalMilliseconds * 0.5);
			timerDraw.Start();
		}

		public void bindHUDS(GameState gameObjects)
        {
			foreach(Player p in gameObjects.players)
            {
				if (p.ID == PlayerId)
				{
					hudLeft.bind(this, p);
				}
				else
				{
					HUD newHUD = new HUD();
					newHUD.bind(this, p);
					flpOtherPlayers.Controls.Add(newHUD);
				}
            }
        }

        internal async Task LeaveLobby()
        {
			await Proxy.Invoke<GameState>("LeaveLobby", new object[] { PlayerId });
		}

        public void timerDraw_Tick(object sender, EventArgs e)
		{
			if (GameOn)
			{
				DrawGraphics();
			}			
		}

		public void DrawGraphics()
		{
			G.FillRectangle(Brushes.Black, new Rectangle(0, 0, pbxWorld.Width, pbxWorld.Height));
			hudLeft.Draw();
			foreach ( Control c in flpOtherPlayers.Controls)
            {
				((HUD)c).Draw();
            }			
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

		private void hostNetworkGame()
		{			
			string URL = "http://127.0.0.1:8030";

			Srv = GameServer.Instance;
					
			Srv.Listen(URL);
			Text += " (Server)";			
			// Server application is also client for player1.
			joinNetworkGame(URL);

		}  

	

	private async void joinNetworkGame(string URL)
		{
			try
			{
				PlayerId = new Random().Next(1_000_000, 9_999_999); //GetHashCode();
				Text += "Planetes: " + PlayerId;
				L = new Lobby(Text);
				Conn = new HubConnection(URL);
				Proxy = Conn.CreateHubProxy("GameHub");
				Proxy.On<GameState>("UpdateModel", updateGameState);
				Proxy.On<GameState>("UpdateLobby", (go) => UpdateLobby(go));
				Proxy.On<Notification, string>("Notify", Notify);
				Proxy.On("Start", Start);
				await Conn.Start();
				await Proxy.Invoke<GameState>("JoinLobby", new object[] { PlayerId });
				
				if (L.ShowDialog(this) == DialogResult.OK)
				{
					await Proxy.Invoke("Start");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public void updateGameState(GameState go)
		{
			gameObjects = go;
			Console.WriteLine("frameNum: " + gameObjects.frameNum);// + "| " + tdiff.ToString()
		}

		public void UpdateLobby(GameState go)
		{
			L.UpdateLobby(go);
		}

		public void Notify(Notification type,  string message)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<Notification, string>(Notify), new object[] { type, message });
			}
			else
			{
				switch(type)
                {
					case Notification.DeathNotice:
                        {
							Yoke.unbind();
							new BillBoard().Show(this);
							break;
                        }
					case Notification.Message:
						{
							MessageBox.Show(message);
							break;
						}
				}
				
			}
		}


		private void Start()
		{
			if (InvokeRequired)
			{
				Invoke(new System.Action(Start));
			}
			else
			{
				try
				{
					Yoke = new ControlPanel(Proxy, PlayerId);
					Yoke.bindWASD();
					Yoke.bindMouse(MouseButtons.Left, HOTAS.Shoot);				
					StartGraphics();
					L.Close();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
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
						joinNetworkGame($"http://127.0.0.1:8030/");
					else
						joinNetworkGame(vd.URL);
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


