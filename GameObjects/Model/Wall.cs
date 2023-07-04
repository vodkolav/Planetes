using Newtonsoft.Json;
using PolygonCollision;
using System.Drawing;
using System.Linq;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Wall
    {
       
        public Color Color { get; set; }
               
        public Polygon Body { get; set; }

        public Circle BoundingCirc
        {
            get 
            {
                float r = Body.Vertices.Max(v => (v - Body.Center).Magnitude);                
                return new Circle(Body.Center, r); 
            }
        }

        public Wall(Point from, Point to, Color color, int width = 20)
        {
            Body = Construct(from, to, width);

            Color = color;
        }

        private Polygon Construct(Point from, Point to, int w)
        {
            Vector F = new Vector(from);
            Vector T = new Vector(to);

            float alp = (T - F).Angle(new Vector(1, 0));

            Vector UP = new Vector(0, -w / 2).Rotated(-alp);
            Vector DN = new Vector(0, w / 2).Rotated(-alp);

            //Vector shift = width / 2 * new Vector((float)(Math.Cos(Math.PI / 2.0 - alp)), (float)(-Math.Sin(Math.PI / 2.0 - alp)));
            Polygon p = new Polygon();

            p.AddVertex(F + UP);
            p.AddVertex(T + UP);
            p.AddVertex(T + DN);
            p.AddVertex(F + DN);
            p.AddVertex(F + UP);
            return p;
        }

        public void Draw()
        {
            Body.Draw(Color);
        }

    }
}
