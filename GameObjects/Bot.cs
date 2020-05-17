using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GameObjects
{
	/// <summary>
	/// Most basic bot
	/// </summary>
	public class Bot : Player
	{
		protected List<HOTAS> directions = new List<HOTAS> { HOTAS.Up, HOTAS.Down };

		public int ReactionSlowDown { get; private set; } = 10;

		public Bot(string name, int health, int ammo,Point At, Color color, ClsGameObjects game)
			: base(name, health, ammo, At,color, game)
		{
			Name = GetType().FullName + " " + color.ToString();
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
		public Bot(string name, int health, int ammo,Point At, Color color, ClsGameObjects game, System.Windows.Forms.Timer timer)
			: base(name, health, ammo,  At,color, game)
		{
			

			//Jet = new Jet2(winSize_x, winSize_y / 2);
			GameState = game;
			timer.Tick += new System.EventHandler(Play);
		}


		protected virtual HOTAS pickOther(HOTAS k)
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
			int count = 0;
			HOTAS direction = directions[0];
			while (true)
			{
				Thread.Sleep(ClsGameObjects.FrameInterval* ReactionSlowDown);

				if (count == 5)
				{
					count = 0;
					direction = pickOther(direction);
					Steer(direction);
					Steer(HOTAS.Shoot);
					
				}
				count++;
			}
		}
	}
	public class Bot4 : Bot
	{
		int messagenum = 0;
		public Bot4(string name, int health, int ammo, Point At, Color color, ClsGameObjects game)
			: base(name, health, ammo, At,color,  game)
		{ }

		public Bot4(string name, int health, int ammo, Point At, Color color,ClsGameObjects game, System.Windows.Forms.Timer timer)
			: base(name, health, ammo, At,color,   game, timer)
		{ }

		protected override void Play()
		{
			//int timeElapsed = 0;


			//todo:
			// make bot catch ammo and health crates
			// smarter maneuvring between asteroids and bullets


			while (true)
			{

				//double astavg = gamenow.AstroidList.Where(a => a.Pos.Y + 50 < Jet.Pos.Y && a.Pos.Y - 50 > Jet.Pos.Y).Select(c => c.Pos.X).Average();
				//int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

				try
				{
					//asteroid evasion tactic
					Astroid astClosest = GameState.AstroidList.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));

					if (astClosest.Pos.X - astClosest.Size * 10 < Jet.Pos.X && Jet.Pos.X < astClosest.Pos.X && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Left);
						Steer(HOTAS.Left);
					}
					else if (astClosest.Pos.X < Jet.Pos.X && Jet.Pos.X < astClosest.Pos.X + astClosest.Size * 10 && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Right);
						Steer(HOTAS.Right);
					}
					else
					{
						Release(HOTAS.Right);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(messagenum++ + e.Message);
				}


				try
				{
					//bullet evasion tactic (not good yet) Where(b=> b.Pos.X + 50 > Jet.Pos.X).

					Bullet bulClosest = Enemy.Bulletlist.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));
					//Jet.Pos.Y-50 > bulClosest.Pos.Y  &&
					if (Jet.Pos.Y < bulClosest.Pos.Y && bulClosest.Pos.Y < Jet.Pos.Y + 50)
					{
						Steer(HOTAS.Down);
						//Jet.Move(Keys.Up);
						//Jet.Move(Keys.Up);
					}//Jet.Pos.Y + 50 < bulClosest.Pos.Y  &&
					else if (Jet.Pos.Y - 50 < bulClosest.Pos.Y && bulClosest.Pos.Y <= Jet.Pos.Y)
					{
						Steer(HOTAS.Up);
						//Jet.Move(Keys.Down);
						//Jet.Move(Keys.Down);
					}
					else
					{
						Release(HOTAS.Up);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(messagenum++ + e.Message);
				}


				//aiming at opponent tactic

				Aim(Enemy.Jet.Pos);
				if (Jet.Pos.Y < Enemy.Jet.Pos.Y - 50)
				{
					//Jet.Move(Keys.Down);
					
					Steer(HOTAS.Down);
				}
				else if (Jet.Pos.Y > Enemy.Jet.Pos.Y + 50)
				{
					Steer(HOTAS.Up);
				}
				else
				{
					Release(HOTAS.Up);
				}

				//shoot at opponent tactic
				if ((Jet.Pos - Enemy.Jet.Pos).Magnitude < 300)
				{
					//BotShoot(timeElapsed);
					Steer(HOTAS.Shoot);
				}
				else
				{
					Release(HOTAS.Shoot);
				}

				//timeElapsed += ClsGameObjects.FrameRate;
				Thread.Sleep(ClsGameObjects.FrameInterval * ReactionSlowDown);
			}
		}
	}


	public class Bot3 : Bot
	{

		public Bot3(string name, int health, int ammo, Point At, Color color, ClsGameObjects game)
			: base(name, health, ammo, At,  color, game)
		{ }

		public Bot3(string name, int health, int ammo, Point At, Color color, ClsGameObjects game, System.Windows.Forms.Timer timer)
			: base(name, health, ammo,  At, color, game, timer)
		{ }

		

		protected override void Play()
		{
			//int timeElapsed = 0;


			//todo:
			// make bot catch ammo and health crates
			// smarter maneuvring between asteroids and bullets


			while (true)
			{

				//double astavg = gamenow.AstroidList.Where(a => a.Pos.Y + 50 < Jet.Pos.Y && a.Pos.Y - 50 > Jet.Pos.Y).Select(c => c.Pos.X).Average();
				//int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

				try
				{
					//asteroid evasion tactic
					Astroid astClosest = GameState.AstroidList.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));

					if (astClosest.Pos.X - astClosest.Size * 10 < Jet.Pos.X && Jet.Pos.X < astClosest.Pos.X && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Left);
						Steer(HOTAS.Left);
					}
					else if (astClosest.Pos.X < Jet.Pos.X && Jet.Pos.X < astClosest.Pos.X + astClosest.Size * 10 && astClosest.Type == AstType.Rubble)
					{
						//Jet.Move(Keys.Right);
						Steer(HOTAS.Right);
					}
					else
					{
						Release(HOTAS.Right);
					}
				}
				catch
				{

				}


				try
				{
					//bullet evasion tactic (not good yet)

					Bullet bulClosest = GameState.player1.Bulletlist.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));
					//Jet.Pos.Y-50 > bulClosest.Pos.Y  &&
					if (bulClosest.Pos.Y > Jet.Pos.Y && bulClosest.Pos.X + 50 > Jet.Pos.X)
					{
						Steer(HOTAS.Up);
						//Jet.Move(Keys.Up);
						//Jet.Move(Keys.Up);
					}//Jet.Pos.Y + 50 < bulClosest.Pos.Y  &&
					else if (bulClosest.Pos.Y < Jet.Pos.Y && bulClosest.Pos.X + 50 > Jet.Pos.X)
					{
						Steer(HOTAS.Down);
						//Jet.Move(Keys.Down);
						//Jet.Move(Keys.Down);
					}
					else
					{
						Release(HOTAS.Up);
					}
				}
				catch
				{
					//int a = 1000;
				}


				//aiming at opponent tactic
				if (Jet.Pos.Y - GameState.player1.Jet.Pos.Y < -20)
				{
					//Jet.Move(Keys.Down);
					Steer(HOTAS.Down);
				}
				else if (Jet.Pos.Y - GameState.player1.Jet.Pos.Y > 20)
				{
					Steer(HOTAS.Up);
				}
				else
				{
					Release(HOTAS.Up);

				}

				//shoot at opponent tactic
				if (Math.Abs(Jet.Pos.Y - GameState.player1.Jet.Pos.Y) < 20)
				{
					//BotShoot(timeElapsed);
					Steer(HOTAS.Shoot);
				}
				else
				{
					Release(HOTAS.Shoot);
				}
				//timeElapsed += ClsGameObjects.FrameRate;
				Thread.Sleep(ClsGameObjects.FrameInterval * ReactionSlowDown);
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
	//			//double astavg = gamenow.AstroidList.Where(a => a.Pos.Y + 50 < Jet.Pos.Y && a.Pos.Y - 50 > Jet.Pos.Y).Select(c => c.Pos.X).Average();
	//			//int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

	//			try
	//			{
	//				//asteroid evasion tactic
	//				Astroid astClosest = GameState.AstroidList.Aggregate((curMin, x) => (curMin == null || (B.Jet.Dist(x)) < B.Jet.Dist(curMin) ? x : curMin));

	//				if (astClosest.Pos.X - astClosest.Size * 10 < B.Jet.Pos.X && B.Jet.Pos.X < astClosest.Pos.X && astClosest.Type == AstType.Rubble)
	//				{
	//					//Jet.Move(Keys.Left);
	//					B.BotSteer(Keys.Left);
	//				}
	//				else if (astClosest.Pos.X < B.Jet.Pos.X && B.Jet.Pos.X < astClosest.Pos.X + astClosest.Size * 10 && astClosest.Type == AstType.Rubble)
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
	//				//Jet.Pos.Y-50 > bulClosest.Pos.Y  &&
	//				if (bulClosest.Pos.Y > B.Jet.Pos.Y && bulClosest.Pos.X + 50 > B.Jet.Pos.X)
	//				{
	//					B.BotSteer(Keys.Up);
	//					B.mo
	//					//Jet.Move(Keys.Up);
	//					//Jet.Move(Keys.Up);
	//				}//Jet.Pos.Y + 50 < bulClosest.Pos.Y  &&
	//				else if (bulClosest.Pos.Y < Jet.Pos.Y && bulClosest.Pos.X + 50 > Jet.Pos.X)
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
	//			if (Jet.Pos.Y - GameState.Player1.Jet.Pos.Y < -20)
	//			{
	//				//Jet.Move(Keys.Down);
	//				BotSteer(Keys.Down);
	//			}
	//			else if (Jet.Pos.Y - GameState.Player1.Jet.Pos.Y > 20)
	//			{
	//				BotSteer(Keys.Up);
	//			}
	//			else
	//			{
	//				BotRelease(Keys.Up);

	//			}

	//			if (Math.Abs(Jet.Pos.Y - GameState.Player1.Jet.Pos.Y) < 20)
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

		public Bot2(string name, int health, int ammo, Point At, Color color, ref ClsGameObjects game)
			: base(name, health, ammo, At, color, game)
		{ }


		protected override void Play()
		{
			int timeElapsed = 0;

			HOTAS direction = directions[0];
			while (true)
			{

				//Console.WriteLine(gamenow.GetHashCode());
				if (Jet.Pos.Y < GameState.player1.Jet.Pos.Y)
				{
					Jet.Move(GameState);// (Keys.Down);									
				}
				else
				{
					Jet.Move(GameState);// Keys.Up);					
				}

				if (Jet.Pos.Y - GameState.player1.Jet.Pos.Y < 50)
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
		public Bot1(string name, int health, int ammo, Point At, Color color, ClsGameObjects game)
			: base(name, health, ammo,  At, color, game)
		{ }


		protected override void Play()
		{
			int timeElapsed = 0;
			int count = 0;
			HOTAS direction = directions[0];
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
}
