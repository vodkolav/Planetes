using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GameObjects
{
	public abstract class Jet : MarshalByRefObject
	{
		public int LastFired { get; set; }
		public int FireRate { get; set; }
		public int Acceleration { get; set; }
		//protected int WWidth { get; set; }
		//protected int WHeight { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public int Pos_x { get; set; }
		public int Pos_y { get; set; }
		public double Bearing { get; set; }
		public Point Aim { get; set; }
		public Size Cockpit_size { get; set; }
		public Brush Color { get; set; }

		public Rectangle Hull
		{
			get
			{
				return new Rectangle(new Point(Pos_x, Pos_y), new Size(30, 24));
			}
		}

		public Rectangle Cockpit
		{
			get
			{
				return new Rectangle(new Point(Pos_x, Pos_y) + new Size(30, 6), Cockpit_size);
			}
		}

		private int _speed_x;

		public int Speed_x
		{
			get { return _speed_x; }
			set
			{
				if (Math.Abs(value) <= 20)
					_speed_x = value;
			}
		}

		private int _speed_y;

		public int Speed_y
		{
			get { return _speed_y; }
			set
			{
				if (Math.Abs(value) <= 10)
					_speed_y = value;
			}
		}


		//public Point Pos { get; set; }
		public Jet(Point start, Brush color, int wwidth, int wheight)
		{
			//WWidth = wwidth;
			//WHeight = wheight;
			Pos_x = start.X;
			Pos_y = start.Y;
			Color = color;
			Cockpit_size = new Size(12, 12);
			Acceleration = 1;
			FireRate = 10;
		}


		public double Dist(Astroid a)
		{
			return Math.Sqrt((a.Pos_x - Pos_x) ^ 2 + (a.Pos_y - Pos_y) ^ 2);
		}


		public bool Hit(Wall w)
		{
			return false;
		}


		public virtual double Dist(Bullet b)
		{
			return Math.Sqrt((b.Pos_x - Pos_x) ^ 2 + (b.Pos_y - Pos_y) ^ 2);
		}


		public void Move(ClsGameObjects gO)
		{
			Wall coll = gO.Walls.FirstOrDefault(w => w.IntersectsWith(Hull));
			if (coll != null)
			{
				Bounce(coll);
			}
			Pos_x += Speed_x;
			Pos_y += Speed_y;

			//Rotate
			Vector diff = new Vector(Aim) - new Vector(Hull.Center());
			Bearing = diff.Angle;
			Console.WriteLine("X:{0}| Y:{1} | Angle:{2})", diff.X, diff.X, Bearing);

		}

		public void Bounce(Wall w)
		{
			Vector velocity = new Vector(Speed_x, Speed_y);
			Vector normal = new Vector(Hull.Center()) - w.Reflect(Hull);
			normal.Normalize();
			Vector reflection = velocity - 2 * velocity.Dot(normal) * normal;
			Speed_x = (int)reflection.X;
			Speed_y = (int)reflection.Y;
		}

		public abstract void Steer(Keys dir);

		public abstract void Shoot(Player player, int timeElapsed);

		public void Draw(Graphics g)
		{
			using (Matrix m = new Matrix())
			{
				m.RotateAt((float)Bearing, Hull.Center());
				g.Transform = m;
				g.FillRectangle(Color, Hull);
				g.FillRectangle(Brushes.Gray, Cockpit);
				g.ResetTransform();

			}
		}

		internal void Turn(Point aim)
		{
			Aim = aim;
		}

		//public void DoSomeShit()
		//{
		//	Point point = new Point(60, 10);

		//	// Assume that the variable "point" contains the location of the
		//	// most recent mouse click.
		//	// To simulate a hit, assign (60, 10) to point.
		//	// To simulate a miss, assign (0, 0) to point.

		//	SolidBrush solidBrush = new SolidBrush(System.Drawing.Color.Black);
		//	Region region1 = new Region(new Rectangle(50, 0, 50, 150));
		//	Region region2 = new Region(new Rectangle(0, 50, 150, 50));
		//	//region1.IsVisible(,)
		//	// Create a plus-shaped region by forming the union of region1 and
		//	// region2.
		//	// The union replaces region1.
		//	region1.Union(region2);

		//	if (region1.IsVisible(point, e.Graphics))
		//	{
		//		// The point is in the region. Use an opaque brush.
		//		solidBrush.Color = Color.FromArgb(255, 255, 0, 0);
		//	}
		//	else
		//	{
		//		// The point is not in the region. Use a semitransparent brush.
		//		solidBrush.Color = Color.FromArgb(64, 255, 0, 0);
		//	}

		//	e.Graphics.FillRegion(solidBrush, region1);

		//}

	}

	public class Jet1 : Jet
	{
		public Jet1(Point start, int wwidth, int wheight) : base(start, Brushes.Blue, wwidth, wheight) { }

		public override void Steer(Keys dir)
		{
			switch (dir)
			{
				case Keys.D:
					Speed_x += Acceleration;
					break;
				case Keys.S:
					Speed_y += Acceleration;
					break;
				case Keys.A:
					Speed_x -= Acceleration;
					break;
				case Keys.W:
					Speed_y -= Acceleration;
					break;
			}
		}

		public override void Shoot(Player player, int timeElapsed)
		{
			if (player.Ammo != 0 && timeElapsed > LastFired + FireRate)
			{
				LastFired = timeElapsed;
				Bullet1 bullet = new Bullet1(Pos_x + Width + Cockpit_size.Width, Pos_y + Height / 2);
				lock (player.GameState)
				{
					player.Bulletlist.Add(bullet);
				}
				player.Ammo--;
			}
		}

	}

	public class Jet2 : Jet
	{
		public Jet2(Point start, int wwidth, int wheight) : base(start, Brushes.Red, wwidth, wheight) { }

		//public override void Move(Keys dir)
		//{
		//    switch (dir)
		//    {
		//        case Keys.Right:
		//            if (Pos_x < WWidth)// - Width - Cockpit_size)
		//                Pos_x += Acceleration;
		//            break;
		//        case Keys.Down:
		//            if (Pos_y < WHeight - Height)
		//                Pos_y += Acceleration;
		//            break;
		//        case Keys.Left:
		//            if (Pos_x > WWidth/2 + Width + Cockpit_size)
		//                Pos_x -= Acceleration;
		//            break;
		//        case Keys.Up:
		//            if (Pos_y > 0)
		//                Pos_y -= Acceleration;
		//            break;
		//    }

		//}

		public override void Steer(Keys dir)
		{
			switch (dir)
			{
				case Keys.Right:
					Speed_x += Acceleration;
					break;
				case Keys.Down:
					Speed_y += Acceleration;
					break;
				case Keys.Left:
					Speed_x -= Acceleration;
					break;
				case Keys.Up:
					Speed_y -= Acceleration;
					break;
			}
		}

		public override void Shoot(Player player, int timeElapsed)
		{
			if (player.Ammo != 0 && timeElapsed > LastFired + FireRate)
			{
				LastFired = timeElapsed;
				Bullet2 bullet = new Bullet2(Pos_x - Width - Cockpit_size.Width, Pos_y + Height / 2);

				lock (player.GameState)
				{
					player.Bulletlist.Add(bullet);
				}
				player.Ammo--;
			}
		}

		public override double Dist(Bullet b)
		{
			//Bullet1 b1 = (Bullet1)b;
			if (b.Pos_x < Pos_x)
			{
				return Math.Sqrt((b.Pos_x - Pos_x) ^ 2 + (b.Pos_y - Pos_y) ^ 2);
			}
			else
			{
				return 1000.0;
			}
		}
	}

}
