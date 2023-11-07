using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public class Wall
    {
       
        public Color Color { get; set; }

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public Corpus _body { get; set; }

        [JsonIgnore]
        public Corpus Body { get => _body; }
        public Vector Pos { get; set; }

        public Circle BoundingCirc {get; set;}

        public Wall()
        {
            
        }

        public static Wall Beam(Vector from, Vector to, Color color, int wallWidth = 20)
        {
            //TODO: move this to polygon class? No

            float alp = (to - from).Angle(new Vector(1, 0));

            Vector UP = new Vector(0, -wallWidth / 2).Rotated(-alp);
            Vector DN = new Vector(0, wallWidth / 2).Rotated(-alp);

            //Vector shift = width / 2 * new Vector((float)(Math.Cos(Math.PI / 2.0 - alp)), (float)(-Math.Sin(Math.PI / 2.0 - alp)));
            Polygon p = new Polygon(4);

            p.AddVertex(from + UP);
            p.AddVertex(to + UP);
            p.AddVertex(to + DN);
            p.AddVertex(from + DN);

            return new Wall(p, color);              

            /*   _body = new Corpus();
               _body.Add(Bar(from, to, width));
               Pos = Body[0].Center;
               float r = ((Polygon)Body[0]).Vertices.Max(v => (v - Pos).Magnitude);
               BoundingCirc = new Circle(Pos, r);
               Color = color;*/
        }

        /// <summary>
        /// Create an axis-aligned Rectangular Wall 
        /// </summary>
        /// <param name="origin"> upper-left corner of the wall </param>
        /// <param name="halfSize">half size of the wall</param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Wall Rectangular(Vector origin, Size halfSize, Color color)
        {        
            //TODO: make it just saize, not halfsize
            Size halfSizeDn = (Size)halfSize.Clone();
            halfSizeDn.Height = -halfSizeDn.Height;

            Polygon P = new Polygon(4);
            P.AddVertex(origin - halfSize);
            P.AddVertex(origin + halfSizeDn);
            P.AddVertex(origin + halfSize);
            P.AddVertex(origin - halfSizeDn);
            return new Wall(P, color);
        }

        /// <summary>
        /// Create a Triangular wall that is circumscribed in a circle with radius 'size.X' (for now).
        /// Each vertice of triangle is placed randomly on a third of that circle
        /// </summary>
        /// <param name="center">center of the circumscribing circle</param>
        /// <param name="size">size of the circumscribing circle</param>
        /// <param name="color">wall color</param>
        /// <returns></returns>
        public static Wall Triangular(Vector center, Size size, Color color)
        {
            // 
            //   /\
            //  /  \
            // /____\

            Polygon res = new Polygon(3);
            float r = size.X;

            for (int i = 0; i <= 2; i++)
            {                
                float th = GameConfig.TossInt(0, 120) * (i + 1) * 0.01745f;
                Vector vtx = center + Vector.FromPolar(r, th);                     
                res.AddVertex(vtx);
                //Walls.Add(new Wall(f, t, wallBrush, (int)w.Magnitude));
            }

            return new Wall(res,color);
        }

        /// <summary>
        /// create a wall out of the given Figure
        /// </summary>
        /// <param name="fig"></param>
        /// <param name="color"></param>
        private Wall(Figure fig, Color color)
        {
            _body = new Corpus();
            _body.Add(fig);
            Pos = Body[0].Center;
            float r = ((Polygon)Body[0]).Vertices.Max(v => (v - Pos).Magnitude);
            BoundingCirc = new Circle(Pos, r);
            Color = color;
        }

        /// <summary>
        /// Like Beam, but if it's longer than MaxSegmentLen, it is cut into several beams, each shorter than MaxSegmentLen.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="color"></param>
        /// <param name="wallWidth"></param>
        /// <param name="MaxSegmentLen"></param>
        /// <returns></returns>
        public static List<Wall> Segmented(Vector from, Vector to, Color color, int wallWidth = 40, float MaxSegmentLen = 200)
        {           

            List < Wall > res = new List<Wall>();

            float len = (to - from).Magnitude;

            if (len / 2 > MaxSegmentLen)
            {
                Vector middle = (to + from) / 2;
                res.AddRange(Segmented(from, middle, color, wallWidth));
                color.R -= (byte)GameConfig.TossInt(100); //make segments of slightly different colors 
                res.AddRange(Segmented(middle, to, color, wallWidth));
            }
            else
            {
                Vector ovlp = (to - from) * 0.05;
                 res.Add(Beam(from - ovlp, to + ovlp, color, wallWidth));
            }
            return res;
        } 

        public void Draw()
        {
            Body[0].Draw(Color);
        }
    }
}
