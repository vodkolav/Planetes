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
               
        public Polygon Body { get; set; }

        public Circle BoundingCirc
        {
            get 
            {
                float r = Body.Vertices.Max(v => (v - Body.Center).Magnitude);                
                return new Circle(Body.Center, r); 
            }
        }

        public Wall()
        {
            
        }

        public Wall(Vector from, Vector to, Color color, int width = 20)
        {
            Body = Construct(from, to, width);

            Color = color;
        }

        private Polygon Construct(Vector from, Vector to, int w)
        {
 
            float alp = (to - from).Angle(new Vector(1, 0));

            Vector UP = new Vector(0, -w / 2).Rotated(-alp);
            Vector DN = new Vector(0, w / 2).Rotated(-alp);

            //Vector shift = width / 2 * new Vector((float)(Math.Cos(Math.PI / 2.0 - alp)), (float)(-Math.Sin(Math.PI / 2.0 - alp)));
            Polygon p = new Polygon(5);

            p.AddVertex(from + UP);
            p.AddVertex(to + UP);
            p.AddVertex(to + DN);
            p.AddVertex(from + DN);
            p.AddVertex(from + UP);
            return p;
        }

        public void Draw()
        {
            Body.Draw(Color);
        }
    }
}
