using System;
using System.Collections.Concurrent;
using System.Drawing;

namespace GameObjects
{
    static class GameConfig
    {
        public static Size WorldSize { get { return new Size(1600, 776); } }

        public static int WallWidth { get { return 30; } }

        public static int StartingHP { get { return 100; } }

        public static int StartingAmmo { get { return 1000; } }

        public static ConcurrentBag<Color> _colors = new ConcurrentBag<Color>() { Color.Blue , Color.Red, Color.Green, Color.Yellow, Color.Magenta, Color.Cyan, Color.White, Color.Orange };
     
        public static Color TossColor {
            get {
                Color b;
                if(_colors.TryTake(out b))
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
                Random r = new Random();
                int x = r.Next(WallWidth*3, WorldSize.Width - WallWidth*3);
                int y = r.Next(WallWidth*3, WorldSize.Height - WallWidth*3);
                return new Point(x,y);
            }
        }

        public static float Lightspeed { get { return 20; } }
    }

}
