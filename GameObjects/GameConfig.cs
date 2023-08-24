using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using GameObjects.Model;
using PolygonCollision;

namespace GameObjects
{
    public static class GameConfig
    {

        static readonly Random r = new Random();

        public static Size WorldSize { get { return new Size(1400, 1000); }}

        public static float starsDensity { get => 1.0f/1500;}

        public static int KillsGoal { get { return 1; } }

        public static float GameSpeed => 7.0f;
        
        public static double JetScale => 0.6;

        public static float Lightspeed { get { return 30 ; } }

        public static int bulletSpeed { get { return 80; } }

        public static float Cooldown { get { return 0.1f; } }

        public static bool EnableAstroids { get { return true; } }

        public static float AsteroidTimeout { get { return 100; } }

        public static float  Thrust { get { return 5f; } }

        public static int WallWidth { get { return 30; } }

        public static int StartingHP { get { return 50; } }

        public static int StartingAmmo { get { return 1000; } }

        #region debug
        public static bool LogRedirectToFile { get { return false; } }

        public static List<LogLevel> loglevels
        {
            get
            {
                return new List<LogLevel>()
                {
                    LogLevel.Debug,
                    //LogLevel.Info,
                    //LogLevel.Warning,
                    LogLevel.Status,
                    //LogLevel.CSV,
                    //LogLevel.Trace
                };
            }
        }

        #endregion


        public static List<Color> _colors = new List<Color>()
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
                lock (_colors)
                {
                    Color b;
                    if (_colors.Count > 0)
                    {
                        int i = r.Next(_colors.Count);
                        b = _colors[i];
                        _colors.RemoveAt(i);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("colors", "no more colors to define for player");
                    }
                    return b;
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
            lock (_colors)
            {
                _colors.Add(color);
            }
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

        public static int TossInt(int max)
        {
            return r.Next(max);
        }

        public static int TossInt( int min, int max)
        {
            return r.Next(min, max);
        }

        public static  AstType TossAsteroidType
        {
            get
            {
                switch (TossInt(7))
                {
                    case 1:
                        return AstType.Ammo;
                    case 2:
                        return AstType.Health;
                    default:
                        return AstType.Rubble;
                }
            }
        }
    }
}
