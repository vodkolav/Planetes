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

namespace Planetes
{
	public partial class Game : Form
	{
		public Bitmap bmp;
		public Graphics g;
		public ClsGameObjects gameObjects;
		//private Thread thrdGameLoop;
		//private int timeElapsed;
		public bool localGame;
		public bool localPlayerIsHost;
		//private Player Winner;

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

		public void StartLocalGame()
		{
			gameObjects.StartServer();
			StartGraphics();
		}

		public void StartGraphics()
		{ 
			//GameOver = false;
			bmp = new Bitmap(pbxWorld.Width, pbxWorld.Height);
			g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			pbxWorld.Image = bmp;
			hudLeft.bind(gameObjects, gameObjects.player1);
			hudRight.bind(gameObjects, gameObjects.player2);
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
				timerDraw.Stop();
				if (gameObjects.Winner != null)
				{
					MessageBox.Show(this, gameObjects.Winner.Name + " wins!", @"Game Over! ");
				}
			}
		}

		
		

		public void DrawGraphics()
		{
			g.FillRectangle(Brushes.Black, new Rectangle(0, 0, pbxWorld.Width, pbxWorld.Height));			
			hudLeft.Draw();
			hudRight.Draw();
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
			gameObjects.control.Release(e.KeyData);
			//gameObjects.player2.Release(e.KeyData);
		}


		//if (e.KeyData == Keys.Space)
		//{
		//	gameObjects.player1.Steer(e.KeyData);
		//}
		//else if (e.KeyData == Keys.Return)
		//{
		//	gameObjects.player2.Steer(e.KeyData);
		//}
		////else if (e.KeyData == Keys.A || e.KeyData == Keys.D || e.KeyData == Keys.W || e.KeyData == Keys.S )
		//else if (new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D }.Contains(e.KeyData))
		//{
		//	//then it's a horizontal move
		//	gameObjects.player1.Steer(e.KeyData);
		//}
		//else if (e.KeyData == Keys.Left || e.KeyData == Keys.Right || e.KeyData == Keys.Up || e.KeyData == Keys.Down)
		//{
		//	//then it's a horizontal move
		//	gameObjects.player2.Steer(e.KeyData);
		//}
		//else if (e.KeyData == Keys.P)
		//{
		//	//then it's a pause
		//	gameObjects.Paused = !gameObjects.Paused;
		//}
		//else  // network game
		//{
		//	if (localPlayerIsHost)
		//	{
		//		if (e.KeyData == Keys.Space)
		//		{
		//			gameObjects.player1.Shoot(timeElapsed);
		//		}
		//		else if (e.KeyData == Keys.A || e.KeyData == Keys.D)
		//		{
		//			//then it's a horizontal move
		//			gameObjects.player1.Steer(e.KeyData);
		//		}
		//		else if (e.KeyData == Keys.W || e.KeyData == Keys.S)
		//		{
		//			//then it's a vertical move
		//			gameObjects.player1.Steer(e.KeyData);
		//		}
		//	}
		//	else
		//	{

		//		if (e.KeyData == Keys.Return)
		//		{
		//			gameObjects.player2.Shoot(timeElapsed);
		//		}
		//		else if (e.KeyData == Keys.Left || e.KeyData == Keys.Right)
		//		{
		//			//then it's a horizontal move
		//			gameObjects.player2.Steer(e.KeyData);
		//		}
		//		else if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
		//		{
		//			//then it's a vertical move
		//			gameObjects.player2.Steer(e.KeyData);

		//		}
		//	}
		//}


