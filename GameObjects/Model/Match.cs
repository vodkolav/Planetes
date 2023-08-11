using System;
using System.Collections.Generic;
using System.Linq;

namespace GameObjects.Model
{
    public delegate void MatchEventHandler(object source, MatchEventArgs e);

    public class MatchEventArgs : EventArgs
    {
        public Player Obj { get; set; }
        public Player Subj { get; set; }

        public string EventInfo { get; set; }

        public MatchEventArgs(Player obj, Player subj, string info)
        {
            Obj = obj;
            Subj = subj;
            EventInfo = info;
        }

        public string GetInfo()
        {
            return EventInfo;
        }
    }

    public class Stat
    {
        public int Kills { get; set; } = 0;
        public int Deaths { get; set; } = 0;
        public int Assists { get; set; } // that will be harder to implement. will do it later
    }

    public class Match
    {
        private int KillsGoal { get; set; } = 3;

        public float RespawnTime { get; set; } = 5;

        private Dictionary<int, Stat> stats { get; set; } // int is Player.ID 

        private GameState gameObjects { get; set; }

        public void MatchEvent(object source, MatchEventArgs e)
        {
            Reap(e.Obj, e.Subj);
            Logger.Log(e.Obj + e.EventInfo + e.Subj, LogLevel.Info);
        }

        internal void Reap(Player killer, Player victim)
        {
            if (killer != null)
                stats[killer.ID].Kills += 1;
            stats[victim.ID].Deaths += 1;
            victim.DeathTime = GameTime.TotalElapsedSeconds;
            victim.isAlive = false;
            Logger.Log(ShowStats(), LogLevel.Info);
        }

        internal void Respawn(Player player)
        {
            player.Jet = new Jet(player, GameConfig.StartingHP, GameConfig.StartingAmmo);
            player.isAlive = true;
            gameObjects.Entities.Add(player.Jet);
            player.DeathTime = 0;
        }

        public Match()
        {
        }

        public Match(GameState gameObjects)
        {
            this.gameObjects = gameObjects;
            InitFeudingParties();
            stats = new Dictionary<int, Stat>();

            foreach (var player in gameObjects.Players)
            {
                stats.Add(player.ID, new Stat());
                player.OnMatchEvent += MatchEvent;
            }

            Logger.Log("creating match", LogLevel.Nothing);
        }

        public void InitFeudingParties()
        {
            //simplest case: Free-For-All (All-Against-All)
            foreach (Player p1 in gameObjects.Players)
            {
                foreach (Player p2 in gameObjects.Players)
                {
                    p1.FeudWith(p2);
                }
            }
        }

        public void CheckGame(GameServer s)
        {
            // remove, wait and respawn a player's jet
            foreach (var player in gameObjects.Players)
            {
                if (!player.isAlive)
                {
                    float tillRespawn = player.DeathTime + RespawnTime - GameTime.TotalElapsedSeconds;
                    if (tillRespawn > 0)
                    {
                        s.Notify(player, Notification.Death,
                            $" {player.Name} is DEAD. He will respawn in {(int)tillRespawn+1} seconds!");
                    }
                    else
                    {
                        Respawn(player);
                        s.Notify(player, Notification.Respawn, $"respawning player {player.Name}");
                    }
                }
            }
            //&& gameObjects.frameNum % 100 != 0)   /// && player.DeathTime + RespawnTime >= gameObjects.frameNum

            //check if some player reached KillsGoal. If so, game over
            var achievers = stats.Where(k => k.Value.Kills >= KillsGoal);

            if (achievers.Any())
            {
                Player winner = gameObjects.Players.Single(p => p.ID == achievers.Single().Key);

                //Notify winner
                s.Notify(winner, Notification.Won, "Yay! you win! ");
                foreach (Player p in gameObjects.Players.Where(p => p != winner))
                {
                    // notify everyone they lost and who won
                    s.Notify(p, Notification.Lost, $"You lose, sorry. \n {winner.Name} is the winner ");
                }
             gameObjects.gameOver();               
            }


            //TODO:
            // close clients on UIs 
            // end game 
            // reset server
        }

        public string ShowStats()
        {
            string res = "| Player | Kills | Deaths |\n";
            
            foreach (var stat in stats)
            {
                var player = gameObjects.Players.Single(p => p.ID == stat.Key);
                res += $"| {player.Name} | {stat.Value.Kills} | {stat.Value.Deaths} |\n";
            }
            return res;
        }
    }
}
