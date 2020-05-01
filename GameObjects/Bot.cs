using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GameObjects
{
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
}
