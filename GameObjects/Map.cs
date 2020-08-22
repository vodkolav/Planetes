using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects
{
    public  class Map
    {
        List<Wall> Walls;
        Size winSize;
        public Map(Size winSize)
        {
            Walls = new List<Wall>();
            this.winSize = winSize;
        }
         
        public static Map Load(string MapFile)
        {
            return JsonConvert.DeserializeObject<Map>(File.ReadAllText(MapFile));            
        }


        public  List<Wall> LoadDefault( Brush wallBrush, int wallWidth = 20)
        {
            //wallWidth = 20; //default wallwidth
            //wallBrush = Brushes.Magenta;

            Walls.Add(new Wall(new Point(0, 0), new Point(winSize.Width, 0),wallBrush));
            Walls.Add(new Wall(new Point(0, 0), new Size(wallWidth, winSize.Height),wallBrush));
            Walls.Add(new Wall(new Point(0, winSize.Height - wallWidth), new Size(winSize.Width, wallWidth), wallBrush));
            Walls.Add(new Wall(new Point(winSize.Width - wallWidth, 0), new Size(wallWidth, winSize.Height), wallBrush));

            Walls.Add(new Wall(new Point(winSize.Width / 2, 100), new Size(wallWidth, 100), wallBrush));

            Walls.Add(new Wall(new Point(100, 100), new Point(200, 200), wallBrush, wallWidth));
            return Walls;
        }
        public List<Wall> LoadDefault2(Brush wallBrush, int wallWidth = 20)
        {
            //wallBrush = Brushes.Magenta;
            int ww = wallWidth;

            Point nw = new Point(0, 0);
            Point ne = new Point(winSize.Width, 0 );
            Point se = new Point(winSize.Width, winSize.Height);
            Point sw = new Point(0 , winSize.Height);


            Walls.Add(new Wall(nw,ne, wallBrush));
            Walls.Add(new Wall(ne,se, wallBrush));
            Walls.Add(new Wall(se,sw, wallBrush));
            Walls.Add(new Wall(sw,nw, wallBrush));

            int sh = 100; //shift

            Point nww = new Point(0, 0 + sh);
            Point nnw = new Point(0 + sh, 0 );
            Walls.Add(new Wall(nww, nnw, wallBrush));

            Point nne = new Point(winSize.Width -sh, 0);
            Point nee = new Point(winSize.Width , 0 + sh);
            Walls.Add(new Wall(nne, nee, wallBrush));

            Point see = new Point(winSize.Width, winSize.Height - sh);
            Point sse = new Point(winSize.Width - sh, winSize.Height);
            Walls.Add(new Wall(see, sse, wallBrush));

            Point ssw = new Point(0 + sh, winSize.Height);
            Point sww = new Point(0 , winSize.Height - sh );
            Walls.Add(new Wall(ssw, sww, wallBrush));

            Point sp1 = new Point(100, 200);
            Point sp2 = new Point(345, 521);
            Walls.Add(new Wall(sp1, sp2, wallBrush));

            return Walls;
        }
    }
}
