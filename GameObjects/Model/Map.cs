using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Map
    {
        // TODO: implement spawning region for asteroids and players

        public PolygonCollision.Rectangle Space { get; set; }

        public List<Wall> Walls { get; set; }

        public List<Star> Stars { get; set; }

        [JsonIgnore]
        public Size Size { get { return new Size((int)Space.Size.X,(int)Space.Size.Y); } }

        public Map()
        {  
        }

        public Map(Size size)
        {
            Space = new PolygonCollision.Rectangle(0,0, size.Width, size.Height);
            Walls = new List<Wall>();
            Stars = new List<Star>();
            LoadDefault2();
            MakeStars(500);
        }




        public static Map Load(string MapFile)
        {
            //todo: implement save and load map
            return JsonConvert.DeserializeObject<Map>(File.ReadAllText(MapFile));
        }

        public void Draw()
        {
            DrawingContext.GraphicsContainer.Clear();            
        }

        public void MakeStars(int amount)
        {
            Logger.Log("making stars", LogLevel.Info);

            Random rand = new Random();
            Stars = Enumerable.Range(0, amount)
                                         .Select(i => new Tuple<int, Star>(rand.Next(amount), new Star(new Vector(rand.Next(Size.Width), rand.Next(Size.Height)), 2, Color.Aquamarine)))
                                         .OrderBy(i => i.Item1)
                                         .Select(i => i.Item2)
                                         .ToList();
        }

        public void LoadDefault2()
        {
            Logger.Log("loading map", LogLevel.Info);
            Color wallBrush = Color.Magenta;

            int b = 0; //border width

            //create edge of screen walls 
            Vector nw = new Vector(b, b);
            Vector ne = new Vector(Size.Width-b, b);
            Vector se = new Vector(Size.Width-b, Size.Height-b);
            Vector sw = new Vector(b, Size.Height-b);


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
    }
}
