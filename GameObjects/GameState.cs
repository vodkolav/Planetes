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
	public class GameState 
	{		
		public static TimeSpan FrameInterval = new TimeSpan(0, 0, 0, 0, 15); // default. If you want to Change it, do it from outside

		public bool GameOn { get; set; } = false;

		public bool Paused { get; set; }
		
		public int frameNum;
		public Size WinSize { get; set; }

		public Player Winner;
		
		public List<Player> players { get;set; }

		[JsonIgnore]
		public Player player1 { get => players[0]; set => players[0] = value; }

		[JsonIgnore]
		public Player player2 { get => players[1]; set => players[1] = value; }	

		public List<Astroid> Astroids { get; set; }

		[JsonIgnore]
		public List<Wall> Walls { get; set; }

		[JsonIgnore]
		public ControlPanel control { get; set; }

		public GameState(Size winSize)
		{
			WinSize = winSize;

			Walls =  new Map(winSize).LoadDefault2();

			Point p1Start = new Point(100, winSize.Height / 2);
			Player player1 = new Player("Player1", 1000, 300, p1Start, Color.Blue, this);

			Point p2Start = new Point(winSize.Width - 150, winSize.Height / 2);
			Player player2 = new Player("Player2", 1000, 300, p2Start, Color.Red, this);

			player1.Enemy = player2;
			player2.Enemy = player1;

			players = new List<Player>(){ player1,	player2};
			Astroids = new List<Astroid>();			
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
					Astroid astroid = new Astroid(WinSize);
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
		{ 
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
