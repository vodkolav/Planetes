using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameObjects;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Services;
using Service.Internal;
using System.Drawing.Drawing2D;
using PolygonCollision;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;

namespace Planetes
{
	public partial class Game : Form
	{ 
		public Bitmap bmp;
		public Graphics g;
		public GameServer srv;

		public IHubProxy Proxy { get; set; }
		public HubConnection Conn { get; set; }

		//private Thread thrdGameLoop;
		//private int timeElapsed;
		public bool localGame;
		public bool localPlayerIsHost;
		//private Player Winner;

		private ClsGameObjects _gameObjects;

		public ClsGameObjects gameObjects
		{
			get { return localPlayerIsHost ? srv.gameObjects : _gameObjects; }
			set { _gameObjects = value; }
		}

		private bool GameOver { get { return gameObjects == null ? true : gameObjects.GameOver; } }
		 
		
		// network
		public TcpChannel serverChan, clientChan;

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
				joinNetworkGame("127.0.0.1");
			}
			
		}

		public void StartLocalGame()
		{
			srv.StartServer();
			StartGraphics();
		}

		public void StartGraphics()
		{ 
			//GameOver = false;
			bmp = new Bitmap(pbxWorld.Width, pbxWorld.Height);
			g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			pbxWorld.Image = bmp;
			//hudLeft.bind(gameObjects, gameObjects.player1);
			//hudRight.bind(gameObjects, gameObjects.player2);
			timerDraw.Interval = ClsGameObjects.FrameInterval;
			timerDraw.Start();
			//timeElapsed = 0;

		}
		
		//private void EndGame(Player winner)
		//{
		//	GameOver = true;			
		//	Winner = winner;
		//}

		public void timerDraw_Tick(object sender, EventArgs e)
		{
			if (!GameOver)
			{
				DrawGraphics();
			}
			else
			{
				//timerDraw.Stop();
				//try
				//{
				//	if (gameObjects.Winner != null)
				//	{
				//		MessageBox.Show(this, gameObjects.Winner.Name + " wins!", @"Game Over! ");
				//	}
				//}
				//catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //            }
			}
		}

		
		

		public void DrawGraphics()
		{			
			g.FillRectangle(Brushes.Black, new Rectangle(0, 0, pbxWorld.Width, pbxWorld.Height));			
			//hudLeft.Draw();
			//hudRight.Draw();
			gameObjects.Draw(g);
			pbxWorld.Image = bmp;
			if (pbxWorld != null)
				pbxWorld.Invoke(new RefreshBitmapDelegate(pbxWorld.Refresh));

		}

		//Region Piloting
		#region Piloting
		private void Game_KeyDown(object sender, KeyEventArgs e)
		{
			if (!GameOver)
			{
				if (localGame) // local game
				{
					gameObjects.control.Press(e.KeyData);
				}
			}
		}

		private void Game_KeyUp(object sender, KeyEventArgs e)
		{
			if (!GameOver)
				gameObjects.control.Release(e.KeyData);
			//gameObjects.player2.Release(e.KeyData);
		}


		private void pbxWorld_MouseMove(object sender, MouseEventArgs e)
		{
			if (!GameOver)
				gameObjects.control.Aim(e.Location);
		}

		private void pbxWorld_MouseDown(object sender, MouseEventArgs e)
		{
			if (!GameOver)
			{
				if (localGame) // local game
				{
					gameObjects.control.Press(e.Button);
				}
			}
		}

		private void pbxWorld_MouseUp(object sender, MouseEventArgs e)
		{
			if (!GameOver)
				gameObjects.control.Release(e.Button);
		}

		#endregion

		private void Game_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (localPlayerIsHost)
				srv.AbortGame();
		}

		#region Local Game

		private void humanVsHumanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			srv = GameServer.Instance;
			//ClsGameObjects.FrameInterval = 40;
			StartLocalGame();
		}

		private void humanVsBotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			srv = GameServer.Instance;

			Point p2Start = new Point(gameObjects.WinSize.Width - 100, gameObjects.WinSize.Height / 2);
			gameObjects.ReplacePlayer2(new Bot4("Bot4", 100, 300, p2Start, Color.Orange, gameObjects));

			StartLocalGame();
		}

		private void botVsBotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			srv =  GameServer.Instance;

			Point p1Start = new Point(100, gameObjects.WinSize.Height / 2);
			gameObjects.ReplacePlayer1(new Bot4("Bot41", 200, 300, p1Start, Color.Green, gameObjects));

			Point p2Start = new Point(gameObjects.WinSize.Width - 100, gameObjects.WinSize.Height / 2);
			gameObjects.ReplacePlayer2(new Bot4("Bot4", 200, 300, p2Start, Color.Orange, gameObjects));

			//ClsGameObjects.FrameInterval = 1; 

			StartLocalGame();
		}

		#endregion

		#region Network Game

		private void hostNetworkGame()
		{
			Text = "Planetes Server";
			string url = "http://localhost:8030";
			//using (WebApp.Start<Startup>(url))
			//{
			//	Console.WriteLine("Server running on {0}", url);
			//	Console.ReadLine();
			//}

			srv = GameServer.Instance;

			localGame = false;
			localPlayerIsHost = true;

			srv.Listen( url);
			while (true)
			{
				//Thread.Sleep(100);
			
					if (srv.Connected)
					{
						srv.StartServer();
						StartGraphics();
						break;
					}
				
			}
		}

		private void stopServer()
		{
			gameObjects.ServerClosed = true;
			Thread.Sleep(5);
			ChannelServices.UnregisterChannel(serverChan);
			serverChan = null;
		}

		public void setGameObjects(ClsGameObjects go)
		{
			gameObjects = go;
            //Console.WriteLine("somethin happened");
		}

		private async void joinNetworkGame(string IPAdress)
		{
			Text = "Planetes Client"; // {IPAdress}
			Conn = new HubConnection($"http://localhost:8030/");
			Proxy = Conn.CreateHubProxy("GameHub");

			Proxy.On<ClsGameObjects>("upd", go =>
		    {
			   setGameObjects(go);
                //Console.WriteLine(go.ToString());
		    });

			Proxy.On<string>("hi", go =>
			{
				Console.WriteLine(go);
			});

			await Conn.Start();
			await Proxy.Invoke("Broadcast");

			localGame = false;
			localPlayerIsHost = false;
			//gameObjects.Connected = true;
			StartGraphics();
		}

		//private void disconnect()
		//{
		//	ChannelServices.UnregisterChannel(clientChan);
		//	clientChan = null;
		//}

		private void hostToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			hostNetworkGame();
		}

		private void joinToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			VerbindungsDialog vd = new VerbindungsDialog();
			if (vd.ShowDialog() == DialogResult.OK)
				try
				{
					if (vd.Controls[3].Text == "...")
						joinNetworkGame("127.0.0.1");
					else
						joinNetworkGame(vd.Controls[3].Text);
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


