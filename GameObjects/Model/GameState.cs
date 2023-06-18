using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameObjects
{
    public class GameState
    {
        public static TimeSpan FrameInterval = GameConfig.FrameInterval; // default. If you want to Change it, do it from GameConfig

        public bool GameOn { get; set; } = false;

        public bool Paused { get; set; }

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
                Astroid astroid = new Astroid(World.size);
                lock (this)
                {
                    Astroids.Add(astroid);
                }
            }

            lock (this) // calculate viewports for each player
            {
                players.ForEach(p => p.viewPort.Update());
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

        public void Draw(ViewPort vp) //maybe it's supposed to be Player , not ViewPort
        {
            //TODO: draw only the objects that are in the ViewPort.
            // for this i need to replace Collides function with Rectangle.intersect - as it's supposed to be computationally cheaper.
            /*  Walls = gameState.World.Walls.Where(w => w.Body.Collides(Body, velocity).Intersect).ToList();
            Players = gameState.players.Where(p => p.Jet.Collides(Body) || p.Bullets.Any(b => Body.Collides(b.Pos))).ToList();
            Astroids = gameState.Astroids.Where(a => !Body.Collides(a.Body)).ToList(); // TODO: understand why Collides here is supposed to be negated? */
           
            lock (this)
            {
                DrawingContext.GraphicsContainer.ViewPortOffset =  -vp.Origin;
            }
            lock (this)// TODO: do these checks in map class 
            {
                DrawingContext.GraphicsContainer.Clear();
                // World.Space.Draw(Color.Black);
            }

            lock (this) // TODO: do these checks in map class 
            {                
                foreach (Wall w in World.Walls)
                {
                    w.Draw();
                }
            }

            lock (this)
            {               
                foreach (Player p in players)
                {
                    p.Draw();
                }
            }

            lock (this)
            {    
                foreach (Astroid a in Astroids)
                {
                    a.Draw();
                }
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
