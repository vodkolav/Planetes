using System;
using System.Linq;

namespace GameObjects
{
    public class AI4 : AI
    {
        public override void Init()
        {
            memory["count"] = 0;
            memory["direction"] = HOTAS.Up;
            memory["amShooting"] = true;
        }

        public override void FrameReact()
        {

            //todo:
            // make bot catch ammo and health crates
            // smarter maneuvring between asteroids and bullets


            //double astavg = gamenow.AstroidList.Where(a => a.Pos.Y + 50 < Jet.Pos.Y && a.Pos.Y - 50 > Jet.Pos.Y).Select(c => c.Pos.X).Average();
            //int astClosest = gamenow.AstroidList.Min(a => Jet.Dist(a));

            try
            {
                //asteroid evasion tactic
                Astroid astClosest = gameState.Astroids.Aggregate((curMin, x) => (curMin == null || (Jet.Dist(x)) < Jet.Dist(curMin) ? x : curMin));

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
                Console.WriteLine(e.Message);
            }


            try
            {
                //bullet evasion tactic (not good yet) Where(b=> b.Pos.X + 50 > Jet.Pos.X)

                //this is wrong - I dont need to evade my own bullets
                Bullet bulClosest = Me.Bullets
                    .Aggregate((curMin, b) => (curMin == null || (Jet.Dist(b)) < Jet.Dist(curMin) ? b : curMin));
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
                Console.WriteLine(e.Message);
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
}
