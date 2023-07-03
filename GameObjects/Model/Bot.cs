using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameObjects
{
    /// <summary>
    /// Baseline for all Bots. 
    /// </summary>
    public abstract class Bot : GameClient 
    {
        protected Thread computer;

        protected Dictionary<string, object> memory;

        protected List<HOTAS> directions = new List<HOTAS> { HOTAS.Up, HOTAS.Down };
        protected Jet Jet { get { return Me.Jet; } }

        public TimeSpan ReactionInterval { get; private set; } = TimeSpan.FromSeconds(1);

        public Bot() : base(new DummyPlug())
        {
            PlayerName = GetType().Name;
            computer = new Thread(BotLoop)
            { 
                Name = "BotThread",
                IsBackground = true
            };
        }

        public override sealed void Start()
        {
            base.Start();
            computer.Start();
        }

        #region Utility functions

        protected virtual Player ClosestEnemy
        {
            get
            {
                if (Me != null)
                {
                    return Me.Enemies.Aggregate((curMin, x) => curMin == null || (Jet.Dist(x.Jet)) < Jet.Dist(curMin.Jet) ? x : curMin);
                }
                else
                { return null; }
            }
        }

        public Astroid ClosestAsteroid
        {
            get
            {
                return gameObjects.Astroids.Aggregate((curMin, x) => curMin == null ||Jet.Dist(x) < Jet.Dist(curMin) ? x : curMin);
            }
        }

        public Bullet ClosestBullet
        {
            get
            {
                return gameObjects.Bullets.Where(b => Me.Enemies.Contains(b.Owner)) //TODO:this is wrong - I dont need to evade my own bullets
                    .Aggregate((curMin, x) => curMin == null || Jet.Dist(x) < Jet.Dist(curMin) ? x : curMin);
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

        public void Press(HOTAS h)
        {
            Yoke.Press(h);
        }

        public void Release(HOTAS h)
        {
            Yoke.Release(h);
        }

        public void Aim(Vector at)
        {
            Yoke.Aim(at);
        }

        private void BotLoop()
        {
            try
            {
                DateTime dt;// maybe replace this with stopwatch
                TimeSpan tdiff;
                memory = new Dictionary<string, object>();
                Prepare();
                while (Me != null)
                {
                    dt = DateTime.UtcNow;
                    //TODO: cancel the call of this function if it takes longer than ReactionInterval 
                    //https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/cancel-async-tasks-after-a-period-of-time
                    FrameReact();
                    tdiff = DateTime.UtcNow - dt;
                    Thread.Sleep(ReactionInterval - tdiff);//this is bad. There should be timer instead

                }
                Logger.Log("A BOT HAS DIED", LogLevel.Info);
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }

        protected sealed override void Die(string message)
        {
            computer.Join();
            base.Die(message);
        }

        protected virtual void Prepare()
        {
            memory["count"] = 0;
            memory["direction"] = directions[0];
            memory["amShooting"] = true;
        }


        /// <summary>
        /// basic logic for simplest bot. 
        /// just move up for 5 frames, start shoot, 5 frmaes down, stop shoot, etc...
        /// </summary>
        protected virtual void FrameReact()
        {
            int count = (int)memory["count"];
            HOTAS direction = (HOTAS)memory["direction"];
            bool amShooting = (bool)memory["amShooting"];

            if (count == 5)
            {
                count = 0;
                direction = pickOpposite(direction);
                Press(direction);
                amShooting = ToggleShoot(amShooting);
            }
            count++;

            memory["count"] = count;
            memory["direction"] = direction;
            memory["amShooting"] = amShooting;
        }
    }



    public class Bot4 : Bot
    {
        public Bot4() : base()
        { }

        protected override void FrameReact()
        {

            //todo: Bot4 logic
            // make bot catch ammo and health crates
            // smarter maneuvring between asteroids and bullets


            //double astavg = gamenow.AstroidList.Where(a => a.Pos.Y + 50 < Jet.Pos.Y && a.Pos.Y - 50 > Jet.Pos.Y).Select(c => c.Pos.X).Average();
            //int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

            try
            {
                //asteroid evasion tactic
                Astroid astClosest = ClosestAsteroid;

                if (astClosest.Pos.X - astClosest.Size * 10 < Jet.Pos.X && Jet.Pos.X < astClosest.Pos.X && astClosest.Type == AstType.Rubble)
                {
                    Press(HOTAS.Left);
                }
                else if (astClosest.Pos.X < Jet.Pos.X && Jet.Pos.X < astClosest.Pos.X + astClosest.Size * 10 && astClosest.Type == AstType.Rubble)
                {
                    Press(HOTAS.Right);
                }
                else
                {
                    Release(HOTAS.Right);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }


            try
            {
                //bullet evasion tactic (not good yet) Where(b=> b.Pos.X + 50 > Jet.Pos.X)

                var bulClosest = ClosestBullet;
                if (Jet.Pos.Y < bulClosest.Pos.Y && bulClosest.Pos.Y < Jet.Pos.Y + 50)
                {
                    Press(HOTAS.Down);
                }
                else if (Jet.Pos.Y - 50 < bulClosest.Pos.Y && bulClosest.Pos.Y <= Jet.Pos.Y)
                {
                    Press(HOTAS.Up);
                }
                else
                {
                    Release(HOTAS.Up);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Warning);
            }

            //aiming at opponent tactic

            Player EnemyClosest = ClosestEnemy;

            Aim(EnemyClosest.Jet.Pos);
            if (Jet.Pos.Y < EnemyClosest.Jet.Pos.Y - 50)
            {

                Press(HOTAS.Down);
            }
            else if (Jet.Pos.Y > EnemyClosest.Jet.Pos.Y + 50)
            {
                Press(HOTAS.Up);
            }
            else
            {
                Release(HOTAS.Up);
            }

            //shoot at opponent tactic
            if ((Jet.Pos - EnemyClosest.Jet.Pos).Magnitude < 300)
            {
                Press(HOTAS.Shoot);
            }
            else
            {
                Release(HOTAS.Shoot);
            }
        }
    }


    public class Bot3 : Bot
    {

        public Bot3() : base()
        { }

        protected override void FrameReact()
        {

            //todo: Bot3 logic
            // make bot catch ammo and health crates
            // smarter maneuvring between asteroids and bullets

            //double astavg = gamenow.AstroidList.Where(a => a.Pos.Y + 50 < Jet.Pos.Y && a.Pos.Y - 50 > Jet.Pos.Y).Select(c => c.Pos.X).Average();
            //int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

            try
            {
                //asteroid evasion tactic

                if (ClosestAsteroid.Pos.X - ClosestAsteroid.Size * 10 < Jet.Pos.X && Jet.Pos.X < ClosestAsteroid.Pos.X && ClosestAsteroid.Type == AstType.Rubble)
                {
                    Press(HOTAS.Left);
                }
                else if (ClosestAsteroid.Pos.X < Jet.Pos.X && Jet.Pos.X < ClosestAsteroid.Pos.X + ClosestAsteroid.Size * 10 && ClosestAsteroid.Type == AstType.Rubble)
                {
                    Press(HOTAS.Right);
                }
                else
                {
                    Release(HOTAS.Right);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }

            try
            {
                //bullet evasion tactic (not good yet)
                Bullet bulClosest = ClosestBullet;

                if (bulClosest.Pos.Y > Jet.Pos.Y && bulClosest.Pos.X + 50 > Jet.Pos.X)
                {
                    Press(HOTAS.Up);
                }
                else if (bulClosest.Pos.Y < Jet.Pos.Y && bulClosest.Pos.X + 50 > Jet.Pos.X)
                {
                    Press(HOTAS.Down);
                }
                else
                {
                    Release(HOTAS.Up);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Warning);
            }

            //aiming at opponent tactic
            if (Jet.Pos.Y - gameObjects.Players[0].Jet.Pos.Y < -20)
            {
                Press(HOTAS.Down);
            }
            else if (Jet.Pos.Y - gameObjects.Players[0].Jet.Pos.Y > 20)
            {
                Press(HOTAS.Up);
            }
            else
            {
                Release(HOTAS.Up);

            }

            //shoot at opponent tactic
            if (Math.Abs(Jet.Pos.Y - gameObjects.Players[0].Jet.Pos.Y) < 20)
            {
                Press(HOTAS.Shoot);
            }
            else
            {
                Release(HOTAS.Shoot);
            }
        }
    }

    public class Bot2 : Bot
    {

        public Bot2() : base()
        { }


        protected override void FrameReact()
        {


            if (Jet.Pos.Y < gameObjects.Players[0].Jet.Pos.Y)
            {
                Yoke.Press(HOTAS.Up);
            }
            else
            {
                Yoke.Press(HOTAS.Down);
            }

            if (Jet.Pos.Y - gameObjects.Players[0].Jet.Pos.Y < 50)
            {
                Yoke.Press(HOTAS.Shoot);
            }
            else
            {
                Yoke.Release(HOTAS.Shoot);
            }

        }
    }
    public class Bot1 : Bot
    {
        public Bot1() 
        { }

        /// <summary>
        /// Constantly accelerates left. every frame aims at closest enemy(if in range) and shoots
        /// </summary>
        protected override void FrameReact()
        {
            int count = (int)memory["count"];
            HOTAS direction = (HOTAS)memory["direction"];
            bool amShooting = (bool)memory["amShooting"];


            Press(HOTAS.Left);
            Aim(ClosestEnemy.Jet.Pos);

            if (Me.Jet.Pos.Dist(ClosestEnemy.Jet.Pos) < 200)
            {
                Press(HOTAS.Shoot);
            }
            else
            {
                Release(HOTAS.Shoot);
            }

            memory["count"] = count;
            memory["direction"] = direction;
            memory["amShooting"] = amShooting;
        }
    }
}
