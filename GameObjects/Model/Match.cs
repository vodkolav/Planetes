﻿using System;
using System.Collections.Generic;

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
        private int KillsGoal { get; set; } = 500;

        public int RespawnTime { get; set; } = 200;

        private Dictionary<int,Stat> stats { get; set; }// int is Player.ID 
        
        private GameState gameObjects { get; set; }
        
        public void MatchEvent(object source, MatchEventArgs e)
        {
            Reap(e.Obj,e.Subj);
            Logger.Log(e.Obj + e.EventInfo + e.Subj, LogLevel.Info);
        }

        internal void Reap(Player killer, Player victim)
        {
            if (killer != null)
                stats[killer.ID].Kills += 1;
            stats[victim.ID].Deaths += 1;
            victim.DeathTime = gameObjects.frameNum;
            victim.isAlive = false;
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

        public Match(GameState gameObjects )
        {
            this.gameObjects = gameObjects;
            InitFeudingParties();
            stats = new Dictionary<int,Stat>();

            foreach (var player in gameObjects.Players)
            {
                stats.Add(player.ID,new Stat());
                player.OnMatchEvent += MatchEvent;
            }
            Logger.Log("creating match",LogLevel.Nothing);
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

        public void  CheckGame(GameServer s)
        {
            //TODO: remove, wait and respawn a player's jet
            foreach (var player in gameObjects.Players)
            {
                if (!player.isAlive)
                {
                    int tillRespawn = player.DeathTime + RespawnTime - gameObjects.frameNum;
                    if (tillRespawn > 0)
                    {
                        if (tillRespawn % 10 == 0)
                        {
                            s.Notify(player, Notification.Death,
                                $" {player.Name} is DEAD. He will respawn in {tillRespawn} frames");
                        }
                    }
                    else
                    {
                        Respawn(player);
                        s.Notify(player, Notification.Respawn, $"respawning player {player.Name}");
                    }
                }
            }
            //&& gameObjects.frameNum % 100 != 0)   /// && player.DeathTime + RespawnTime >= gameObjects.frameNum

            //check if a player reached KillsGoal. If so, game over
            /*var achievers = stats.Where(k => k.Value.Kills >= KillsGoal);

            if (achievers.Any())
            {
                Player winner = gameObjects.Players.Single(p => p.ID == achievers.Single().Key);

                //Notify winner
                s.Notify(winner, Notification.Won, "Congrats! you win! ");
                foreach (Player p in gameObjects.Players.Where(p => p != winner))
                {
                    // notify everyone they lost and who won
                    s.Notify(p, Notification.Lost, $"You lose, sorry. \n {winner.Name} is the winner ");
                }
                throw new Exception("game over");
            }*/

            //TODO:
            // close clients on UIs 
            // end game 
            // reset server
        }
    }
}
