using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameObjects
{
    /// <summary>
    /// Bot that operates remotely, as GameClient 
    /// </summary>
    public class Bot : GameClient, IBot
    {
        protected Thread computer;

        protected Dictionary<string, object> memory;

        protected List<HOTAS> directions = new List<HOTAS> { HOTAS.Up, HOTAS.Down };
        protected Jet Jet { get { return Me.Jet; } }

        public AI Ai { get; set; }




        public Bot(IUI game, AI Ai) : base(game)
        {
            PlayerName = GetType().Name;
            computer = new Thread(BotLoop)
            {
                Name = "BotThread",
                IsBackground = true
            };
            this.Ai = Ai;
        }

        public override sealed void Start()
        {
            base.Start();
            computer.Start();
        }

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
            Yoke.Do(HOTAS.Aim, at);
        }


        private async void BotLoop()
        {
            try
            {
                memory = new Dictionary<string, object>();
                Ai.Init();
                while (Me != null)
                {
                    Ai.FrameReact();
                    await Task.Delay(700);
                }
                Console.WriteLine("A BOT HAS DIED");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected sealed override void Die(string message)
        {
            computer.Join();
            base.Die(message);
        }
    }
}
