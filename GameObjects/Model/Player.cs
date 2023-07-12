﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    public enum Action { Press, Release, Aim, setViewPort }
    public enum Notification { DeathNotice, Kicked, Message }

    public struct PlayerInfo
    {
        public string PlayerName { get; set; } 
        public Size VisorSize { get; set; }
    }

    [JsonObject(IsReference = true)]
    public class Player
    {
        public int ID { get; set; }        
        public string ConnectionID { get; set; }
        public string Name { get; set; }
        public Color Color { get { return Jet.Color; } }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Ammo { get; set; }
        public int MaxAmmo { get; set; }        

        public List<Player> Enemies { get; set; }
        public Jet Jet { get; set; }

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
            if (Name == "WPFplayer")
            {
                Logger.Log("sent command: " + instruction.ToString(), LogLevel.Info);
                //Logger.Log("Acceleration: " + Acceleration.ToString(), LogLevel.Info);
            }
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
            viewPort.Size = (Size)argument;
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
                        Jet.KeyBrake = true;
                        break;
                    }
            }
            Jet.Acceleration = Acceleration;
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
                        Jet.KeyBrake = false;
                        break;
                    }
            }
            Jet.Acceleration = Acceleration;
        }

        public virtual void Aim(object argument)
        {
            Jet.Aim = (Vector)argument - (viewPort.Size * .5);
        }

        public virtual void Shoot(GameState gameObjects)
        {
            if (KeyShoot)
            {
                if (Ammo != 0 && gameObjects.frameNum > Jet.LastFired + Jet.Cooldown)
                {
                    Jet.LastFired = gameObjects.frameNum;
                    Bullet bullet = new Bullet(this, Jet.Gun, speed: Jet.Bearing.GetNormalized() * Bullet.linearSpeed /*+ Speed*/, size: 3, color: Color);
                    gameObjects.Entities.Add(bullet);
                    Ammo--;
                }
            }
        }

        internal void Hit(int points)
        {
            if (Health > points)
                Health -= points;
            else
                isAlive = false;

        }
    }
}
