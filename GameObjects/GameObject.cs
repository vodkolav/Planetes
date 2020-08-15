using Newtonsoft.Json;
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

	public class ClsGameObjects 
	{
		public static int FrameInterval = 40; // default. If you want to Change it, do it from outside

		//public static ClsGameObjects theObject;

		public List<Player> players { get;set; }
		[JsonIgnore]
		public Player player1 { get => players[0]; set => players[0] = value; }
		[JsonIgnore]
		public Player player2 { get => players[1]; set => players[1] = value; }
		public Size WinSize { get; set; }
		//public int WinSize_y { get; set; }
		//[JsonIgnore]
		public List<Astroid> Astroids { get; set; }
		[JsonIgnore]
		public List<Wall> Walls { get; set; }
		public bool Paused { get; set; }
		[JsonIgnore]
		public ControlPanel control { get; set; }
		public bool GameOver { get; set; } = true;
		
		public Player Winner;
		public int timeElapsed;

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
			Player player1 = new Player("Player1", 1000, 300, p1Start, Color.Blue, this);

			Point p2Start = new Point(winSize.Width - 150, winSize.Height / 2);
			Player player2 = new Player("Player2", 1000, 300, p2Start, Color.Red, this);

			player1.Enemy = player2;
			player2.Enemy = player1;

			players = new List<Player>(){ player1,	player2};
			Astroids = new List<Astroid>();
			//theObject = this;
			timeElapsed = 0;
		}

		public bool Frame()
		{
			if (Paused) return true;
			lock (this)
			{
				player1.Move();
				player2.Move();
			}

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
					Over(looser.Enemy);
				}

				timeElapsed++;
				return true;
			}
            else
            {
				return false;
            }
        }

		public void Over(Player winner)
		{
			GameOver = true;
			Winner = winner;
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
		}
		public void ReplacePlayer1(Bot bot)
		{
			player1 = bot;
			player1.Enemy = player2;
			player2.Enemy = player1;
			player1.Jet.Aim = new Vector(0, WinSize.Height / 2);
		}
	}	
}
