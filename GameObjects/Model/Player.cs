using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameObjects
{
    public enum Action { Press, Release, Aim }
    public enum Notification { DeathNotice, Kicked, Message }

    [JsonObject(IsReference = true)]
    public class Player
    {
        public int ID { get; set; }
        [JsonIgnore]
        public string ConnectionID { get; set; }
        public string Name { get; set; }
        public Color Color { get { return Jet.Color; } }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Ammo { get; set; }
        public int MaxAmmo { get; set; }        

        public List<Player> Enemies { get; set; }
        public Jet Jet { get; set; }
        public List<Bullet> Bullets { get; set; }
        public Vector Acceleration;
        public bool KeyShoot { get; set; }
        [JsonIgnore]
        public GameState gameState { get; set; }
        [JsonIgnore]
        public Dictionary<Action, Action<HOTAS>> actionMapping { get; set; }
        public bool isAlive { get; private set; } = true;

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

        public Player()
        {

        }

        public Player(int id, string connectionid, string name, int health, int ammo, Point At, Color color, GameState game)
        {
            ID = id;
            ConnectionID = connectionid;
            Name = name;
            Health = health;
            MaxHealth = health;
            Ammo = ammo;
            MaxAmmo = ammo;
            Jet = new Jet(At, color);
            Bullets = new List<Bullet>();
            Enemies = new List<Player>();
            gameState = game;
            Acceleration = new Vector();
            MapActions();
        }

        public Player(int id, string connectionid, string playername, GameState game) :
            this(id, connectionid, playername, health: GameConfig.StartingHP,
                 GameConfig.StartingAmmo, GameConfig.TossPoint, GameConfig.TossColor, game)
        {

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
            return Name;
        }

        public void FeudWith(Player enemy)
        {
            if (enemy != this)
            {
                if (!Enemies.Contains(enemy))
                {
                    Enemies.Add(enemy);
                }

                if (!enemy.Enemies.Contains(this))
                {
                    enemy.Enemies.Add(this);
                }
            }
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
            switch (command)
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

            if (Name == "Human")
            {
                Console.WriteLine("Acceleration: " + Acceleration.ToString());
            }
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

        public void Aim(Vector at)
        {
            Jet.Aim = at;
        }
        public void Move()
        {
            Jet.Move(gameState);

            //check wheteher we've hit some enemies
            foreach (Bullet b in Bullets)
            {
                foreach (Player e in Enemies)
                {
                    if (e.Jet.Collides(b))
                    {
                        b.HasHit = true;
                        e.Hit(b.Power);
                        break;
                    }
                }
            }
            Bullets.ForEach(b => b.Move(gameState));
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
                Health -= points;
            else
                isAlive = false;

        }

        internal void Draw()
        {
            Jet.Draw();
            Bullets.ForEach(b => b.Draw());
        }
    }
}
