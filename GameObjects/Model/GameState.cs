using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    public enum GameStatus {Ready, Lobby, Cancelled, On, Over }

    public class GameState
    {       
        public GameStatus GameOn { get; set; } = GameStatus.Ready;

        public bool Paused { get; set; } // TODO: allow players to pause game 

        public DateTime StartTime { get; set; }

        public int frameNum { get; set; }

        public List<Player> Players { get; set; }

        [JsonIgnore]
        public IEnumerable<Jet> Jets { get { return Entities.OfType<Jet>(); } }

        [JsonIgnore]
        public IEnumerable<Astroid> Astroids { get { return Entities.OfType<Astroid>(); } } //.Where(e => e.GetType() == typeof(Astroid)).se;

        [JsonIgnore]
        public IEnumerable<Bullet> Bullets { get { return Entities.OfType<Bullet>(); } }

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public List<ICollideable> Entities { get; set; }

        public Map World { get; set; }

        public bool ShouldSerializeWorld()
        {
            // don't remove! even though it has 0 references, this function is essential
            // We need to serialize the World only in lobby phase. 
            // Since the worlds is static, once the game has started, no need to send World to clients anymore
            return GameOn!= GameStatus.On ;
        }

        public Resources ModelStore { get; set; }

        public bool ShouldSerializeModelStore()
        {
            // don't remove! even though it has 0 references, this function is essential
            // We need to serialize the World only in lobby phase. 
            // Since the worlds is static, once the game has started, no need to send World to clients anymore
            return GameOn != GameStatus.On;
        }

        public GameState()
        {
        }

        public GameState(Size worldSize)
        {
            Entities = new List<ICollideable>();
            World = new Map(worldSize);
            Players = new List<Player>();
            ModelStore = Resources.Instance;
            Resources.Init();
            frameNum = 0;
        }

        public void Start()
        {
            GameOn = GameStatus.On;
            GameTime.StartTime = DateTime.UtcNow;
            StartTime = GameTime.StartTime;
        }

        public void Frame()
        {
           // if (Paused) return; //TODO: Note: when game is paused, the time still goes on. need to take it into consideration as well 
            frameNum++;

            /*   if (frameNum >10) // TODO: understand how to properly calculate FPS
            {
               Logger.Log("Model FPS: " + (frameNum / (GameTime.TotalElapsedSeconds)), LogLevel.Status);
            }*/



            PolygonCollisionResult r;

            Players.ForEach(p => p.Shoot(this));

            foreach (ICollideable e in Entities)
            {
                e.Move(GameTime.DeltaTime);
                e.upToDate = false;
            }

            //check for collision of all objects with World bounds
            foreach (ICollideable e in Entities)
            {
                r = e.Collides(World);
                if (r.Intersect)
                {
                    e.HandleCollision(World, r);
                    break;
                }
            }

            //check for collision of Bullets Astroids and Jets with Walls
            foreach (ICollideable e in Entities)
            {
                List<PolygonCollisionResult> collisions = new List<PolygonCollisionResult>();
                foreach (Wall w in World.Walls)
                {
                    r = e.Collides(w);
                    if (r.Intersect)
                    {
                        collisions.Add(r);
                        e.HandleCollision(w, r);
                        break;
                    }
                }
                e.Collisions = collisions;
            }

            //check for collision of Bullets and Astroids with Jets
            List<Type> types = new List<Type>() { typeof(Astroid), typeof(Bullet) };

            foreach (Jet j in Entities.OfType<Jet>())
            {
                foreach (ICollideable e in Entities.Where(e => types.Contains(e.GetType())))
                {
                    r = e.Collides(j);
                    if (r.Intersect)
                    {
                        e.HandleCollision(j, r);
                        break;
                    }
                }
            }
            
            //check for collision of Bullets with Astroids
            foreach (Astroid a in Entities.OfType<Astroid>())
            {
                foreach (ICollideable e in Entities.OfType<Bullet>())
                {
                    r = e.Collides(a);
                    if (r.Intersect)
                    {
                        e.HandleCollision(a, r);
                        break;
                    }
                }
            }

            Entities.RemoveAll(b => !b.isAlive);

            if (GameConfig.EnableAstroids)
            {
                //Spawn asteroid after timeout

                int chance = GameConfig.TossInt(World.Size.Area / (int)GameConfig.AsteroidTimeout);

                if (GameConfig.EnableAstroids && chance < 1) //GameTime.TotalElapsedSeconds % GameConfig.AsteroidTimeout < 0.1)
                {
                    Entities.Add(new Astroid(GameConfig.TossAsteroidType));
                }
            }
        }

        internal void Track(string playerName, string source)
        {
#if DEBUG
            // This function may be useful to track a specific game object (usually Jet)
            // over time and then plot the data. useful when diagnosing fps issues
            try
            {
                if (GameConfig.loglevels.Contains(LogLevel.CSV))
                {
                    if (source == "header")
                    {
                        string CSVheader = ",frame, UtcNow, DeltaTime, Source, JetSpeedMag, JetSpeedX, JetSpeedY, JetPosMag, JetPosX, JetPosY";
                        //string CSVheader = "frame, UtcNow, DeltaTime, Source, JetSpeed, JetPosMag, JetBearingX, JetBearingY";
                        Logger.Log(CSVheader, LogLevel.CSV);
                    }

                    Jet debugged = Players.Single(p => p.Name.ToLower().Contains(playerName)).Jet; // WPFplayer

                    string CSVline = $",{frameNum}, {GameTime.TotalElapsedSeconds:F4}, {GameTime.DeltaTime:F4}, {source}, " +
                                     $"{debugged.Speed.Magnitude:F4}, {debugged.Speed.X:F4},{debugged.Speed.Y:F4}, " +
                                     $"{debugged.Pos.Magnitude}, {debugged.Pos.X}, {debugged.Pos.Y}";
                                    // $"{debugged.Bearing.X}, {debugged.Bearing.Y}";

                    Logger.Log(CSVline, LogLevel.CSV);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Warning);
            }
#endif
        }
    }
}
