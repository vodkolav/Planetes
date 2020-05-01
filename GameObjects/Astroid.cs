using System;
using System.Drawing;

namespace GameObjects
{
	public enum AstType { Rubble, Ammo, Health };

	public class Astroid : MarshalByRefObject
	{
		public int Size { get; set; }
		public int Speed { get; set; }
		public int Angle { get; set; }
		public int Pos_x { get; set; }
		public int Pos_y { get; set; }
		public AstType Type { get; set; }
		public bool HasHit { get; set; }
		static public int Timeout { get { return 50; } }

		public void TossType(Random random)
		{
			switch (random.Next(5))
			{
				case (1):
					Type = AstType.Ammo;
					break;
				case 2:
					Type = AstType.Health;
					break;
				default:
					Type = AstType.Rubble;
					break;
			}
		}

		public Brush Color
		{
			get
			{
				switch (Type)
				{
					case AstType.Ammo:
						return Brushes.Yellow;
					case AstType.Health:
						return Brushes.Blue;
					default:
						return Brushes.Brown;
				}
			}
		}

		public Astroid(int winSize_x, int winSize_y)
		{
			Random random = new Random();
			Size = random.Next(20) + 5;
			Speed = random.Next(1, 5) + 1;
			Pos_x = random.Next(winSize_x);
			Pos_y = random.Next(winSize_y);
			Angle = random.Next(360);
			TossType(random);
			HasHit = false;
		}

		public void SayHello(Graphics g)
		{
			Console.WriteLine("hello");
		}

		public void Draw(Graphics g)
		{
			if (!HasHit)
				g.FillEllipse(Color, Pos_x, Pos_y, Size, Size);
		}

		public void Update(ClsGameObjects gameObjects)
		{
			if (!HasHit)
			{
				if (Pos_x + Size > gameObjects.Player1.Jet.Pos_x
					&& Pos_x - Size < gameObjects.Player1.Jet.Pos_x + gameObjects.Player1.Jet.Width
					&& Pos_y - Size < gameObjects.Player1.Jet.Pos_y + gameObjects.Player1.Jet.Height
					&& Pos_y + Size > gameObjects.Player1.Jet.Pos_y)
				{
					HasHit = true;
					if (Type == AstType.Ammo)
					{
						gameObjects.Player1.Recharge(Size);
					}
					else if (Type == AstType.Health)
					{
						gameObjects.Player1.Heal(1);
					}
					else if (gameObjects.Player1.Health > 0)
						gameObjects.Player1.Health--;
					else
						GameOver.Show(gameObjects.Player2);
				}
				if (Pos_x + Size > gameObjects.Player2.Jet.Pos_x - gameObjects.Player2.Jet.Width - gameObjects.Player2.Jet.Cockpit_size.Width
					&& Pos_x - Size < gameObjects.Player2.Jet.Pos_x + gameObjects.Player2.Jet.Width
					&& Pos_y - Size < gameObjects.Player2.Jet.Pos_y + gameObjects.Player2.Jet.Height
					&& Pos_y + Size > gameObjects.Player2.Jet.Pos_y)
				{
					HasHit = true;
					if (Type == AstType.Ammo)
					{
						gameObjects.Player2.Recharge(Size);
					}
					else if (Type == AstType.Health)
					{
						gameObjects.Player2.Heal(1);
					}
					else if (gameObjects.Player2.Health > 0)
						gameObjects.Player2.Health--;
					else
						GameOver.Show(gameObjects.Player1);
				}
				if (Pos_x + Size > gameObjects.WinSize_x || Pos_x < 0 || Pos_y > gameObjects.WinSize_y || Pos_y < 0)
				{
					HasHit = true;
				}
			}

			Pos_x += (int)(Math.Cos(2 * Math.PI / 360 * Angle) * Speed);
			Pos_y += (int)(Math.Sin(2 * Math.PI / 360 * Angle) * Speed);
		}


	}
}
