using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class localBot : Player
    {
        [JsonIgnore]
        private Dictionary<string, object> memory;

        [JsonIgnore]
        private List<HOTAS> directions = new List<HOTAS> { HOTAS.Up, HOTAS.Down };

        public localBot()
        {
        }

        public localBot(int playerID, GameState gS) : base(playerID, "0", "Bot", gS)
        {
            Name = Color.ToString() + " " + GetType().Name;
            memory = new Dictionary<string, object>();
            Init();
        }

        public virtual void FrameReact()
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

        protected virtual void Init()
        {
            memory["count"] = 0;
            memory["direction"] = directions[0];
            memory["amShooting"] = true;
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
    }
}
