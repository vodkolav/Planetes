using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameObjects
{
    public class GameState
    {
        public static TimeSpan FrameInterval = GameConfig.FrameInterval; // default. If you want to Change it, do it from GameConfig

        public bool GameOn { get; set; } = false;

        public bool Paused { get; set; } // TODO: allow players to pause game 
        [JsonIgnore]
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
            return !GameOn;
        }

        public GameState()
        {
        }

            public GameState(Size worldSize)
        {
            Entities = new List<ICollideable>();
            World = new Map(worldSize);
            Players = new List<Player>();
            frameNum = 0;
        }

        public void Frame()
        {
            if (Paused) return;
            frameNum++;
            //Logger.Log("Model FPS: " + frameNum / (DateTime.Now - StartTime).TotalSeconds, LogLevel.Status);
            lock (this) // execute actions for each player
            {
                PolygonCollisionResult r;

                Players.ForEach(p => p.Shoot(this));

                Entities.ForEach(j => j.Move(this));

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
                foreach (Wall w in World.Walls)
                {           
                    foreach (ICollideable e in Entities)
                    {
                        r = e.Collides(w);
                        if (r.Intersect)
                        {
                            e.HandleCollision(w, r);
                            break;
                        }
                    }
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
                            e.HandleCollision(a,r);
                            break;
                        }
                    }
                }

                Entities.RemoveAll(b => b.HasHit);

                //Spawn asteroid after timeout
                if (frameNum % GameConfig.AsteroidTimeout == 0)
                {
                    Entities.Add(new Astroid(World.Size));
                }
            }
        }        

        public Player Reap()
        {
            Player loser;
            if ((loser = Players.FirstOrDefault(p => !p.isAlive)) != null)
            {
                Players.Remove(loser);
                //Must remove a dead player from all other players enemies lists,
                //otherwise from the point of view of the bullets the dead one still exists 
                Players.ForEach(p => p.Enemies.Remove(loser));               
                return loser;
            }
            return null;
        }

        public void InitFeudingParties()
        {
            //simplest case: Free-For-All (All-Against-All)
            foreach (Player p1 in Players)
            {
                foreach (Player p2 in Players)
                {
                    p1.FeudWith(p2);
                }
            }
        }
    }
}
