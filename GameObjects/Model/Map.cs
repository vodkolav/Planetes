using Newtonsoft.Json;
using PolygonCollision;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameObjects
{
    public class Map
    {
        public List<Wall> Walls { get; set; }
        Size winSize;
        public Polygon Space { get; set; }

        public Map(Size size)
        {
            winSize = size;
            Space = new Polygon().FromRectangle(0, 0, size.Width, size.Height);
            Walls = new List<Wall>();            
            Walls = LoadDefault2();
        }

        public static Map Load(string MapFile)
        {
            //todo: implement save
            return JsonConvert.DeserializeObject<Map>(File.ReadAllText(MapFile));
        }

        public void Draw()
        {            //should replace this with buffered image of the map
            Space.Draw(Color.Black);
            Walls.ForEach(w => w.Draw());
        }

        public List<Wall> LoadDefault2()
        {
            Color wallBrush = Color.Magenta;

            int b = 0; //border width

            //create edge of screen walls 
            Point nw = new Point(b, b);
            Point ne = new Point(winSize.Width-b, b);
            Point se = new Point(winSize.Width-b, winSize.Height-b);
            Point sw = new Point(b, winSize.Height-b);


            Walls.Add(new Wall(nw, ne, wallBrush)); //upper
            Walls.Add(new Wall(ne, se, wallBrush)); //right
            Walls.Add(new Wall(se, sw, wallBrush)); //bottom
            Walls.Add(new Wall(sw, nw, wallBrush)); //left

            //create corner diagonal walls 
            int sh = 200; //shift from corner

            Point nww = new Point(0, 0 + sh);
            Point nnw = new Point(0 + sh, 0);
            Walls.Add(new Wall(nww, nnw, wallBrush));

            Point nne = new Point(winSize.Width - sh, 0);
            Point nee = new Point(winSize.Width, 0 + sh);
            Walls.Add(new Wall(nne, nee, wallBrush));

            Point see = new Point(winSize.Width, winSize.Height - sh);
            Point sse = new Point(winSize.Width - sh, winSize.Height);
            Walls.Add(new Wall(see, sse, wallBrush));

            Point ssw = new Point(0 + sh, winSize.Height);
            Point sww = new Point(0, winSize.Height - sh);
            Walls.Add(new Wall(ssw, sww, wallBrush));

            Point sp1 = new Point(100, 200);
            Point sp2 = new Point(345, 521);
            Walls.Add(new Wall(sp1, sp2, wallBrush));

            return Walls;
        }
    }
}
