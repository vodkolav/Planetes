using System;
using System.Linq;

namespace GameObjects
{
    public class AI3 : AI
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
            //bullet evasion tactic (not good yet)
            Bullet bulClosest = ClosestEnemyBullet;

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

            //aiming at opponent tactic
            if (Jet.Pos.Y - gameState.players[0].Jet.Pos.Y < -20)
            {
                Press(HOTAS.Down);
            }
            else if (Jet.Pos.Y - gameState.players[0].Jet.Pos.Y > 20)
            {
                Press(HOTAS.Up);
            }
            else
            {
                Release(HOTAS.Up);

            }

            //shoot at opponent tactic
            if (Math.Abs(Jet.Pos.Y - gameState.players[0].Jet.Pos.Y) < 20)
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
