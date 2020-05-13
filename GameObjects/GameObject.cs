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
		public static int FrameRate = 20;
		public static ClsGameObjects theObject;
		public bool Connected { get; set; }
		public bool ServerClosed { get; set; }
		public Player player1 { get; set; }
		public Player player2 { get; set; }
		public int WinSize_x { get; set; }
		public int WinSize_y { get; set; }
		public List<Astroid> AstroidList { get; set; }
		public List<Wall> Walls { get; set; }
		public bool Paused { get; set; }
		public Controls control { get; set; }


		public ClsGameObjects(int winSize_x, int winSize_y)
		{

			WinSize_x = winSize_x;
			WinSize_y = winSize_y;

			Walls = new List<Wall>();
			Brush wallBrush = Brushes.Magenta;
			int ww = 20; //wallwidth

			Walls.Add(new Wall(wallBrush, new Point(0, 0), new Size(winSize_x, ww)));
			Walls.Add(new Wall(wallBrush, new Point(0, 0), new Size(ww, winSize_y)));
			Walls.Add(new Wall(wallBrush, new Point(0, winSize_y - ww), new Size(winSize_x, ww)));
			Walls.Add(new Wall(wallBrush, new Point(winSize_x - ww, 0), new Size(ww, winSize_y)));

			Walls.Add(new Wall(wallBrush, new Point(winSize_x/2, 400), new Size(ww, 100)));

			Walls.Add(new Wall(wallBrush, new Point(100, 100), new Point(200, 200), ww));

			Point p1Start = new Point(100, winSize_y / 2);
			player1 = new Player("Player1", 200, 300, p1Start, Color.Blue, this);

			Point p2Start = new Point(winSize_x - 100, winSize_y / 2);
			player2 = new Player("Player2", 200, 300, p2Start, Color.Red, this);

			player1.Enemy = player2;
			player2.Enemy = player1;

			control = new Controls();
			control.bindWASDto(player1);
			control.bindMouse(MouseButtons.Left, player1, HOTAS.Shoot);
			control.bindARROWSto(player2);

			AstroidList = new List<Astroid>();
			theObject = this;
			Connected = false;
			ServerClosed = false;
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}


		public void ReplacePlayerWith(Bot bot)
		{
			player2 = bot;
			player1.Enemy = player2;
			player2.Enemy = player1;
			player2.Jet.Aim = new Vector(0, WinSize_y / 2);
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

	

	static public class GameOver
	{
		static bool end = false;

		public static bool End
		{
			get { return GameOver.end; }
			set { GameOver.end = value; }
		}
		static public void Show(Player winner)
		{
			if (end == false)
			{
				end = true;
				MessageBox.Show(winner.Name + " wins!", @"Game Over! ");
			}
		}
	}
}
