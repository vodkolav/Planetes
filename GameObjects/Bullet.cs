using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Drawing;

namespace GameObjects
{
	public class Bullet 
	{
		public Vector Pos { get; set; }

		public static int linearSpeed = 30;

		public Vector Speed { get; set; }

		public int Size { get; set; }

        public Color Color { get; set; }

        public bool HasHit { get; set; }		

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
			 
			//Pos.X < a.Pos.X &&
			//	Pos.X > a.Pos.X - a.Size * 2 &&
			//	Pos.Y > a.Pos.Y &&
			//	Pos.Y < a.Pos.Y + a.Size * 2;
		}

		public void Draw(Graphics g)
		{
			if (!HasHit)
			{
				g.DrawLine(new Pen(Color, Size), (Pos - (Speed*0.5)).AsPoint, Pos.AsPoint);
			}
		}

		public void Move(ClsGameObjects gameObjects)
		{
			//check for collision with wall
			foreach (Wall w in gameObjects.Walls)
			{
				if (w.region.Collides(Pos))

				//Pos.X > gameObjects.WinSize_x)
				{
					HasHit = true;
					return;
				}
			}

			//check whether a bullet as way outside of screen - can remove it then
			if (Pos.Magnitude > new Vector(gameObjects.WinSize).Magnitude * 2)			
				
			//Pos.X < gameObjects.Player2.Jet.Pos.X &&			
			//	Pos.X > gameObjects.Player2.Jet.Pos.X - gameObjects.Player2.Jet.Width &&
			//	Pos.Y > gameObjects.Player2.Jet.Pos.Y &&
			//	Pos.Y < gameObjects.Player2.Jet.Pos.Y + gameObjects.Player2.Jet.Height)
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

		public void Offset(Vector destination)
		{
			Pos = Pos + destination;
		}
	}
}
