using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;

namespace GameObjects
{
	public enum AstType { Rubble, Ammo, Health };

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

			Walls.Add(new Wall(wallBrush, new Point(100,100), new Point (200,200)));


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

	public struct Vector
	{
		public double x { get; }

		public double y { get; }

		public Vector(Point point)
		{
			x = point.X;
			y = point.Y;
		}

		public Vector(double X, double Y)
		{
			x = X;
			y = Y;
		}

		public Vector(Vector2 v)
		{
			x = v.X ;
			y = v.Y;
		}

		public Vector2 asVector2()
		{
			return new Vector2((float)x, (float)y);
		}

		public Point asPoint
		{
			get { return new Point((int)x, (int)y); }
		}

		public static Vector operator -(Vector v1, Vector v2)
		{
			return new Vector(
			   v1.x - v2.x,
			   v1.y - v2.y);
		}

		public static Vector operator +(Vector v1, Vector v2)
		{
			return new Vector(
			   v1.x + v2.x,
			   v1.y + v2.y);
		}

		public static Vector operator *(Vector v1, double s2)
		{
			return
			   new Vector
			   (
				  v1.x * s2,
				  v1.y * s2
			   );
		}

		public static Vector operator *(double s1, Vector v2)
		{
			return v2 * s1;
		}

		public double Magnitude
		{
			get
			{
				return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
			}
		}

		public Vector Normalize()
		{
			Vector2 v2 = asVector2();
			return new Vector( Vector2.Normalize(v2));
		}

		public static double angle(Vector v1, Vector v2)
		{			
			double dot = Dot(v1, v2);      // dot product between [x1, y1] and [x2, y2]
			double det = v1.x * v2.y - v1.y * v2.x;      // determinant
			double angle = - Math.Atan2(det, dot) * 57.2958;
			//Console.WriteLine("X:{0}| Y:{1} | Angle:{2})", v2.x, v2.y, angle);
			return angle;
		}


		internal double Angle
		{
			get
			{
				return (double)angle(this, new Vector(1, 0));
			}
		}

		public static double Dot(Vector v1, Vector v2)
		{
			return v1.x * v2.x + v1.y * v2.y;
		}

		public double Dot(Vector other)
		{
			return Dot(this, other);
		}


	}

	public static class RectExtension
	{
		public static Point Center(this Rectangle rect)
		{
			return new Point(rect.Location.X + rect.Size.Width/2 ,  rect.Location.Y + rect.Size.Height/2);
		}

		public static Point Center(this RectangleF rect)
		{
			return new Point((int)(rect.Location.X + rect.Size.Width / 2), (int)(rect.Location.Y + rect.Size.Height / 2));
		}
	}


	public abstract class Player : MarshalByRefObject
	{
		public string Name { get; set; }
		public bool Host { get; set; }
		public int Health { get; set; }
		public int MaxHealth { get; set; }
		public int Ammo { get; set; }
		public int MaxAmmo { get; set; }
		public bool Fired { get; set; }
		public int WinSize_x { get; set; }
		public int WinSize_y { get; set; }
		public Jet Jet { get; set; }
		public List<Bullet> Bulletlist { get; set; }
		public Keys? KeyVert { get; set; }
		public Keys? KeyHorz { get; set; }
		public Keys? KeyShoot { get; set; }
		public ClsGameObjects GameState { get; set; }
		protected List<Keys> SteerKeysBindings;
		protected Keys ShootKeyBindings;

		public Player(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game)
		{
			Name = name;
			Health = health;
			MaxHealth = health;
			Ammo = ammo;
			MaxAmmo = ammo;
			WinSize_x = winSize_x;
			WinSize_y = winSize_y;
			Bulletlist = new List<Bullet>();
			Fired = false;
			GameState = game;
		}

		public void Recharge(int amount)
		{
			Ammo = Math.Min(Ammo + amount, MaxAmmo);
		}

		public void Heal(int amount)
		{
			Health = Math.Min(Health + amount, MaxHealth);
		}

		public virtual void Steer(Keys command)
		{
			if (SteerKeysBindings.Contains(command))
			{
				Jet.Steer(command);
			}
			else if (command == ShootKeyBindings)
			{
				KeyShoot = command;
			}
		}

		public abstract void Release(Keys command);

		public void Move()
		{
			Jet.Move(GameState);
		}

		public void Turn(Point location)
		{
			Jet.Turn(location);
		}

		public virtual void Shoot(int timeElapsed)
		{
			if (KeyShoot != null)
				Jet.Shoot(this, timeElapsed);
		}
	}

