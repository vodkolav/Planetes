using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GameObjects
{
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
			SteerKeysBindings = new List<Keys> { Keys.A, Keys.D, Keys.W, Keys.S };
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


}
