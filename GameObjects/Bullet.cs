using System;
using System.Drawing;

namespace GameObjects
{
	public class Bullet : MarshalByRefObject
	{
		public int Pos_x { get; set; }
		public int Pos_y { get; set; }
		public int Speed { get; set; }
		public int Size { get; set; }
		public bool HasHit { get; set; }
		public Pen Pen { get; protected set; }

		public Bullet(int start_x, int start_y)
		{
			Pos_x = start_x;
			Pos_y = start_y;
			Speed = 10;
			Size = 5;
			HasHit = false;
		}

		public bool Hit(Astroid a)
		{
			return Pos_x < a.Pos_x &&
					Pos_x > a.Pos_x - a.Size * 2 &&
					Pos_y > a.Pos_y &&
					Pos_y < a.Pos_y + a.Size * 2;
		}

		public void Draw(Graphics g)
		{
			if (!HasHit)
			{
				g.DrawLine(Pen, Pos_x, Pos_y, Pos_x + Size, Pos_y);
			}
		}

		public virtual void Move(ClsGameObjects gameObjects)
		{

		}
	}
	public class Bullet1 : Bullet
	{
		public Bullet1(int start_x, int start_y) : base(start_x, start_y)
		{
			Pen = new Pen(Color.Yellow, 2);
		}

		public override void Move(ClsGameObjects gameObjects)
		{
			if (!HasHit)
			{
				//check for collision with wall
				if (Pos_x > gameObjects.WinSize_x)
				{
					HasHit = true;
					return;
				}
				//check for collision with opponent's Jet
				if (Pos_x < gameObjects.Player2.Jet.Pos_x &&
					Pos_x > gameObjects.Player2.Jet.Pos_x - gameObjects.Player2.Jet.Width &&
					Pos_y > gameObjects.Player2.Jet.Pos_y &&
					Pos_y < gameObjects.Player2.Jet.Pos_y + gameObjects.Player2.Jet.Height)
				{
					HasHit = true;
					if (gameObjects.Player2.Health > 0)
						gameObjects.Player2.Health--;
					else
						GameOver.Show(gameObjects.Player1);

				}

				foreach (Astroid ast in gameObjects.AstroidList)
				{
					if (Hit(ast))
					{
						HasHit = true;
						ast.HasHit = true;
					}
				}
			}
			Pos_x += Speed;

		}
	}

	public class Bullet2 : Bullet
	{
		public Bullet2(int start_x, int start_y) : base(start_x, start_y)
		{
			Pen = new Pen(Color.Cyan, 2);
		}
		public override void Move(ClsGameObjects gameObjects)
		{
			if (!HasHit)
			{
				//check for collision with wall
				if (Pos_x < 0)
				{
					HasHit = true;
					return;
				}

				//check for collision with opponent's Jet
				if (Pos_x > gameObjects.Player1.Jet.Pos_x &&
					Pos_x < gameObjects.Player1.Jet.Pos_x + gameObjects.Player1.Jet.Width &&
					Pos_y > gameObjects.Player1.Jet.Pos_y &&
					Pos_y < gameObjects.Player1.Jet.Pos_y + gameObjects.Player2.Jet.Height)
				{
					HasHit = true;
					if (gameObjects.Player1.Health > 0)
						gameObjects.Player1.Health--;
					else
						GameOver.Show(gameObjects.Player2);
				}
				foreach (Astroid ast in gameObjects.AstroidList)
				{
					if (Hit(ast))
					{
						HasHit = true;
						ast.HasHit = true;
					}
				}
			}
			Pos_x -= Speed;

		}
	}
}