	public class Player1 : Player
	{
		public Player1(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{
			Jet = new Jet1(new Point(10, winSize_y / 2), winSize_x, winSize_y);
			SteerKeysBindings = new List<Keys>{ Keys.A, Keys.D, Keys.W, Keys.S };
			ShootKeyBindings = Keys.Space;
		}


		public override void Release(Keys command)
		{
			if (command == Keys.A || command == Keys.D)
			{
				//then it's a horizontal move
				KeyHorz = null;
			}
			else if (command == Keys.W || command == Keys.S)
			{
				//then it's a horizontal move
				KeyVert = null;
			}
			else if (command == Keys.Space)
			{
				KeyShoot = null;
			}
		}
	}

	public class Player2 : Player
	{
		public Player2(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{
			Jet = new Jet2(new Point(winSize_x - 60, winSize_y / 2), winSize_x, winSize_y);
			SteerKeysBindings = new List<Keys> { Keys.Up, Keys.Down, Keys.Left, Keys.Right };
			ShootKeyBindings = Keys.Enter;
		}


		public override void Release(Keys command)
		{
			if (command == Keys.Left || command == Keys.Right)
			{
				KeyHorz = null;
			}
			else if (command == Keys.Up || command == Keys.Down)
			{
				KeyVert = null;
			}
			else if (command == Keys.Return)
			{
				KeyShoot = null;
			}
		}
	}

	#region Bots
	/// <summary>
	/// Most basic bot
	/// </summary>
	public class Bot : Player2
	{
		protected List<Keys> directions = new List<Keys> { Keys.Up, Keys.Down };


		public Bot(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{

			//Jet = new Jet2(winSize_x, winSize_y / 2);

			Thread t = new Thread(Play);
			t.Name = "BotThread";
			t.IsBackground = true;
			t.Start();
		}


		/// <summary>
		/// attempt to link bot movement to primary game timer. does not work well
		/// </summary>
		/// <param name="name"></param>
		/// <param name="health"></param>
		/// <param name="ammo"></param>
		/// <param name="winSize_x"></param>
		/// <param name="winSize_y"></param>
		/// <param name="game"></param>
		/// <param name="timer"></param>
		public Bot(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game, System.Windows.Forms.Timer timer)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{

			//Jet = new Jet2(winSize_x, winSize_y / 2);
			GameState = game;
			timer.Tick += new System.EventHandler(Play);
		}


		public sealed override void Steer(Keys command)
		{
			//Must remain empty to prevent user from controling bot
		}

		public sealed override void Release(Keys command)
		{
			//Must remain empty to prevent user from controling bot
		}

		//public sealed override void Shoot(int timeElapsed)
		//{
		//	//Must remain empty to prevent user from controling bot
		//}


		public void BotSteer(Keys command)
		{
			base.Steer(command);
		}

		public void BotRelease(Keys command)
		{
			base.Release(command);
		}

		public void BotShoot(int timeElapsed)
		{
			base.Shoot(timeElapsed);
		}

		protected virtual Keys pickOther(Keys k)
		{
			return directions.Where(c => c.CompareTo(k) != 0).Single();
		}


		/// <summary>
		/// attempt to link bot movement to primary game timer. does not work well
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void Play(object sender, EventArgs e)
		{
			Play();
		}

		/// <summary>
		/// basic logic for simplest bot. just move one up, shoot, one down, shoot, etc
		/// </summary>
		protected virtual void Play()
		{
			int timeElapsed = 0;
			int count = 0;
			Keys direction = directions[0];
			while (true)
			{
				Thread.Sleep(50);
				if (count < 5)
				{
					BotSteer(direction);
					BotShoot(timeElapsed);
					count++;
				}
				else
				{
					count = 0;
					direction = pickOther(direction);
				}
				timeElapsed++;
			}
		}
	}

	public class Bot4 : Bot
	{
		bool chasing = false;
		int messagenum = 0;
		public Bot4(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{ }

		public Bot4(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game, System.Windows.Forms.Timer timer)
			: base(name, health, ammo, winSize_x, winSize_y, game, timer)
		{ }

		protected override void Play()
		{
			int timeElapsed = 0;


			//todo:
			// make bot catch ammo and health crates
			// smarter maneuvring between asteroids and bullets


			while (true)
			{

				//double astavg = gamenow.AstroidList.Where(a => a.Pos_y + 50 < Jet.Pos_y && a.Pos_y - 50 > Jet.Pos_y).Select(c => c.Pos_x).Average();
				//int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

				try
				{
					//asteroid evasion tactic
					Astroid astClosest = GameState.AstroidList.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));

					if (astClosest.Pos_x - astClosest.Size * 10 < Jet.Pos_x && Jet.Pos_x < astClosest.Pos_x && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Left);
						BotSteer(Keys.Left);
					}
					else if (astClosest.Pos_x < Jet.Pos_x && Jet.Pos_x < astClosest.Pos_x + astClosest.Size * 10 && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Right);
						BotSteer(Keys.Right);
					}
					else
					{
						BotRelease(Keys.Right);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(messagenum++ + e.Message);
				}


