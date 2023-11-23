using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public class Map
    {
        // TODO: implement spawning region for asteroids and players

        public Rectangle Space { get; set; }

        public List<Wall> Walls { get; set; }

        public List<Star> Stars { get; set; }

        public Color wallBrush = Colors.Magenta;

        [JsonIgnore]
        public Size Size { get { return Space.Size; } }

        public Map()
        {
        }

        public Map(Size size)
        {
            Space = new Rectangle(0, 0, size.Width, size.Height);
            Walls = new List<Wall>();
            Stars = new List<Star>();
            LoadDefault2();
            MakeStars((int)(size.Area * GameConfig.starsDensity));
        }

        public static Map Load(string MapFile)
        {
            //todo: implement save and load map
            return JsonConvert.DeserializeObject<Map>(File.ReadAllText(MapFile));
        }

        public void Draw()
        {
            Space.Clear();
        }

        public void MakeStars(int amount)
        {
            Logger.Log("Igniting stars", LogLevel.Info);

            Random rand = new Random();
            Stars = Enumerable.Range(0, amount)
                                         .Select(i => new Tuple<int, Star>(rand.Next(amount), new Star(new Vector(rand.Next(Size.Width), rand.Next(Size.Height)), 1, Colors.Aquamarine)))
                                         .OrderBy(i => i.Item1)
                                         .Select(i => i.Item2)
                                         .ToList();
        }

        public void LoadDefault2()
        {
            Logger.Log("Creating world", LogLevel.Info);

            int b = 0; //border width

            //create edge of screen walls 
            Vector nw = new Vector(b, b);
            Vector ne = new Vector(Size.Width - b, b);
            Vector se = new Vector(Size.Width - b, Size.Height - b);
            Vector sw = new Vector(b, Size.Height - b);

            AddWalls(Wall.Segmented(nw, ne, wallBrush)); //upper
            AddWalls(Wall.Segmented(ne, se, wallBrush)); //right
            AddWalls(Wall.Segmented(se, sw, wallBrush)); //bottom
            AddWalls(Wall.Segmented(sw, nw, wallBrush)); //left

            int sh = Math.Min(Size.Width, Size.Height) / 10; //shift from corner is 1/10th of map size

            AddDiagonals(sh, 40);

            AddBrackets(sh, 40);

            AddBox(Size * new Vector(3 / 4f, 1 / 4f), new Size(2 * sh, 3 * sh), 40);

            AddBox(Size / 4, new Size(1 * sh, 1 * sh), 40);

            AddWall(Wall.Triangular(Size * new Vector(1 / 4f, 3 / 4f), new Size(1 * sh, 1 * sh), wallBrush));

            AddHexagon(Size * new Vector(3 / 4f, 3 / 4f), new Size(0.5f * sh, 1 * sh));

            AddWall(Wall.Rectangular(Size * new Vector(3 / 4f, 2 / 4f), new Size(0.5f * sh, 0.5f * sh), Colors.Tomato));
           
            AddBox(Size * new Vector(2 / 4f, 3 / 4f), new Size(1 * sh, 1 * sh), 40);

            AddBox(Size * new Vector(1 / 4f, 2 / 4f), new Size(1 * sh, 1 * sh), 40);

            wallBrush = Colors.Green;
            AddHexagon(Size * new Vector(2 / 4f, 1 / 4f), new Size(0.5f * sh, sh));
        }

        private void AddWalls(IEnumerable<Wall> walls)
        {
            Walls.AddRange(walls);
        }

        private void AddWall(Wall wall)
        {
            Walls.Add(wall);
        }

        /// <summary>
        /// Adds 4 diagonal walls in the corners of the map
        /// </summary>
        /// <param name="sh"></param>
        public void AddDiagonals(int sh, int wallWidth = 20)
        {
            //create diagonal walls in corners
            // /        \
            //
            //
            // \        /

            Vector nww = new Vector(0, 0 + sh);
            Vector nnw = new Vector(0 + sh, 0);
            Walls.Add(Wall.Beam(nww, nnw, wallBrush, wallWidth));

            Vector nne = new Vector(Size.Width - sh, 0);
            Vector nee = new Vector(Size.Width, 0 + sh);
            Walls.Add(Wall.Beam(nne, nee, wallBrush, wallWidth));

            Vector see = new Vector(Size.Width, Size.Height - sh);
            Vector sse = new Vector(Size.Width - sh, Size.Height);
            Walls.Add(Wall.Beam(see, sse, wallBrush, wallWidth));

            Vector ssw = new Vector(0 + sh, Size.Height);
            Vector sww = new Vector(0, Size.Height - sh);
            Walls.Add(Wall.Beam(ssw, sww, wallBrush, wallWidth));
        }

        /// <summary>
        /// Adds a bunch of inclined walls to the Map. 
        /// Not really useful for anything
        /// </summary>
        /// <param name="sh"></param>
        public void AddSlashWalls(int sh)
        {
            Vector sp1 = new Vector(100, 200);
            Vector sp2 = new Vector(345, 521);
            Walls.Add(Wall.Beam(sp1, sp2, wallBrush));

            for (int i = 1; i < 8; i++)
            {
                sp1 = new Vector(i * 200, Size.Height - sh);
                sp2 = new Vector(i * 200 + 345, Size.Height - 2 * sh);
                Walls.Add(Wall.Beam(sp1, sp2, wallBrush));
            }
        }

        public void AddBrackets(int sh, int wallWidth = 20)
        {
            Color wallBrush = Colors.Magenta;
            int b = 0; //border width
            int brSideLen = sh;

            // Adds 4 "brackets to the map in the following configuration:
            //  __        __
            // |            |
            //
            // |__        __|
            //
            // the corners of brackets are shifted "sh" amount from map corners
            // and have length brSideLen

            Vector nw = new Vector(b, b);
            Vector ne = new Vector(Size.Width - b, b);
            Vector se = new Vector(Size.Width - b, Size.Height - b);
            Vector sw = new Vector(b, Size.Height - b);

            Vector horzSideLen = new Vector(brSideLen, 0);
            Vector vertSideLen = new Vector(0, brSideLen);

            int wwsh = wallWidth / 2;
            Vector hsh = new Vector(wwsh, 0);

            Vector vsh = new Vector(0, wwsh);

            Vector nwsh = new Vector(sh, sh);
            Vector nesh = new Vector(-sh, sh);

            Walls.Add(Wall.Beam(nw + nwsh - vsh, nw + nwsh + vertSideLen, wallBrush, wallWidth));
            Walls.Add(Wall.Beam(nw + nwsh - hsh, nw + nwsh + horzSideLen, wallBrush, wallWidth));

            Walls.Add(Wall.Beam(ne + nesh - vsh, ne + nesh + vertSideLen, wallBrush, wallWidth));
            Walls.Add(Wall.Beam(ne + nesh + hsh, ne + nesh - horzSideLen, wallBrush, wallWidth));

            Walls.Add(Wall.Beam(sw - nesh + vsh, sw - nesh - vertSideLen, wallBrush, wallWidth));
            Walls.Add(Wall.Beam(sw - nesh - hsh, sw - nesh + horzSideLen, wallBrush, wallWidth));

            Walls.Add(Wall.Beam(se - nwsh + vsh, se - nwsh - vertSideLen, wallBrush, wallWidth));
            Walls.Add(Wall.Beam(se - nwsh + hsh, se - nwsh - horzSideLen, wallBrush, wallWidth));
        }

        /// <summary>
        /// Adds a rectangle of size 'size' made out of 4 walls to the map 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <param name="wallWidth"></param>
        public void AddBox(Vector center, Size size, int wallWidth = 20)
        {
            Color wallBrush = Colors.Magenta;
            int b = 0; //border width
            int sh = wallWidth / 2;
            Vector hsh = new Vector(sh, 0);
            //  =>

            Vector vsh = new Vector(0, sh);
            //  ||
            //  \/

            Vector origin = center - size / 2;
            // Adds a rectangle made of walls to the map 
            //  _______
            // |   .   |
            // |_______|
            //

            Vector nw = origin;
            Vector ne = origin + new Vector(size.Width - b, b);
            Vector se = origin + size;
            Vector sw = origin + new Vector(b, size.Height - b);

            Walls.Add(Wall.Beam(nw - hsh, ne - hsh * 0.8, wallBrush, wallWidth)); //north
            wallBrush.R -= (byte)GameConfig.TossInt(100); //make segments of slightly different colors 
            Walls.Add(Wall.Beam(ne - vsh, se - vsh * 0.8, wallBrush, wallWidth)); //east
            wallBrush.R -= (byte)GameConfig.TossInt(100); //make segments of slightly different colors 
            Walls.Add(Wall.Beam(se + hsh, sw + hsh * 0.8, wallBrush, wallWidth)); //south
            wallBrush.R -= (byte)GameConfig.TossInt(100); //make segments of slightly different colors 
            Walls.Add(Wall.Beam(sw + vsh, nw + vsh * 0.8, wallBrush, wallWidth)); //west    
        }


        /// <summary>
        /// Constructs a hexagonal structure made of 6 identical wall elements
        /// (walls colors are different)
        /// </summary>
        /// <param name="center"> where the center of hexagon should be</param>
        /// <param name="size">size of an individual wall element</param>
        public void AddHexagon(Vector center, Size size)
        {
            //  ___
            // /   \
            // \___/

            Vector f = center + new Vector(size.Y / 2, -(float)(size.Y * Math.Sin(Math.Sqrt(3) / 2)));
            Vector w = new Vector(size.Width, 0);
            Vector l = new Vector(0, size.Y);
            Vector o;

            for (int i = 0; i <= 5; i++)
            {
                Vector t = f + l;
                Walls.Add(Wall.Beam(f, t, wallBrush, (int)w.Magnitude));
                wallBrush.R -= (byte)GameConfig.TossInt(100); //make segments of slightly different colors

                o = t + w / 2;
                f = t;
                f.RotateAt(60, o);
                l = l.Rotated(60);
                w = w.Rotated(60);
            }
        }
    }
}
