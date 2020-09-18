using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameObjects
{
    public class GameState
    {
        public static TimeSpan FrameInterval = new TimeSpan(0, 0, 0, 0, 15); // default. If you want to Change it, do it from outside

        public bool GameOn { get; set; } = false;

        public bool Paused { get; set; }

        public int frameNum;
        public Size WinSize { get; set; }

        public Player Winner;
        [JsonIgnore]
        public BlockingCollection<Tuple<string, Notification, string>> messageQ { get; set; }
        //the Tuple holds: int ids of players to send message to and string the message
        public List<Player> players { get; set; }

        public List<Astroid> Astroids { get; set; }

        [JsonIgnore]
        public List<Wall> Walls { get; set; }

        public GameState(Size winSize)
        {
            WinSize = winSize;

            Walls = new Map(winSize).LoadDefault2();
            players = new List<Player>();
            Astroids = new List<Astroid>();
            messageQ = new BlockingCollection<Tuple<string, Notification, string>>();

            frameNum = 0;
        }

        public bool Frame()
        {
            if (Paused) return true;
            lock (this)
            {
                players.ForEach(p => p.Move());
                players.ForEach(p => p.Shoot(frameNum));
                //purge loosers
                Player looser;
                if ((looser = players.FirstOrDefault(p => !p.isAlive)) != null)
                {
                    Over(looser);
                }
            }

            if (GameOn)
            {
                //Move asteroids
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
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Over(Player loser)
        {
            messageQ.Add(new Tuple<string, Notification, string>(loser.ConnectionID, Notification.DeathNotice, "YOU DIED"));
            players.Remove(loser); //when a player is killed - moving mouse along his client makes the game slow - must fix it
        }

        public void Draw(Graphics g)
        {
            //make it iDrawable interface
            Walls.ForEach(w => w.Draw(g));

            lock (this)
            {
                players.ForEach(p => p.Draw(g));
            }

            lock (this)
            {
                Astroids.ForEach(a => a.Draw(g));
            }
        }
    }
}
