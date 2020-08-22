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
		public static TimeSpan FrameInterval = new TimeSpan(0, 0, 0, 0, 15); // default. If you want to Change it, do it from outside

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
		public bool GameOn { get; set; } = false;
		
		public Player Winner;
		public int frameNum;

		public ClsGameObjects(Size winSize)
		{

			WinSize = winSize;
			//WinSize_y = winSize_y;		

			Walls =  new Map(winSize).LoadDefault2(Brushes.Magenta);

			Point p1Start = new Point(100, winSize.Height / 2);
			Player player1 = new Player("Player1", 1000, 300, p1Start, Color.Blue, this);

			Point p2Start = new Point(winSize.Width - 150, winSize.Height / 2);
			Player player2 = new Player("Player2", 1000, 300, p2Start, Color.Red, this);

			player1.Enemy = player2;
			player2.Enemy = player1;

			players = new List<Player>(){ player1,	player2};
			Astroids = new List<Astroid>();			
			//theObject = this;
			frameNum = 0;
		}

		public bool Frame()
		{
			if (Paused) return true;
			lock (this)
			{
				players.ForEach(p => p.Move());
				players.ForEach(p => p.Shoot(frameNum));
			}

			//player1.Shoot(timeElapsed);
			//player2.Shoot(timeElapsed);

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
			if (GameOn)
			{				
				//Move asteroids
				lock (this)
				{
					Astroids.ForEach(b => b.Move(this));
					Astroids.RemoveAll(c => c.HasHit);
				}
				//Spawn asteroid after timeout
				if (frameNum % Astroid.Timeout == 0)
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

				frameNum++;
				return true;
			}
            else
            {
				return false;
            }
        }

		public void Over(Player winner)
		{
			GameOn = false;
			Winner = winner;
		}

		public void Draw(Graphics g)
		{ //gameObjects = (ClsGameObjects)Activator.GetObject(typeof(ClsGameObjects), "tcp://127.0.0.1:8085/GameObjects");
			//make it iDrawable interface
			Walls.ForEach(w => w.Draw(g));			
			
			lock (this)
			{
				players.ForEach(p=>p.Draw(g));
			}		

			lock (this)
			{
				Astroids.ForEach(a => a.Draw(g));				
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
