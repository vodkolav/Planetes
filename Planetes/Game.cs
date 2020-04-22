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

namespace Planetes
{
	public partial class Game : Form
	{
		private Bitmap bmp;
		private Pen p1;
		private Graphics g;
		private ClsGameObjects gameObjects;
		private Thread thrdDrawGraphics;
		private bool stillOpen;
		private int timeElapsed;
		private bool localGame;
		private bool localPlayerIsHost;



		// network
		private TcpChannel serverChan, clientChan;

		delegate void RefreshBitmapDelegate();
		delegate void RefreshProgessBarDelegate();

		public Game()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.FixedHeight | ControlStyles.FixedWidth | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
		}

		private void startGame()
		{
			stillOpen = true;
			bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			g = Graphics.FromImage(bmp);
			pbPl1Ammo.Visible = true;
			pbPl1Ammo.Maximum = gameObjects.Player1.MaxAmmo;
			pbPl1Hlth.Visible = true;
			pbPl1Hlth.Maximum = gameObjects.Player1.MaxHealth;
			pbPl2Ammo.Visible = true;
			pbPl2Ammo.Maximum = gameObjects.Player2.MaxAmmo;
			pbPl2Hlth.Visible = true;
			pbPl2Hlth.Maximum = gameObjects.Player2.MaxHealth;
			pictureBox1.Image = bmp;
			thrdDrawGraphics = new Thread(DrawGraphics);
			thrdDrawGraphics.Name = "DrawGraphics";
			thrdDrawGraphics.Start();
			GameOver.End = false;
			{
				timer1.Interval = ClsGameObjects.FrameRate;
				timer1.Start();
				timeElapsed = 0;
			}
		}

		private void DrawGraphics()
		{
			while (stillOpen)
			{
				//gameObjects = (ClsGameObjects)Activator.GetObject(typeof(ClsGameObjects), "tcp://127.0.0.1:8085/GameObjects");
				p1 = new Pen(Color.White, 5);
				//p2 = new Pen(Color.Yellow, 2);
				g.FillRectangle(Brushes.Black, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
				//g.DrawLine(p1, this.Width / 2, 0, this.Width / 2, this.Height);



				foreach (Wall w in gameObjects.Walls)
				{
					w.Draw(g);
				}

				gameObjects.Player1.Jet.Draw(g);
				gameObjects.Player2.Jet.Draw(g);
				//g.FillRectangle(Brushes.Blue, gameObjects.Player1.Jet.Pos_x, gameObjects.Player1.Jet.Pos_y, gameObjects.Player1.Jet.Width, gameObjects.Player1.Jet.Height);
				//g.FillRectangle(Brushes.Gray, gameObjects.Player1.Jet.Pos_x + gameObjects.Player1.Jet.Width, gameObjects.Player1.Jet.Pos_y + gameObjects.Player1.Jet.Height / 2 - gameObjects.Player1.Jet.Cockpit_size / 2, gameObjects.Player1.Jet.Cockpit_size, gameObjects.Player1.Jet.Cockpit_size);
				//g.FillRectangle(Brushes.Blue, gameObjects.Player2.Jet.Pos_x - gameObjects.Player2.Jet.Width, gameObjects.Player2.Jet.Pos_y, gameObjects.Player2.Jet.Width, gameObjects.Player2.Jet.Height);
				//g.FillRectangle(Brushes.Gray, gameObjects.Player2.Jet.Pos_x - gameObjects.Player2.Jet.Width - gameObjects.Player2.Jet.Cockpit_size, gameObjects.Player2.Jet.Pos_y + gameObjects.Player2.Jet.Height / 2 - gameObjects.Player2.Jet.Cockpit_size / 2, gameObjects.Player2.Jet.Cockpit_size, gameObjects.Player2.Jet.Cockpit_size);
				//lock (gameObjects)
				//{
				//	foreach (Bullet b  in gameObjects.Player1.Bulletlist)
				//	{
				//		if (!b.HasHit)
				//		{
				//			g.DrawLine(b.Pen, b.Pos_x, b.Pos_y, b.Pos_x + b.Size, b.Pos_y);
				//		}
				//	}
				//	foreach (Bullet b in gameObjects.Player2.Bulletlist)
				//	{						
				//		//b.Draw(g);
				//		if (!b.HasHit)
				//		{
				//			g.DrawLine(b.Pen, b.Pos_x, b.Pos_y, b.Pos_x + b.Size, b.Pos_y);
				//		}
				//	}
				//	foreach (Astroid a in gameObjects.AstroidList)
				//	{
				//		//ast.Draw(g);
				//		if (!a.HasHit)
				//			g.FillEllipse(a.Color, a.Pos_x, a.Pos_y, a.Size, a.Size);
				//	}


				//	//debug
				//	//try
				//	//{
				//	//	if (P1keyVert != null || P1keyHorz != null)
				//	//	{
				//	//		Console.WriteLine(gameObjects.Player1.Jet.Pos_x.ToString() + ", " + gameObjects.Player1.Jet.Pos_y.ToString());
				//	//		lastx = gameObjects.Player1.Jet.Pos_x;
				//	//		lasty = gameObjects.Player1.Jet.Pos_y;
				//	//	}
				//	//}
				//	//catch
				//	//{
				//	//	Console.WriteLine("oops");
				//	//}
				//}
				pictureBox1.Image = bmp;
				if (pictureBox1 != null)
					pictureBox1.Invoke(new RefreshBitmapDelegate(pictureBox1.Refresh));
			}
		}


		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			gameObjects.Player1.Release(e.KeyData);
			gameObjects.Player2.Release(e.KeyData);
		}

