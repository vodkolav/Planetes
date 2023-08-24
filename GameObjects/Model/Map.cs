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
            Space = new Rectangle(0,0, size.Width, size.Height);
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
                                         .Select(i => new Tuple<int, Star>(rand.Next(amount), new Star(new Vector(rand.Next(Size.Width), rand.Next(Size.Height)), 2, Colors.Aquamarine)))
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


            Walls.Add(new Wall(nw, ne, wallBrush)); //upper
            Walls.Add(new Wall(ne, se, wallBrush)); //right
            Walls.Add(new Wall(se, sw, wallBrush)); //bottom
            Walls.Add(new Wall(sw, nw, wallBrush)); //left

            //create corner diagonal walls 
            int sh = 200; //shift from corner

            Vector nww = new Vector(0, 0 + sh);
            Vector nnw = new Vector(0 + sh, 0);
            Walls.Add(new Wall(nww, nnw, wallBrush));

            Vector nne = new Vector(Size.Width - sh, 0);
            Vector nee = new Vector(Size.Width, 0 + sh);
            Walls.Add(new Wall(nne, nee, wallBrush));

            Vector see = new Vector(Size.Width, Size.Height - sh);
            Vector sse = new Vector(Size.Width - sh, Size.Height);
            Walls.Add(new Wall(see, sse, wallBrush));

            Vector ssw = new Vector(0 + sh, Size.Height);
            Vector sww = new Vector(0, Size.Height - sh);
            Walls.Add(new Wall(ssw, sww, wallBrush));

            AddBrackets(sh);
            //AddFreeWalls(sh);
        }

        public void AddSlashWalls(int sh)
        { 
            Vector sp1 = new Vector(100, 200);
            Vector sp2 = new Vector(345, 521);
            Walls.Add(new Wall(sp1, sp2, wallBrush));

            for (int i = 1; i < 8; i++)
            {
                sp1 = new Vector(i * 200, Size.Height - sh);
                sp2 = new Vector(i * 200 + 345, Size.Height - 2*sh);
                Walls.Add(new Wall(sp1, sp2, wallBrush));
            }
        }

        public void AddBrackets(int sh)
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

            //create edge of screen walls 
            Vector nw = new Vector(b, b);
            Vector ne = new Vector(Size.Width - b, b);
            Vector se = new Vector(Size.Width - b, Size.Height - b);
            Vector sw = new Vector(b, Size.Height - b);

            Vector vertSideLen = new Vector(brSideLen, 0);
            Vector horzSideLen = new Vector(0,brSideLen);

            Vector nwsh = new Vector(sh, sh);
            Vector nesh = new Vector(-sh, sh);

            Walls.Add(new Wall(nw + nwsh, nw + nwsh + horzSideLen, wallBrush));
            Walls.Add(new Wall(nw + nwsh, nw + nwsh + vertSideLen, wallBrush));

            Walls.Add(new Wall(ne + nesh, ne + nesh + horzSideLen, wallBrush));
            Walls.Add(new Wall(ne + nesh, ne + nesh - vertSideLen, wallBrush));

            Walls.Add(new Wall(sw - nesh, sw - nesh - horzSideLen, wallBrush));
            Walls.Add(new Wall(sw - nesh, sw - nesh + vertSideLen, wallBrush));

            Walls.Add(new Wall(se - nwsh, se - nwsh - horzSideLen, wallBrush));
            Walls.Add(new Wall(se - nwsh, se - nwsh - vertSideLen, wallBrush));
        }
    }
}
