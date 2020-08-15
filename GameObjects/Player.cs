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
		public List<Bullet> Bulletlist { get; set; }
		public Vector Acceleration;
		public bool KeyShoot { get; set; }
		[JsonIgnore]
		public ClsGameObjects GameState { get; set; }		
		[JsonIgnore]
		public Dictionary<Action, System.Action<HOTAS>> actionMapping { get; set; }
		public bool isDead { get; private set; }
		
		public void Act (Tuple<Action , HOTAS> instruction)
        {
			actionMapping[instruction.Item1](instruction.Item2);
        }

		private void MapActions()
        {
			actionMapping = new Dictionary<Action, Action<HOTAS>>();
			actionMapping.Add(Action.Press, Steer);
			actionMapping.Add(Action.Release, Release);
			//actionMapping.Add(Action.Aim, Aim);
		}

		public Player(string name, int health, int ammo, Point At, Color color, ClsGameObjects game)
		{
			Name = name;
			Health = health;
			MaxHealth = health;
			Ammo = ammo;
			MaxAmmo = ammo;
			Jet = new Jet(At, color);
			Bulletlist = new List<Bullet>();
			//Fired = false;
			GameState = game;
			Acceleration = new Vector();
			bindCommands();
			MapActions();
		}

		private void bindCommands()
		{
			//commands = new Dictionary<HOTAS, Vector>();
			//commands.Add(HOTAS.Up, new Vector(0, -1));
			//commands.Add(HOTAS.Down, new Vector(0, 1));
			//commands.Add(HOTAS.Left, new Vector(-1,0));
			//commands.Add(HOTAS.Right, new Vector(1,0));
		}

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
	}
}