		//private void playersAct(object key)
		//{
		//  while (!released)
		//	  {
		//		gameObjects.Player1.Action((Keys)key);
		//		gameObjects.Player2.Action((Keys)key);
		//	  }
		//}


		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (GameOver.End == false)
			{
				if (localGame) // local game
				{

					if (e.KeyData == Keys.Space)
					{
						gameObjects.Player1.Steer(e.KeyData);
					}
					else if (e.KeyData == Keys.Return)
					{
						gameObjects.Player2.Steer(e.KeyData);
					}
					//else if (e.KeyData == Keys.A || e.KeyData == Keys.D || e.KeyData == Keys.W || e.KeyData == Keys.S )
					else if (new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D }.Contains(e.KeyData))
					{
						//then it's a horizontal move
						gameObjects.Player1.Steer(e.KeyData);
					}
					else if (e.KeyData == Keys.Left || e.KeyData == Keys.Right || e.KeyData == Keys.Up || e.KeyData == Keys.Down)
					{
						//then it's a horizontal move
						gameObjects.Player2.Steer(e.KeyData);
					}
					else if (e.KeyData == Keys.P)
					{
						//then it's a horizontal move
						gameObjects.Paused = !gameObjects.Paused;
					}
				}

				else  // network game
				{
					if (localPlayerIsHost)
					{
						if (e.KeyData == Keys.Space)
						{
							gameObjects.Player1.Shoot(timeElapsed);
						}
						else if (e.KeyData == Keys.A || e.KeyData == Keys.D)
						{
							//then it's a horizontal move
							gameObjects.Player1.Steer(e.KeyData);
						}
						else if (e.KeyData == Keys.W || e.KeyData == Keys.S)
						{
							//then it's a vertical move
							gameObjects.Player1.Steer(e.KeyData);
						}
					}
					else
					{

						if (e.KeyData == Keys.Return)
						{
							gameObjects.Player2.Shoot(timeElapsed);
						}
						else if (e.KeyData == Keys.Left || e.KeyData == Keys.Right)
						{
							//then it's a horizontal move
							gameObjects.Player2.Steer(e.KeyData);
						}
						else if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
						{
							//then it's a vertical move
							gameObjects.Player2.Steer(e.KeyData);

						}
					}
				}
			}
		}

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

		private void timer1_Tick(object sender, EventArgs e)
		{
			//if (P1keyHorz!=null)
			//{
			//	gameObjects.Player1.Move(P1keyHorz.Value);				
			//}
			//if (P2keyHorz != null)
			//{				
			//	gameObjects.Player2.Move(P2keyHorz.Value);
			//}
			//if (P1keyVert != null)
			//{
			//	gameObjects.Player1.Move(P1keyVert.Value);				
			//}
			//if (P2keyVert != null)
			//{				
			//	gameObjects.Player2.Move(P2keyVert.Value);
			//}
			if (gameObjects.Paused) return;
			gameObjects.Player1.Move();
			gameObjects.Player2.Move();

			gameObjects.Player1.Shoot(timeElapsed);
			gameObjects.Player2.Shoot(timeElapsed);

			pbPl1Ammo.Value = gameObjects.Player1.Ammo;
			pbPl1Hlth.Value = gameObjects.Player1.Health;
			pbPl2Ammo.Value = gameObjects.Player2.Ammo;
			pbPl2Hlth.Value = gameObjects.Player2.Health;
			if (!localGame && !localPlayerIsHost)
			{
				if (gameObjects.ServerClosed)
				{
					disconnect();
					Close();
				}
			}
			else
			{
				if (GameOver.End == false)
				{
					//for (int i = 0; i < gameObjects.Player1.Bulletlist.Count; i++)
					//{
					lock (gameObjects)
					{
						gameObjects.Player1.Bulletlist.ForEach(b => b.Move(gameObjects));
						gameObjects.Player1.Bulletlist.RemoveAll(b => b.HasHit);
					}

					lock (gameObjects)
					{
						gameObjects.Player2.Bulletlist.ForEach(b => b.Move(gameObjects));
						gameObjects.Player2.Bulletlist.RemoveAll(b => b.HasHit);
					}


					//for (int i = 0; i < gameObjects.Player2.Bulletlist.Count; i++)
					//{
					//	Bullet b = gameObjects.Player2.Bulletlist[i];
					//	b.Update(gameObjects);
					//}
					//gameObjects.Player2.Bulletlist.RemoveAll(b => b.HasHit);
					//for (int i = 0; i < gameObjects.AstroidList.Count; i++)

					lock (gameObjects)
						gameObjects.AstroidList.ForEach(b => b.Update(gameObjects));

					if (timeElapsed % Astroid.Timeout == 0)
					{
						Astroid astroid = new Astroid(Width, Height);
						lock (gameObjects)
						{
							gameObjects.AstroidList.Add(astroid);
							gameObjects.AstroidList.RemoveAll(c => c.HasHit);
						}
					}
					if (gameObjects.Player2.Fired == true)
					{
						Bullet2 bullet = new Bullet2(gameObjects.Player2.Jet.Pos_x - gameObjects.Player2.Jet.Width - gameObjects.Player2.Jet.Cockpit_size.Width, gameObjects.Player2.Jet.Pos_y + gameObjects.Player2.Jet.Height / 2);
						gameObjects.Player2.Bulletlist.Add(bullet);
						gameObjects.Player2.Fired = false;
					}
					timeElapsed++;
				}
				else { stillOpen = false; timer1.Enabled = false; }
			}
			Thread.Sleep(20);
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			stillOpen = false;
			if (thrdDrawGraphics != null)
				thrdDrawGraphics.Abort();
			if (!localGame && localPlayerIsHost) stopServer();
		}


		private void hostNetworkGame()
		{
			Text = "World of Starcraft (Server)";
			serverChan = new TcpChannel(8085);
			ChannelServices.RegisterChannel(serverChan, false);
			gameObjects = new ClsGameObjects(pictureBox1.Width, pictureBox1.Height);
			//RemotingConfiguration.RegisterWellKnownServiceType(typeof(ClsGameObjects), "GameObjects", WellKnownObjectMode.Singleton);

			ObjRef or = RemotingServices.Marshal(gameObjects, "GameObjects");
			//or.GetObjectData( ;
			localGame = false;
			localPlayerIsHost = true;
			while (true)
			{
				if (gameObjects.Connected == true)
				{
					startGame();
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
			Text = "World of Starcraft (Client)";
			clientChan = new TcpChannel();
			ChannelServices.RegisterChannel(clientChan, false);
			gameObjects = (ClsGameObjects)Activator.GetObject(typeof(ClsGameObjects), "tcp://" + IPAdress + ":8085/GameObjects");
			localGame = false;
			localPlayerIsHost = false;
			gameObjects.Connected = true;
			startGame();
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

		private void humanVsHumanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			gameObjects = new ClsGameObjects(pictureBox1.Width, pictureBox1.Height);
			startGame();
		}

		private void humanVsBotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			localGame = true;
			gameObjects = new ClsGameObjects(pictureBox1.Width, pictureBox1.Height);
			gameObjects.Player2 = new Bot4("Bot4", 20, 300, pictureBox1.Width, pictureBox1.Height, gameObjects);
			startGame();
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
				catch (Exception ex) { MessageBox.Show("Could not connect to server!\n" + ex.Message); }
		}

		private void LocalGameToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
	}
}


