﻿using PolygonCollision;
using System.Collections.Generic;
using System.Linq;

namespace GameObjects
{
    /// <summary>
    /// Baseline for all AIs. Provides an interface for an AI to interact with the world and control it's bot. 
    /// </summary>
    public abstract class AI
    {
        public IBot Bot { get; set; }

        protected Player Me { get { return Bot.Me; } }

        protected Jet Jet { get { return Me.Jet; } }

        protected GameState gameState { get { return Me.gameState; } }

        protected Dictionary<string, object> memory { get; set; }

        protected List<HOTAS> directions = new List<HOTAS> { HOTAS.Up, HOTAS.Down };

        public AI(IBot bot): this()
        {
            Bot = bot;            
        }
        public AI()
        {
            memory = new Dictionary<string, object>();
        }

        public abstract void Init();

        public abstract void FrameReact();

        protected void Press(HOTAS direction)
        {
            Bot.Press(direction);
        }

        protected void Release(HOTAS direction)
        {
            Bot.Release(direction);
        }

        protected void Aim(Vector at)
        {
            Bot.Aim(at);
        }


        #region Utility functions

        protected virtual Player ClosestEnemy
        {
            get
            {
                if (Me.Enemies.Count != 0)
                {
                    return Me.Enemies.Aggregate((curMin, x) => curMin == null || Me.Jet.Dist(x.Jet) < Me.Jet.Dist(curMin.Jet) ? x : curMin);
                }
                else
                { return null; }
            }
        }

        protected virtual Astroid ClosestAsteroid
        {
            get
            {
                return Me.gameState.Astroids.Aggregate((curMin, x) => curMin == null || Me.Jet.Dist(x) < Me.Jet.Dist(curMin) ? x : curMin);
            }
        }

        protected virtual HOTAS pickOpposite(HOTAS k)
        {
            return directions.Where(c => c.CompareTo(k) != 0).Single();
        }

        protected virtual bool ToggleShoot(bool amShooting)
        {
            if (amShooting)
            {
                Release(HOTAS.Shoot);
            }
            else
            {
                Press(HOTAS.Shoot);
            }
            return !amShooting;
        }

        #endregion
    }
}