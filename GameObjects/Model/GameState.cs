using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameObjects
{
    public class GameState
    {
        public static TimeSpan FrameInterval = GameConfig.FrameInterval; // default. If you want to Change it, do it from GameConfig

        public bool GameOn { get; set; } = false;

        public bool Paused { get; set; } // TODO: allow players to pause game 

        public int frameNum { get; set; }

        public List<Player> Players { get; set; }

        public List<Jet> Jets { get; set; }

        public List<Astroid> Astroids { get; set; }

        public List<Bullet> Bullets { get; set; }

        public Map World { get; set; }

        public GameState()
        {
            World = new Map(GameConfig.WorldSize);
            Players = new List<Player>();
            Astroids = new List<Astroid>();
            Bullets = new List<Bullet>();
            Jets = new List<Jet>();
            frameNum = 0;
        }

        public void Frame()
        {
            if (Paused) return;
            frameNum++;
            
            lock (this) // execute actions for each player
            {
                Jets.ForEach(j => j.Move(this));

                Players.ForEach (p => p.Shoot(this));

                Bullets.ForEach(b => b.Move(this));
                Bullets.RemoveAll(b => b.HasHit);

                Astroids.ForEach(a => a.Move(this));
                Astroids.RemoveAll(a => a.HasHit);

                //Spawn asteroid after timeout
                if (frameNum % GameConfig.AsteroidTimeout == 0)
                {
                    Astroids.Add(new Astroid(World.size));
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
