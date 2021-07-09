namespace GameObjects
{
    public class AI1 : AI
    {
        public override void Init()
        {
            memory["count"] = 0;
            memory["direction"] = HOTAS.Up;
            memory["amShooting"] = true;
        }

        /// <summary>
        /// Constantly accelerates left. every frame aims at closest enemy(if in range) and shoots
        /// </summary>
        public override void Act()
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
