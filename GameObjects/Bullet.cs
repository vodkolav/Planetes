using PolygonCollision;
using System;
using System.Drawing;

namespace GameObjects
{
	public class Bullet : MarshalByRefObject
	{
		public Vector Pos { get; set; }
		public Vector Speed { get; set; }
		public int Size { get; set; }
		public bool HasHit { get; set; }
		private Pen Pen;
		public Player Shooter { get; private set; }

		public Bullet(Player player, float linearSpeed = 20)
		{
			Shooter = player;
			Pos = player.Jet.Gun;
			Speed = player.Jet.Bearing * (linearSpeed / player.Jet.Bearing.Magnitude);
			Size = 5;
			Pen = new Pen(player.Jet.Color, Size);
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
				g.DrawLine(Pen, (Pos - Speed).AsPoint, Pos.AsPoint);
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

			//check for collision with opponent's Jet
			if (Shooter.Enemy.Jet.Collides(this))

			//Pos.X < gameObjects.Player2.Jet.Pos.X &&
			//	Pos.X > gameObjects.Player2.Jet.Pos.X - gameObjects.Player2.Jet.Width &&
			//	Pos.Y > gameObjects.Player2.Jet.Pos.Y &&
			//	Pos.Y < gameObjects.Player2.Jet.Pos.Y + gameObjects.Player2.Jet.Height)
			{
				HasHit = true;
				//if (gameObjects.Player2.Health > 0)
				Shooter.Enemy.Hit(1);
				//else
				//	GameOver.Show(gameObjects.Player1);

			}

			foreach (Astroid ast in gameObjects.AstroidList)
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
