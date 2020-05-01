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
		public static int FrameRate = 10;
		public static ClsGameObjects theObject;
		public bool Connected { get; set; }
		public bool ServerClosed { get; set; }
		public Player1 Player1 { get; set; }
		public Player2 Player2 { get; set; }
		public int WinSize_x { get; set; }
		public int WinSize_y { get; set; }
		public List<Astroid> AstroidList { get; set; }
		public List<Wall> Walls { get; set; }
		public bool Paused { get; set; }


		public ClsGameObjects(int winSize_x, int winSize_y)
		{

			WinSize_x = winSize_x;
			WinSize_y = winSize_y;

			Walls = new List<Wall>();
			Brush wallBrush = Brushes.Magenta;
			Walls.Add(new Wall(wallBrush, new Point(0, 0), new Size(winSize_x, 5)));
			Walls.Add(new Wall(wallBrush, new Point(0, 0), new Size(5, winSize_y)));
			Walls.Add(new Wall(wallBrush, new Point(0, winSize_y - 5), new Size(winSize_x, 5)));
			Walls.Add(new Wall(wallBrush, new Point(winSize_x - 5, 0), new Size(5, winSize_y)));

			Walls.Add(new Wall(wallBrush, new Point(winSize_x - 50, 500), new Size(5, 100)));

			Walls.Add(new Wall(wallBrush, new Point(100, 100), new Point(200, 200)));


			Player1 = new Player1("Player1", 20, 300, winSize_x, winSize_y, this);
			Player2 = new Player2("Player2", 20, 300, winSize_x, winSize_y, this);
			AstroidList = new List<Astroid>();
			theObject = this;
			Connected = false;
			ServerClosed = false;
		}

		public override object InitializeLifetimeService()
		{
			return null;
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
