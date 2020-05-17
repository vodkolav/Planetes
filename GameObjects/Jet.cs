using PolygonCollision;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GameObjects
{
	public class Jet : MarshalByRefObject
	{
		public Vector Pos { get => Hull.Center; }
	
		private Vector _speed;

		public Vector Speed
		{
			get { return _speed; }
			private set
			{
				if (value.Magnitude_X <= 15 && value.Magnitude_Y <=15)
				{					
					_speed = value;
				}
			}
		}

		public Vector Acceleration { get; set; }

		public float Thrust { get; set; }	
		
		public Vector Bearing { get; set; } = new Vector(1, 0);

		public Vector Aim { get; set; }

		private SolidBrush brush;

		public Color Color
		{
			get { return brush.Color; }
			set { brush = new SolidBrush(value); }
		}

		public Polygon Hull{ get; set; }

		public Polygon Cockpit { get; set;	}

		public Vector Gun { get { return Cockpit.Vertices[1]; } }
	
		public int LastFired { get; set; }

		public int Cooldown { get; set; }

		public Jet(Point start, Color color)
		{			
			Hull = new Polygon();
			Hull.AddVertex(new Vector(50, 50));
			Hull.AddVertex(new Vector(80, 50));
			Hull.AddVertex(new Vector(80, 70));
			Hull.AddVertex(new Vector(50, 70));

			Cockpit = new Polygon();
			Cockpit.AddVertex(new Vector(80, 50));
			Cockpit.AddVertex(new Vector(100, 60));
			Cockpit.AddVertex(new Vector(80, 70));

			Offset(new Vector(start));

			//Pos_x = start.X;
			//Pos_y = start.Y;
			Color = color;
			Thrust = 0.5f;
			Cooldown = 3;
		}
		
		private void Offset(Vector by)
		{
			Hull.Offset(by);
			Cockpit.Offset(by);
		}

		private void Rotate(Vector dir)
		{
			float diff = Bearing.Angle(dir);
			Bearing = dir;
			Hull.RotateAt(diff, Hull.Center);
			Cockpit.RotateAt(diff, Hull.Center);
		}

		public float Dist(Astroid a)
		{
			return (Pos - a.Pos).Magnitude;
		}

		public float Dist(Bullet b)
		{
			Vector diff = b.Pos - Pos;
			return diff.Magnitude;
		}

		// In some cases Asteroids don't collide when they should 
		// that's because I use polygon/point collision while asteroids are circles 
		public bool Collides(Astroid a)
		{
			return Hull.Collides(a.Body) || Cockpit.Collides(a.Body);
		}

		public bool Collides(Bullet b)
		{
			return Hull.Collides(b.Pos) || Cockpit.Collides(b.Pos);
		}
		
		public void Move(ClsGameObjects gO)
		{
			Speed += Acceleration * Thrust;
			
			PolygonCollisionResult r;
			foreach(Wall w in gO.Walls)
			{
				r = Hull.Collides(w.region, Speed);
				if (r.WillIntersect)
				{
					Offset(Speed + r.MinimumTranslationVector);
					Bounce(r.translationAxis);
				}
			}
			Offset(Speed);

			//Rotate
			Vector dir = Aim -  Hull.Center;
			
			Rotate(dir);

		}
		
		public void Bounce(Vector normal)
		{
			Speed = Speed - 2 * Speed.Dot(normal) * normal;			
		}
		
		public void Shoot(Player player, int timeElapsed)
		{
			if (player.Ammo != 0 && timeElapsed > LastFired + Cooldown)
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

			Hull.Draw(g, brush);
			Cockpit.Draw(g,Brushes.Gray);
			//	g.ResetTransform();

			//}
		}

		public override string ToString()
		{
			return "Speed: " + Speed.ToString() + " |Acc:" + Acceleration.ToString();
		}
	}	
}
