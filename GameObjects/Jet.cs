using PolygonCollision;
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

		public Vector Pos { get => Hull.Center; }
		public int Pos_x { get; set; }
		public int Pos_y { get; set; }

		public Vector Bearing { get; set; } = new Vector(1, 0);
		public Point Aim { get; set; }
		public Size Cockpit_size { get; set; }
		public Brush Color { get; set; }

		public Polygon Hull{ get; set; }

		public Vector Gun { get { return Cockpit.Vertices[1]; } }

		public Polygon Cockpit { get; set;	}

		
		private Vector _speed;

		public Vector Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}


		public int Speed_x
		{
			get { return (int)_speed.X; }
			set
			{
				if (Math.Abs(value) <= 20)
					_speed.X = value;
			}
		}


		public int Speed_y
		{
			get { return (int)_speed.Y; }
			set
			{
				if (Math.Abs(value) <= 10)
					_speed.Y = value;
			}
		}


		//public Point Pos { get; set; }
		public Jet(Point start, Brush color)
		{
			//WWidth = wwidth;
			//WHeight = wheight;
			Hull = new Polygon();
			Hull.AddVertex(new Vector(50, 50));
			Hull.AddVertex(new Vector(80, 50));
			// for cockpit Hull.AddVertex(new Vector(100, 60));
			Hull.AddVertex(new Vector(80, 70));
			Hull.AddVertex(new Vector(50, 70));

			Cockpit = new Polygon();
			Cockpit.AddVertex(new Vector(80, 50));
			Cockpit.AddVertex(new Vector(100, 60));
			Cockpit.AddVertex(new Vector(80, 70));

			Offset(Vector.FromPoint(start));

			//Pos_x = start.X;
			//Pos_y = start.Y;
			Color = color;
			Acceleration = 1;
			FireRate = 10;
		}



		private void Offset(Vector destination)
		{
			Hull.Offset(destination);
			Cockpit.Offset(destination);
		}

		private void Rotate(Vector dir)
		{
			float diff = Bearing.Angle(dir);
			Bearing = dir;
			Hull.RotateAt(diff, Hull.Center);
			Cockpit.RotateAt(diff, Hull.Center);
		}


		public double Dist(Astroid a)
		{
			return (Pos - a.Pos).Magnitude;
		}


		public bool Hit(Wall w)
		{
			return false;
		}


		public virtual float Dist(Bullet b)
		{
			Vector diff = b.Pos - Pos;
			return diff.Magnitude;
		}

		internal bool Collides(Astroid a)
		{
			return Hull.Collides(a.Pos) || Cockpit.Collides(a.Pos);
		}

		public bool Collides(Bullet b)
		{
			return Hull.Collides(b.Pos) || Cockpit.Collides(b.Pos);
		}


		public void Move(ClsGameObjects gO)
		{
			PolygonCollisionResult r;
			foreach(Wall w in gO.Walls)
			{
				r = Hull.Collides(w.region, Speed);
				if (r.Intersect)
				{
					Bounce(r.translationAxis);
				}
			}
			Offset(Speed);

			//Rotate
			Vector dir = new Vector(Aim) - new Vector(Hull.Center);
			
			Rotate(dir);

		}


		[Obsolete]
		public void Bounce(Wall w)
		{
			Vector velocity = new Vector(Speed_x, Speed_y);
			Vector normal = new Vector(Hull.Center) - w.Reflect(Hull);
			normal.Normalize();
			Vector reflection = velocity - 2 * velocity.Dot(normal) * normal;
			Speed_x = (int)reflection.X;
			Speed_y = (int)reflection.Y;
		}

		public void Bounce(Vector normal)
		{
			Vector reflection = Speed - 2 * Speed.Dot(normal) * normal;
			Speed = reflection;
		}

		public abstract void Steer(Keys dir);

		public void Shoot(Player player, int timeElapsed)
		{
			if (player.Ammo != 0 && timeElapsed > LastFired + FireRate)
			{
				LastFired = timeElapsed;
				Bullet bullet = new Bullet(player);
				lock (player.GameState)
				{
					player.Bulletlist.Add(bullet);
				}
				player.Ammo--;
			}
		}

		public void Draw(Graphics g)
		{
			//using (Matrix m = new Matrix())
			//{
			//	m.RotateAt((float)Bearing, Hull.Center());
			//	g.Transform = m;
		
			Hull.Draw(g, Color);
			Cockpit.Draw(g,Brushes.Gray);
			//	g.ResetTransform();

			//}
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
		public Jet1(Point start) : base(start, Brushes.Blue) { }

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
	}

	public class Jet2 : Jet
	{
		public Jet2(Point start) : base(start, Brushes.Red) { }
				

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
	}
}