				try
				{
					//bullet evasion tactic (not good yet) Where(b=> b.Pos_x + 50 > Jet.Pos_x).

					Bullet bulClosest = GameState.Player1.Bulletlist.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));
					//Jet.Pos_y-50 > bulClosest.Pos_y  &&
					if (Jet.Pos_y < bulClosest.Pos_y && bulClosest.Pos_y < Jet.Pos_y + 50)
					{
						BotSteer(Keys.Down);
						//Jet.Move(Keys.Up);
						//Jet.Move(Keys.Up);
					}//Jet.Pos_y + 50 < bulClosest.Pos_y  &&
					else if (Jet.Pos_y - 50 < bulClosest.Pos_y && bulClosest.Pos_y <= Jet.Pos_y)
					{
						BotSteer(Keys.Up);
						//Jet.Move(Keys.Down);
						//Jet.Move(Keys.Down);
					}
					else
					{
						BotRelease(Keys.Up);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(messagenum++ + e.Message);
				}


				//aiming at opponent tactic
				if (Jet.Pos_y < GameState.Player1.Jet.Pos_y - 30)
				{
					//Jet.Move(Keys.Down);
					BotSteer(Keys.Down);
				}
				else if (Jet.Pos_y > GameState.Player1.Jet.Pos_y + 30)
				{
					BotSteer(Keys.Up);
				}
				else
				{
					BotRelease(Keys.Up);
				}

				//shoot at opponent tactic
				if (Math.Abs(Jet.Pos_y - GameState.Player1.Jet.Pos_y) < 30)
				{
					//BotShoot(timeElapsed);
					BotSteer(Keys.Return);
				}
				else
				{
					BotRelease(Keys.Return);
				}



				int step = 5;
				timeElapsed += ClsGameObjects.FrameRate;
				Thread.Sleep(ClsGameObjects.FrameRate * 10);
			}
		}
	}


	public class Bot3 : Bot
	{
		bool chasing = false;

		public Bot3(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{ }

		public Bot3(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game, System.Windows.Forms.Timer timer)
			: base(name, health, ammo, winSize_x, winSize_y, game, timer)
		{ }

		protected override void Play()
		{
			int timeElapsed = 0;


			//todo:
			// make bot catch ammo and health crates
			// smarter maneuvring between asteroids and bullets


			while (true)
			{

				//double astavg = gamenow.AstroidList.Where(a => a.Pos_y + 50 < Jet.Pos_y && a.Pos_y - 50 > Jet.Pos_y).Select(c => c.Pos_x).Average();
				//int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

				try
				{
					//asteroid evasion tactic
					Astroid astClosest = GameState.AstroidList.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));

					if (astClosest.Pos_x - astClosest.Size * 10 < Jet.Pos_x && Jet.Pos_x < astClosest.Pos_x && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Left);
						BotSteer(Keys.Left);
					}
					else if (astClosest.Pos_x < Jet.Pos_x && Jet.Pos_x < astClosest.Pos_x + astClosest.Size * 10 && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Right);
						BotSteer(Keys.Right);
					}
					else
					{
						BotRelease(Keys.Right);
					}
				}
				catch
				{

				}