		//else  // network game
		//		{
		//			if (localPlayerIsHost)
		//			{
		//				switch (e.KeyData)
		//				{
		//					case Keys.Right:
		//						if (gameObjects.Player1.Jet.Pos_x<pictureBox1.Width)
		//							gameObjects.Player1.Jet.Pos_x += gameObjects.Player1.Jet.Speed;
		//						break;
		//					case Keys.Down:
		//						if (gameObjects.Player1.Jet.Pos_y + gameObjects.Player1.Jet.Height<pictureBox1.Height)
		//							gameObjects.Player1.Jet.Pos_y += gameObjects.Player1.Jet.Speed;
		//						break;
		//					case Keys.Left:
		//						if (gameObjects.Player1.Jet.Pos_x > pictureBox1.Width / 2 + gameObjects.Player1.Jet.Width + 2 * gameObjects.Player1.Jet.Cockpit_size)
		//							gameObjects.Player1.Jet.Pos_x -= gameObjects.Player1.Jet.Speed;
		//						break;
		//					case Keys.Up:
		//						if (gameObjects.Player1.Jet.Pos_y > 0)
		//							gameObjects.Player1.Jet.Pos_y -= gameObjects.Player1.Jet.Speed;
		//						break;
		//					case Keys.Return:
		//						if (gameObjects.Player1.Ammo != 0)
		//						{
		//							Bullet1 bullet = new Bullet1(gameObjects.Player1.Jet.Pos_x + gameObjects.Player1.Jet.Width + gameObjects.Player1.Jet.Cockpit_size, gameObjects.Player1.Jet.Pos_y + gameObjects.Player1.Jet.Height / 2);
		//gameObjects.Player1.Bulletlist.Add(bullet);
		//							gameObjects.Player1.Ammo--;
		//						}
		//						break;
		//				}
		//			}
		//			else
		//			{
		//				switch (e.KeyData)
		//				{
		//					case Keys.Right:
		//						if (gameObjects.Player2.Jet.Pos_x<pictureBox1.Width)
		//							gameObjects.Player2.Jet.Pos_x += gameObjects.Player2.Jet.Speed;
		//						break;
		//					case Keys.Down:
		//						if (gameObjects.Player2.Jet.Pos_y + gameObjects.Player2.Jet.Height<pictureBox1.Height)
		//							gameObjects.Player2.Jet.Pos_y += gameObjects.Player2.Jet.Speed;
		//						break;
		//					case Keys.Left:
		//						if (gameObjects.Player2.Jet.Pos_x > pictureBox1.Width / 2 + gameObjects.Player2.Jet.Width + 2 * gameObjects.Player2.Jet.Cockpit_size)
		//							gameObjects.Player2.Jet.Pos_x -= gameObjects.Player2.Jet.Speed;
		//						break;
		//					case Keys.Up:
		//						if (gameObjects.Player2.Jet.Pos_y > 0)
		//							gameObjects.Player2.Jet.Pos_y -= gameObjects.Player2.Jet.Speed;
		//						break;
		//					case Keys.Return:
		//						if (gameObjects.Player2.Ammo != 0)
		//						{
		//							gameObjects.Player2.Fired = true;
		//							gameObjects.Player2.Ammo--;
		//						}
		//						break;
		//				}
		//			}
		//		}

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
			gameObjects.control.Release(e.Button);
		}

		#endregion

		private void Game_FormClosing(object sender, FormClosingEventArgs e)
		{
			gameObjects.AbortGame();
		}

		#region Local Game

		private void humanVsHumanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			gameObjects = new ClsGameObjects(pbxWorld.Size);
			//ClsGameObjects.FrameInterval = 40;
			StartLocalGame();
		}

		private void humanVsBotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			gameObjects = new ClsGameObjects(pbxWorld.Size);

			Point p2Start = new Point(gameObjects.WinSize.Width - 100, gameObjects.WinSize.Height / 2);
			gameObjects.ReplacePlayer2(new Bot4("Bot4", 100, 300, p2Start, Color.Orange, gameObjects));

			StartLocalGame();
		}

		private void botVsBotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			gameObjects = new ClsGameObjects(pbxWorld.Size);

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

			var serv = new BinaryServerFormatterSinkProvider
			{
				TypeFilterLevel = TypeFilterLevel.Full
			};

			var clie = new BinaryClientFormatterSinkProvider();

			// Creating the IDictionary to set the port on the channel instance.
			IDictionary props = new Hashtable
			{
				["port"] = 8085
			};
			// Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.)
			serverChan = new TcpChannel(props, clie, serv);

			//serverChan = new TcpChannel(8085);
			ChannelServices.RegisterChannel(serverChan, false);
			gameObjects = new ClsGameObjects(pbxWorld.Size);
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(ClsGameObjects), "GameObjects", WellKnownObjectMode.Singleton);

			ObjRef or = RemotingServices.Marshal(gameObjects, "GameObjects");
			//or.GetObjectData( ;
			localGame = false;
			localPlayerIsHost = true;
			while (true)
			{
				if (gameObjects.Connected == true)
				{
					gameObjects.StartServer();
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

		private void joinNetworkGame(string IPAdress)
		{
			Text = "Planetes Client";
			var serv = new BinaryServerFormatterSinkProvider
			{
				TypeFilterLevel = TypeFilterLevel.Full
			};

			var clie = new BinaryClientFormatterSinkProvider();

			// Creating the IDictionary to set the port on the channel instance.
			IDictionary props = new Hashtable
			{
				["port"] = 0
			};
			// Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.)
			clientChan = new TcpChannel(props, clie, serv);
			ChannelServices.RegisterChannel(clientChan, false);
			gameObjects = (ClsGameObjects)Activator.GetObject(typeof(ClsGameObjects), "tcp://" + IPAdress + ":8085/GameObjects");
			localGame = false;
			localPlayerIsHost = false;
			gameObjects.Connected = true;
			StartGraphics();
		}

		private void disconnect()
		{
			ChannelServices.UnregisterChannel(clientChan);
			clientChan = null;
		}

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


