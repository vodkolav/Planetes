using System;
using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    public enum Action { Press, Release, Aim, SetViewPort }

    public enum Notification { Joined, JoinFailed, Kicked, Death, Respawn, Won, Lost, Message, GameOver }
     
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

        //TODO: use hex strings to store color data instead, ex:  "Color": "#FF6B8E23",
        public Color Color { get; set; }
      
        public List<Player> Enemies { get; set; }
        public Jet Jet { get; set; }

        public event MatchEventHandler OnMatchEvent;

        public float DeathTime { get; set; }

        public bool isAlive { get; set; } = true;

        [JsonIgnore]
        public GameState gameState { get; set; }

        public ViewPort viewPort { get; set; }

        public void MatchEvent(ICollideable killer)
        {
            if (OnMatchEvent != null)
            {
                OnMatchEvent(this, new MatchEventArgs(killer.Owner, this, " Killed "));
            }
        }

        public void Act<T>(Tuple<Action, T> instruction)
        {
            if (Name == "WPFplayer")
            {
                Logger.Log("sent command: " + instruction.ToString(), LogLevel.Info);
                //Logger.Log("Acceleration: " + Acceleration.ToString(), LogLevel.Info);
            }

            switch (instruction.Item1)
            {

                case Action.Press:
                {
                    Steer(instruction.Item2);
                    break;
                }

                case Action.Release:
                {
                    Release(instruction.Item2);
                    break;
                }
                case Action.Aim:
                {
                    Aim(instruction.Item2);
                    break;
                }
                case Action.SetViewPort:
                {
                    setViewPort(instruction.Item2);
                    break;
                }
            }
        }

        public Player()
        {

        }

        public Player(int id, string connectionid, PlayerInfo Info, int health, int ammo, Color color, GameState game)
        {
            ID = id;
            ConnectionID = connectionid;
            Name = GameConfig.GetColorName(color) + "_" + Info.PlayerName;
            Color = color;
            Jet = new Jet(this, health, ammo);
            Enemies = new List<Player>();
            gameState = game;
            viewPort = new ViewPort(this);
            setViewPort(Info.VisorSize);
        }

        public Player(int id, string connectionid, PlayerInfo Info, GameState game) :
            this(id, connectionid, Info, health: GameConfig.StartingHP,
                 GameConfig.StartingAmmo, GameConfig.TossColor, game)
        {

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

        public virtual void Steer(object argument)
        {
            if ((HOTAS)argument == HOTAS.GameSpeedUp || (HOTAS)argument == HOTAS.GameSpeedDown)
                ChangeGameSpeed((HOTAS)argument);
            else
            if (isAlive)
                Jet.Press((HOTAS)argument);
        }

        private void ChangeGameSpeed(HOTAS argument)
        {
            switch (argument)
            {
                case HOTAS.GameSpeedUp:
                    {
                        if (GameConfig.GameSpeed < 10)
                        GameConfig.GameSpeed += 1;
                        break;
                    }
                case HOTAS.GameSpeedDown:
                    {
                        if (GameConfig.GameSpeed > 1)
                            GameConfig.GameSpeed -= 1;
                        break;
                    }
            }
        }

        public virtual void Release(object argument)
        {
            if (isAlive)
                Jet.Release((HOTAS)argument);
        }

        public virtual void Aim(object argument)
        {
            //argument is vector which holds coordinates of crosshair relative to the Visor UI element's Origin
            if (isAlive)
                //Jet.Aim is the coordinates of crosshair relative to the Visor UI element's Center
                //In other words, the vector from the player's jet to crosshair
                //Or, un-Normalized Bearing
                Jet.Aim = (Vector)argument - (viewPort.Size/2);
            // Print world-relative coordinates of where the mouse now points 
            Logger.Log(((Vector)argument + viewPort.Origin).ToString(), LogLevel.Status);
        }

        internal void setViewPort(object argument)
        {
            viewPort.Size = new Size((Vector)argument);
        }

        public virtual void Shoot(GameState gameObjects) // maybe move this to Jet
        {
            if (isAlive)
                Jet.Shoot(gameObjects);
        }
    }
}
