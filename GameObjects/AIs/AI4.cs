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

        public override void Act()
        {
            //todo:
            // make bot catch ammo and health crates
            // smarter maneuvring between asteroids and bullets

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
            //bullet evasion tactic 

            Bullet bulClosest = ClosestEnemyBullet;
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
