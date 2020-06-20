using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GameObjects
{

	public class ClsGameObjects : MarshalByRefObject
	{
		public static int FrameInterval = 40; // default. If you want to Change it, do it from outside
		//public static ClsGameObjects theObject;
		public bool Connected { get; set; }
		public bool ServerClosed { get; set; }
		public List<Player> players { get;private set; }
		public Player player1 { get; set; }
		public Player player2 { get; set; }
		public Size WinSize { get; set; }
		//public int WinSize_y { get; set; }
		public List<Astroid> Astroids { get; set; }
		public List<Wall> Walls { get; set; }
		public bool Paused { get; set; }
		public ControlPanel control { get; set; }
		public bool GameOver { get; private set; } = true;
		public Thread thrdGameLoop;
		public Player Winner;
		int timeElapsed; 

		public ClsGameObjects(Size winSize)
		{

			WinSize = winSize;
			//WinSize_y = winSize_y;

			Walls = new List<Wall>();
			Brush wallBrush = Brushes.Magenta;
			int ww = 20; //wallwidth

			Walls.Add(new Wall(wallBrush, new Point(0, 0), new Point(winSize.Width, 0)));
			Walls.Add(new Wall(wallBrush, new Point(0, 0), new Size(ww, winSize.Height)));
			Walls.Add(new Wall(wallBrush, new Point(0, winSize.Height - ww), new Size(winSize.Width, ww)));
			Walls.Add(new Wall(wallBrush, new Point(winSize.Width - ww, 0), new Size(ww, winSize.Height)));

			Walls.Add(new Wall(wallBrush, new Point(winSize.Width/2, 100), new Size(ww, 100)));

			Walls.Add(new Wall(wallBrush, new Point(100, 100), new Point(200, 200), ww));

			Point p1Start = new Point(100, winSize.Height / 2);
			player1 = new Player("Player1", 100, 300, p1Start, Color.Blue, this);

			Point p2Start = new Point(winSize.Width - 150, winSize.Height / 2);
			player2 = new Player("Player2", 100, 300, p2Start, Color.Red, this);

			player1.Enemy = player2;
			player2.Enemy = player1;

			control = new ControlPanel();
			control.bindWASDto(player1);
			control.bindMouse(MouseButtons.Left, player1, HOTAS.Shoot);
			control.bindARROWSto(player2);

			players = new List<Player>(){ player1,	player2};
			Astroids = new List<Astroid>();
			//theObject = this;
			Connected = false;
			ServerClosed = false;
			timeElapsed = 0;
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public void StartServer()
		{
			GameOver = false;

			thrdGameLoop = new Thread(GameLoop)
			{
				Name = "GameLoop"
			};
			thrdGameLoop.Start();

			//timeElapsed = 0;

		}

		public void EndGame(Player winner)
		{
			GameOver = true;
			Winner = winner;
		}


		public void AbortGame()
		{
			GameOver = true;
			if (thrdGameLoop != null)
				thrdGameLoop.Abort();
		}


		private void GameLoop()
		{
			while (!GameOver)
			{
				Thread.Sleep(FrameInterval);
				Frame();
			}
		}

		private void Frame()
		{
			if (Paused) return;
			lock (this)
			{
				player1.Move();
				player2.Move();
			}

			Console.WriteLine(player1.Jet.ToString());


			player1.Shoot(timeElapsed);
			player2.Shoot(timeElapsed);

			//pbPl1Ammo.Value = gameObjects.player1.Ammo;
			//pbPl1Hlth.Value = gameObjects.player1.Health;
			//pbPl2Ammo.Value = gameObjects.player2.Ammo;
			//pbPl2Hlth.Value = gameObjects.player2.Health;
			//if (!localGame && !localPlayerIsHost)
			//{
			//	if (gameObjects.ServerClosed)
			//	{
			//		disconnect();
			//		Close();
			//	}
			//}
			//else
			//{
			if (!GameOver)
			{
				//Move player 1 bullets
				lock (this)
				{
					player1.Bulletlist.ForEach(b => b.Move(this));
					player1.Bulletlist.RemoveAll(b => b.HasHit);
				}
				//Move player 2 bullets
				lock (this)
				{
					player2.Bulletlist.ForEach(b => b.Move(this));
					player2.Bulletlist.RemoveAll(b => b.HasHit);
				}
				//Move asteroids
				lock (this)
				{
					Astroids.ForEach(b => b.Move(this));
					Astroids.RemoveAll(c => c.HasHit);
				}
				//Spawn asteroid after timeout
				if (timeElapsed % Astroid.Timeout == 0)
				{
					Astroid astroid = new Astroid(WinSize.Width, WinSize.Height);
					lock (this)
					{
						Astroids.Add(astroid);
					}
				}
				Player looser;
				if ((looser = players.FirstOrDefault(p => p.isDead)) != null)
				{
					EndGame(looser.Enemy);
				}

				timeElapsed++;
			}
			//else
			//{
			//	//stillOpen = false;
			//	//timer1.Enabled = false;
			//}
		}

		public void Draw(Graphics g)
		{ //gameObjects = (ClsGameObjects)Activator.GetObject(typeof(ClsGameObjects), "tcp://127.0.0.1:8085/GameObjects");
			
			foreach (Wall w in Walls)
			{
				w.Draw(g);
			}

			lock (this)
			{
				player1.Jet.Draw(g);
				player2.Jet.Draw(g);
			}

			lock (this)
			{
				foreach (Bullet b in player1.Bulletlist)
				{
					b.Draw(g);
				}
				foreach (Bullet b in player2.Bulletlist)
				{
					b.Draw(g);
				}
			}

			lock (this)
			{
				foreach (Astroid ast in Astroids)
				{
					//make it iDrawable interface
					ast.Draw(g);
				}
			}
		}


		public void ReplacePlayer2(Bot bot)
		{
			player2 = bot;
			player1.Enemy = player2;
			player2.Enemy = player1;
			player2.Jet.Aim = new Vector(0, WinSize.Height / 2);
			players[1] = player2;
		}
		public void ReplacePlayer1(Bot bot)
		{
			player1 = bot;
			player1.Enemy = player2;
			player2.Enemy = player1;
			player1.Jet.Aim = new Vector(0, WinSize.Height / 2);
			players[0] = player1;
		}
	}

	public static class RectExtension
	{
		public static Point Center(this Rectangle rect)
		{
			return new Point(rect.Location.X + rect.Size.Width / 2, rect.Location.Y + rect.Size.Height / 2);
		}

		public static Point Center(this RectangleF rect)
		{
			return new Point((int)(rect.Location.X + rect.Size.Width / 2), (int)(rect.Location.Y + rect.Size.Height / 2));
		}
	}

}
