using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameObjects
{
    public enum Action { Press, Release, Aim, setViewPort }
    public enum Notification { DeathNotice, Kicked, Message }

    public struct PlayerInfo
    {
        public string PlayerName { get; set; } 
        public Vector VisorSize { get; set; }
    }

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
        public Dictionary<Action, Action<object>> actionMapping { get; set; }

        public ViewPort viewPort { get; set; }

        public bool isAlive { get; private set; } = true;

        public void Act(Tuple<Action, HOTAS> instruction)
        {
            actionMapping[instruction.Item1](instruction.Item2);
        }

        public void Act(Tuple<Action, Vector> instruction)
        {
            actionMapping[instruction.Item1](instruction.Item2);
        }

        private void MapActions()
        {
            actionMapping = new Dictionary<Action, System.Action<object>>
            {
                { Action.Press, Steer },
                { Action.Release, Release },
                { Action.Aim, Aim },
                { Action.setViewPort,setViewPort } // This Action is not used for now, but will be useful when you want tot change window size in-game
            };
        }

        public Player()
        {

        }

        public Player(int id, string connectionid, PlayerInfo Info, int health, int ammo, Color color, GameState game)
        {
            ID = id;
            ConnectionID = connectionid;
            Name = Info.PlayerName;
            Health = health;
            MaxHealth = health;
            Ammo = ammo;
            MaxAmmo = ammo;
            Jet = new Jet(this, color);
            Bullets = new List<Bullet>();
            Enemies = new List<Player>();
            gameState = game;
            Acceleration = new Vector();
            viewPort = new ViewPort(this);
            setViewPort(Info.VisorSize);
            MapActions();
        }

        public Player(int id, string connectionid, PlayerInfo Info, GameState game) :
            this(id, connectionid, Info, health: GameConfig.StartingHP,
                 GameConfig.StartingAmmo, GameConfig.TossColor, game)
        {

        }

        internal void setViewPort(object argument)
        {
            viewPort.Size = (Vector)argument;
        }

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

        public virtual void Steer(object argument)
        {
            switch ((HOTAS)argument)
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
                case HOTAS.Brake:
                    {
                        Acceleration = -Jet.Speed * 0.3;
                        break;
                    }
            }
            Jet.Acceleration = Acceleration;

            if (Name == "Human")
            {
                Console.WriteLine("Acceleration: " + Acceleration.ToString());
            }
        }

        public virtual void Release(object argument)
        {
            switch ((HOTAS)argument)
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
                case HOTAS.Brake:
                    {
                        Acceleration.X = 0;
                        Acceleration.Y = 0;                       
                        break;
                    }
            }
            Jet.Acceleration = Acceleration;
        }

        public virtual void Aim(object argument)
        {
            Jet.Aim = (Vector)argument - (viewPort.Size * .5);
        }

        public void Move()
        {
            Jet.Move(gameState);
            viewPort.Update();
            Shoot(gameState.frameNum);
            //check wheteher we've hit some enemies
            Bullets.ForEach(b => b.Move(gameState));
            Bullets.RemoveAll(b => b.HasHit);
        }

        public virtual void Shoot(int timeElapsed)
        {
            if (KeyShoot)
                Jet.Shoot(timeElapsed);
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
