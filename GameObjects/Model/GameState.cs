using Newtonsoft.Json;
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

        public bool Paused { get; set; }

        public int frameNum { get; set; }

        public Size WinSize { get; set; }
                       
        public List<Player> players { get; set; }

        public List<Astroid> Astroids { get; set; }

        [JsonIgnore]
        public List<localBot> localBots { get { return players.OfType<localBot>().ToList(); } }

        public Map World { get; set; }

        public GameState(Size winSize)
        {
            WinSize = winSize;

            World = new Map(winSize);
            players = new List<Player>();
            Astroids = new List<Astroid>();   
            frameNum = 0;
        }

        public void Frame()
        {
            if (Paused) return;
            lock (this)
            {
                players.ForEach(p => p.Move());
                players.ForEach(p => p.Shoot(frameNum));
            }

            lock (this)
            {
                Astroids.ForEach(b => b.Move(this));
                Astroids.RemoveAll(c => c.HasHit);
            }
            //Spawn asteroid after timeout
            if (frameNum % Astroid.Timeout == 0)
            {
                Astroid astroid = new Astroid(WinSize);
                lock (this)
                {
                    Astroids.Add(astroid);
                }
            }

            frameNum++;
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



        public void Draw()
        {
            //make it iDrawable interface?
            lock (this)
            {
                World.Draw();
            }
            lock (this)
            {
                players.ForEach(p => p.Draw());
            }

            lock (this)
            {
                Astroids.ForEach(a => a.Draw());
            }
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
