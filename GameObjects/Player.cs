using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GameObjects
{
	public enum Action { Press, Release, Aim}

	public class Player
	{
		public string Name { get; set; } 
		public bool Host { get; set; }
		public int Health { get; set; }
		public int MaxHealth { get; set; }
		public int Ammo { get; set; }
		public int MaxAmmo { get; set; }
        //public bool Fired { get; set; }
        [JsonIgnore]
		public Player Enemy { get; set; }
		public Jet Jet { get; set; }
		public List<Bullet> Bullets { get; set; }
		public Vector Acceleration;
		public bool KeyShoot { get; set; }
		[JsonIgnore]
		public GameState GameState { get; set; }		
		[JsonIgnore]
		public Dictionary<Action, System.Action<HOTAS>> actionMapping { get; set; }
		public bool isDead { get; private set; }

		public void Act(Tuple<Action, HOTAS> instruction)
		{
			actionMapping[instruction.Item1](instruction.Item2);
		}

		private void MapActions()
		{
            actionMapping = new Dictionary<Action, Action<HOTAS>>
            {
                { Action.Press, Steer },
                { Action.Release, Release }
            };
        }


		public Player(string name, int health, int ammo, Point At, Color color, GameState game)
		{
			Name = name;
			Health = health;
			MaxHealth = health;
			Ammo = ammo;
			MaxAmmo = ammo;
			Jet = new Jet(At, color);
			Bullets = new List<Bullet>();
			//Fired = false;
			GameState = game;
			Acceleration = new Vector();
			MapActions();
		}
      
		//private void bindCommands()
		//{
		//	//commands = new Dictionary<HOTAS, Vector>();
		//	//commands.Add(HOTAS.Up, new Vector(0, -1));
		//	//commands.Add(HOTAS.Down, new Vector(0, 1));
		//	//commands.Add(HOTAS.Left, new Vector(-1,0));
		//	//commands.Add(HOTAS.Right, new Vector(1,0));
		//}

		public override string ToString()
		{
			return "player1";
		}

		public void Recharge(int amount)
		{
			Ammo = Math.Min(Ammo + (int)amount, MaxAmmo);
		}

		public void Heal(int amount)
		{
			Health = Math.Min(Health + amount, MaxHealth);
		}

		public virtual void Steer(HOTAS command)
		{
			switch(command)
			{
				case (HOTAS.Up):
					{
						Acceleration.Y = -1;
						break;
					}
				case (HOTAS.Down):
					{
						Acceleration.Y = 1;
						break;
					}
				case (HOTAS.Left):
					{
						Acceleration.X = -1;
						break;
					}
				case (HOTAS.Right):
					{
						Acceleration.X = 1;
						break;
					}
				case (HOTAS.Shoot):
					{
						KeyShoot = true;
						break;
					}
			}
			Jet.Acceleration = Acceleration;
			//if (SteerKeysBindings.Contains(command))
			//{
			//	Jet.Steer(command);
			//}
			//else if (command == ShootKeyBindings)
			//{
			//	KeyShoot = command;
			//}
		}

		public virtual void Release(HOTAS command)
		{
			switch (command)
			{
				case (HOTAS.Up):
					{
						Acceleration.Y = 0;
						break;
					}
				case (HOTAS.Down):
					{
						Acceleration.Y = 0;
						break;
					}
				case (HOTAS.Left):
					{
						Acceleration.X = 0;
						break;
					}
				case (HOTAS.Right):
					{
						Acceleration.X = 0;
						break;
					}
				case (HOTAS.Shoot):
					{
						KeyShoot = false;
						break;
					}
			}
			Jet.Acceleration = Acceleration;
		}

		public void Aim( Vector at)
		{
			Jet.Aim =  at;
		}
		public void Move()
		{
			Jet.Move(GameState);

			//check for collision with opponent's Jet
			foreach (Bullet b in Bullets)
			{
				if (Enemy.Jet.Collides(b))
				{
					b.HasHit = true;
					Enemy.Hit(1);
				}
			}
			Bullets.ForEach(b => b.Move(GameState));
			Bullets.RemoveAll(b => b.HasHit);

		}
		public virtual void Shoot(int timeElapsed)
		{
			if (KeyShoot)
				Jet.Shoot(this, timeElapsed);
		}

		internal void Hit(int points)
		{
			if (Health > points)
				Health-= points;
			else
				isDead=true;
		}

		internal void Draw(Graphics g)
		{
			Jet.Draw(g);
			Bullets.ForEach(b => b.Draw(g));
		}
    }
}
