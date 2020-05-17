﻿using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GameObjects
{
	public class Player : MarshalByRefObject
	{
		public string Name { get; set; }
		public bool Host { get; set; }
		public int Health { get; set; }
		public int MaxHealth { get; set; }
		public int Ammo { get; set; }
		public int MaxAmmo { get; set; }
		//public bool Fired { get; set; }
		public Player Enemy { get; set; }
		public Jet Jet { get; set; }
		public List<Bullet> Bulletlist { get; set; }
		public Vector Acceleration;
		public bool KeyShoot { get; set; }
		public ClsGameObjects GameState { get; set; }
		protected List<Keys> SteerKeysBindings;
		protected Keys ShootKeyBindings;
		public Dictionary<HOTAS, Vector> commands { get; set; }
		public bool isDead { get; private set; }


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
			bindCommands();
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

		public void Move()
		{
			Jet.Move(GameState);
		}

		public void Aim(Vector location)
		{
			Jet.Aim = location;
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
