using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    public class Bullet 
	{
		public Vector Pos { get; set; }

		public static int linearSpeed = 20;

		public Vector Speed { get; set; }

		public int Size { get; set; }

        public Color Color { get; set; }

        public bool HasHit { get; set; }

		public int Power { get; set; } = 1;

        public Bullet(Vector pos, Vector speed, int size, Color color )
		{
			Pos = pos;
			Speed = speed;
			Size = size;
			Color = color;
			HasHit = false;
		}

		public bool Collides(Astroid a)
		{
			return (Pos - a.Pos).Magnitude < a.Size ;
		}

		public void Draw(Graphics g)
		{
			if (!HasHit)
			{
				g.DrawLine(new Pen(Color, Size), (Pos - (Speed*0.5)).AsPoint, Pos.AsPoint);
			}
		}

		public void Move(GameState gameObjects)
		{
			//check for collision with wall
			foreach (Wall w in gameObjects.Walls)
			{
				if (w.region.Collides(Pos))
				{
					HasHit = true;
					return;
				}
			}

			//check whether a bullet as way outside of screen - can remove it then
			if (Pos.Magnitude > new Vector(gameObjects.WinSize).Magnitude * 2)	
			{
				HasHit = true;
				return;
			}

			foreach (Astroid ast in gameObjects.Astroids)
			{
				if (Collides(ast))
				{
					HasHit = true;
					ast.HasHit = true;
				}
			}

			Offset(Speed);
		}

		public void Offset(Vector by)
		{
			Pos += by;
		}
	}
}
