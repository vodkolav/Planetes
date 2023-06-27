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

        public List<Player> players { get; set; }

        public List<Astroid> Astroids { get; set; }

        public Map World { get; set; }

        public GameState()
        {
            World = new Map(GameConfig.WorldSize);
            players = new List<Player>();
            Astroids = new List<Astroid>();
            frameNum = 0;
        }

        public void Frame()
        {
            if (Paused) return;
            frameNum++;
            
            lock (this) // execute actions for each player
            {
                players.ForEach(p => p.Move());

                Astroids.ForEach(b => b.Move(this));
                Astroids.RemoveAll(c => c.HasHit);

                //Spawn asteroid after timeout
                if (frameNum % Astroid.Timeout == 0)
                {
                    Astroids.Add(new Astroid(World.size));
                }
            }
        }        

        public Player Reap()
        {
            Player loser;
            if ((loser = players.FirstOrDefault(p => !p.isAlive)) != null)
            {
                players.Remove(loser);
                //Must remove a dead player from all other players enemies lists,
                //otherwise from the point of view of the bullets the dead one still exists 
                players.ForEach(p => p.Enemies.Remove(loser));               
                return loser;
            }
            return null;
        }

        public void InitFeudingParties()
        {
            //simplest case: Free-For-All (All-Against-All)
            foreach (Player p1 in players)
            {
                foreach (Player p2 in players)
                {
                    p1.FeudWith(p2);
                }
            }
        }
    }
}
