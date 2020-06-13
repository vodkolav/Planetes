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
		public List<Astroid> AstroidList { get; set; }
		public List<Wall> Walls { get; set; }
		public bool Paused { get; set; }
		public ControlPanel control { get; set; }


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
			AstroidList = new List<Astroid>();
			//theObject = this;
			Connected = false;
			ServerClosed = false;
		}

		public override object InitializeLifetimeService()
		{
			return null;
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
