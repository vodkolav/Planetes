using System;
using System.Collections.Concurrent;
using System.Drawing;

namespace GameObjects
{
    public static class GameConfig
    {

        static readonly Random r = new Random();

        public static Size WorldSize { get { return new Size(2500, 1200); } }

        public static TimeSpan FrameInterval { get { return new TimeSpan(0, 0, 0, 0, 16); } }

        public static int WallWidth { get { return 30; } }

        public static int StartingHP { get { return 1000; } }

        public static int StartingAmmo { get { return 1000; } }

        public static float  Thrust { get { return 0.1f; } }

        public static ConcurrentBag<Color> _colors = new ConcurrentBag<Color>() { Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Magenta, Color.Cyan, Color.White, Color.Orange, Color.Purple };

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
        internal static void ReturnColor(Color color)
        {
            _colors.Add(color);
        }
        public static Point TossPoint
        {
            get
            {
                int x = r.Next(WallWidth * 3, WorldSize.Width - WallWidth * 3);
                int y = r.Next(WallWidth * 3, WorldSize.Height - WallWidth * 3);
                return new Point(x, y);
            }
        }

        public static float Lightspeed { get { return 4; } }

        public static int AsteroidTimeout { get { return 50; } }
    }

}
