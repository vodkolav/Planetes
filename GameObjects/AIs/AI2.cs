namespace GameObjects
{
    public class AI2 : AI
    {

        public override void Init()
        {
            memory["count"] = 0;
            memory["direction"] = HOTAS.Up;
            memory["amShooting"] = true;
        }

        /// <summary>
        /// basic logic for simplest bot. 
        /// just move up for 5 frames, start shoot, 5 frmaes down, stop shoot, etc...
        /// </summary>
        public override void Act()
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
}