				try
				{
					//bullet evasion tactic (not good yet)

					Bullet bulClosest = GameState.Player1.Bulletlist.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));
					//Jet.Pos_y-50 > bulClosest.Pos_y  &&
					if (bulClosest.Pos_y > Jet.Pos_y && bulClosest.Pos_x + 50 > Jet.Pos_x)
					{
						BotSteer(Keys.Up);
						//Jet.Move(Keys.Up);
						//Jet.Move(Keys.Up);
					}//Jet.Pos_y + 50 < bulClosest.Pos_y  &&
					else if (bulClosest.Pos_y < Jet.Pos_y && bulClosest.Pos_x + 50 > Jet.Pos_x)
					{
						BotSteer(Keys.Down);
						//Jet.Move(Keys.Down);
						//Jet.Move(Keys.Down);
					}
					else
					{
						BotRelease(Keys.Up);
					}
				}
				catch
				{
					//int a = 1000;
				}


				//aiming at opponent tactic
				if (Jet.Pos_y - GameState.Player1.Jet.Pos_y < -20)
				{
					//Jet.Move(Keys.Down);
					BotSteer(Keys.Down);
				}
				else if (Jet.Pos_y - GameState.Player1.Jet.Pos_y > 20)
				{
					BotSteer(Keys.Up);
				}
				else
				{
					BotRelease(Keys.Up);

				}

				//shoot at opponent tactic
				if (Math.Abs(Jet.Pos_y - GameState.Player1.Jet.Pos_y) < 20)
				{
					//BotShoot(timeElapsed);
					BotSteer(Keys.Return);
				}
				else
				{
					BotRelease(Keys.Return);
				}
				int step = 5;
				timeElapsed += ClsGameObjects.FrameRate;
				Thread.Sleep(ClsGameObjects.FrameRate * 10);
			}
		}
	}

	//public class AI: MarshalByRefObject
	//{
	//	public Bot B { get; set; }
	//	public ClsGameObjects GameState { get; set; }

	//	public AI(Bot player, ClsGameObjects gamestate)
	//	{
	//		B = player;
	//		GameState = gamestate;
	//	}

	//	protected override void Play()
	//	{
	//		int timeElapsed = 0;


	//		//todo:
	//		// make bot catch ammo and health crates
	//		// smarter maneuvring between asteroids


	//		while (true)
	//		{
	//			//double astavg = gamenow.AstroidList.Where(a => a.Pos_y + 50 < Jet.Pos_y && a.Pos_y - 50 > Jet.Pos_y).Select(c => c.Pos_x).Average();
	//			//int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

	//			try
	//			{
	//				//asteroid evasion tactic
	//				Astroid astClosest = GameState.AstroidList.Aggregate((curMin, x) => (curMin == null || (B.Jet.Dist(x)) < B.Jet.Dist(curMin) ? x : curMin));

	//				if (astClosest.Pos_x - astClosest.Size * 10 < B.Jet.Pos_x && B.Jet.Pos_x < astClosest.Pos_x && astClosest.Type == AstType.Rubble)
	//				{
	//					//Jet.Move(Keys.Left);
	//					B.BotSteer(Keys.Left);
	//				}
	//				else if (astClosest.Pos_x < B.Jet.Pos_x && B.Jet.Pos_x < astClosest.Pos_x + astClosest.Size * 10 && astClosest.Type == AstType.Rubble)
	//				{
	//					//Jet.Move(Keys.Right);
	//					B.BotSteer(Keys.Right);
	//				}
	//				else
	//				{
	//					B.BotRelease(Keys.Right);
	//				}
	//			}
	//			catch
	//			{

	//			}


	//			try
	//			{
	//				//bullet evasion tactic (not good yet)

	//				Bullet bulClosest = GameState.Player1.Bulletlist.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));
	//				//Jet.Pos_y-50 > bulClosest.Pos_y  &&
	//				if (bulClosest.Pos_y > B.Jet.Pos_y && bulClosest.Pos_x + 50 > B.Jet.Pos_x)
	//				{
	//					B.BotSteer(Keys.Up);
	//					B.mo
	//					//Jet.Move(Keys.Up);
	//					//Jet.Move(Keys.Up);
	//				}//Jet.Pos_y + 50 < bulClosest.Pos_y  &&
	//				else if (bulClosest.Pos_y < Jet.Pos_y && bulClosest.Pos_x + 50 > Jet.Pos_x)
	//				{
	//					BotSteer(Keys.Down);
	//					//Jet.Move(Keys.Down);
	//					//Jet.Move(Keys.Down);
	//				}
	//				else
	//				{
	//					BotRelease(Keys.Up);
	//				}
	//			}
	//			catch
	//			{
	//				//int a = 1000;
	//			}


	//			//aiming at opponent tactic
	//			if (Jet.Pos_y - GameState.Player1.Jet.Pos_y < -20)
	//			{
	//				//Jet.Move(Keys.Down);
	//				BotSteer(Keys.Down);
	//			}
	//			else if (Jet.Pos_y - GameState.Player1.Jet.Pos_y > 20)
	//			{
	//				BotSteer(Keys.Up);
	//			}
	//			else
	//			{
	//				BotRelease(Keys.Up);

	//			}

	//			if (Math.Abs(Jet.Pos_y - GameState.Player1.Jet.Pos_y) < 20)
	//			{
	//				BotShoot(timeElapsed);
	//			}
	//			int step = 5;
	//			timeElapsed += ClsGameObjects.FrameRate;
	//			Thread.Sleep(ClsGameObjects.FrameRate * 10);
	//		}
	//	}

	//}

	public class Bot2 : Bot
	{

		public Bot2(string name, int health, int ammo, int winSize_x, int winSize_y, ref ClsGameObjects game)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{ }


		protected override void Play()
		{
			int timeElapsed = 0;

			Keys direction = directions[0];
			while (true)
			{

				//Console.WriteLine(gamenow.GetHashCode());
				if (Jet.Pos_y < GameState.Player1.Jet.Pos_y)
				{
					Jet.Move(GameState);// (Keys.Down);									
				}
				else
				{
					Jet.Move(GameState);// Keys.Up);					
				}

				if (Jet.Pos_y - GameState.Player1.Jet.Pos_y < 50)
				{
					Jet.Shoot(this, timeElapsed);
				}


				Thread.Sleep(100);
				timeElapsed++;
			}

		}
	}

	public class Bot1 : Bot
	{
		public Bot1(string name, int health, int ammo, int winSize_x, int winSize_y, ClsGameObjects game)
			: base(name, health, ammo, winSize_x, winSize_y, game)
		{ }


		protected override void Play()
		{
			int timeElapsed = 0;
			int count = 0;
			Keys direction = directions[0];
			while (true)
			{
				Thread.Sleep(50);
				if (count < 5)
				{
					Jet.Move(GameState);// direction);
					Jet.Shoot(this, timeElapsed);
					count++;
				}
				else
				{
					count = 0;
					direction = pickOther(direction);
				}
				timeElapsed++;
			}

		}
	}

	#endregion


	public abstract class Jet : MarshalByRefObject
	{
		public int LastFired { get; set; }
		public int FireRate { get; set; }
		public int Acceleration { get; set; }
		protected int WWidth { get; set; }
		protected int WHeight { get; set; }
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
			WWidth = wwidth;
			WHeight = wheight;
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
			Console.WriteLine("X:{0}| Y:{1} | Angle:{2})", diff.x, diff.y, Bearing);

		}

		public void Bounce(Wall w)
		{
			Vector velocity = new Vector(Speed_x, Speed_y);
			Vector normal = new Vector(Hull.Center()) - w.Reflect(Hull);
			normal = normal.Normalize();
			Vector reflection = velocity - 2 * velocity.Dot(normal) * normal;
			Speed_x = (int)reflection.x;
			Speed_y = (int)reflection.y;
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


	public class Wall : MarshalByRefObject
	{
		public Brush Color { get; set; }

		public Region region { get; set; }


		public Wall(Brush color, Point location, Size size)
		{
			Rectangle rectangle = new Rectangle(location, size);
			region = new Region(rectangle);
			Color = color;
		}

		public Wall(Brush color, Point from, Point to)
		{
			region = Calculate(from, to);

			Color = color;
		}

		private Region Calculate(Point from, Point to, int w = 5)
		{
			Vector A = new Vector(from);
			Vector B = new Vector(to);

			double alp = (B - A).Angle;

			Vector shift = w / 2 * new Vector(Math.Cos(Math.PI / 2 - alp), - Math.Sin(Math.PI / 2 - alp));

			GraphicsPath path = new GraphicsPath();
			path.StartFigure();
			path.AddLine((A + shift).asPoint, (B + shift).asPoint);
			path.AddLine((B + shift).asPoint, (B - shift).asPoint);
			path.AddLine((B - shift).asPoint, (A - shift).asPoint);
			path.AddLine((A - shift).asPoint, (A + shift).asPoint);
			path.CloseFigure();
			Region reg = new Region(path);
			return reg;

			//Point[] pts = new Point[] { (A + shift).asPoint, (B + shift).asPoint, (B - shift).asPoint, (A - shift).asPoint, };
			//byte[] types = new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.CloseSubpath };
			//return new Region(new GraphicsPath(pts, types));

			//g.FillRegion(Brushes.AliceBlue, r);


		}

		public void Draw(Graphics g)
		{
			//g.FillRectangle(Color, rectangle);
			g.FillRegion(Color, region);
			
		}

		internal bool IntersectsWith(Rectangle hull)
		{
			bool vis = region.IsVisible(hull);
			return vis; //  false;// 
		}

		public Vector Reflect(Rectangle r)
		{
			Region reflectionPlace = region.Clone();
			reflectionPlace.Intersect(r);

			RegionData rd = reflectionPlace.GetRegionData();

			Vector reflectionPoint = new Vector(-10,5);
			return reflectionPoint;
		}
	}

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
