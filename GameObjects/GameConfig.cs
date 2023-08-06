using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using PolygonCollision;

namespace GameObjects
{
    public static class GameConfig
    {

        static readonly Random r = new Random();

        public static Size WorldSize { get { return new Size(1200, 600); } }

        public static float GameSpeed => 7.0f;

        public static TimeSpan FrameInterval { get { return new TimeSpan(0, 0, 0, 0, 8); } } //TODO: drop this
        
        public static double JetScale => 0.6;

        public static float Lightspeed { get { return 40 ; } }

        public static int bulletSpeed { get { return 200; } }

        public static float Cooldown { get { return 0.1f; } }

        public static bool EnableAstroids { get { return false; } }

        public static float AsteroidTimeout { get { return 5; } }

        public static float  Thrust { get { return 4f; } } 



        public static int WallWidth { get { return 30; } }

        public static int StartingHP { get { return 50; } }

        public static int StartingAmmo { get { return 1000; } }



        public static ConcurrentBag<Color> _colors = new ConcurrentBag<Color>()
        {
            Colors.Blue, 
            Colors.Red, 
            Colors.Green, 
            Colors.Yellow, 
            Colors.Magenta, 
            Colors.Cyan, 
            Colors.White, 
            Colors.Orange, 
            Colors.Purple,
            Colors.Chartreuse,
            Colors.OliveDrab,
        };

        public static Color TossColor
        {
            get
            {
                Color b;
                if (_colors.TryTake(out b))
                {
                    return b;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("colors", "no more colors to define for player");
                }
            }
        }

        public static string GetColorName(Color col)
        {
            PropertyInfo colorProperty = typeof(Colors).GetProperties()
                .FirstOrDefault(p => Color.AreClose((Color)p.GetValue(null), col));
            return colorProperty != null ? colorProperty.Name : "unnamed color";

        }

        internal static void ReturnColor(Color color)
        {
            _colors.Add(color);
        }

        public static Vector TossPoint
        {
            get
            {
                int x = r.Next(WallWidth * 3, WorldSize.Width - WallWidth * 3);
                int y = r.Next(WallWidth * 3, WorldSize.Height - WallWidth * 3);
                return new Vector(x, y);
            }
        }
    }
}
