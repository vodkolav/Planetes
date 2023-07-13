using System;
using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    public enum Action { Press, Release, Aim, setViewPort }
    public enum Notification { DeathNotice, Won, Lost, Respawned, Joined, Kicked, Message }

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
        public Color Color { get; set; }
        
        public List<Player> Enemies { get; set; }
        public Jet Jet { get; set; }

        
        [JsonIgnore]
        public GameState gameState { get; set; }
        [JsonIgnore]
        public Dictionary<Action, Action<object>> actionMapping { get; set; }

        public ViewPort viewPort { get; set; }

        
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
            Color = color;
            Jet = new Jet(this, health, ammo);
            Enemies = new List<Player>();
            gameState = game;
            viewPort = new ViewPort(this);
            Jet.OnMatchEvent += gameState.Match.MatchEvent;
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

        public virtual void Steer(object argument)
        {
            Jet.Press((HOTAS)argument);
        }

        public virtual void Release(object argument)
        {
            Jet.Release((HOTAS)argument);
        }

        public virtual void Aim(object argument)
        {
            Jet.Aim = (Vector)argument - (viewPort.Size * .5);
        }

        public virtual void Shoot(GameState gameObjects)
        {
            Jet.Shoot(gameObjects);
        }
    }
}
