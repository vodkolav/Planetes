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
        public List<Figure> _body { get; set; }

        [JsonIgnore]
        public List<Figure> Body { get => _body; }
        public Vector Pos { get; set; }

        public Circle BoundingCirc {get; set;}

        public Wall()
        {
            
        }

        public Wall(Vector from, Vector to, Color color, int width = 20)
        {
            _body = new List<Figure>
            {
                Construct(from, to, width)
            };
            Pos = Body[0].Center;
            float r = ((Polygon)Body[0]).Vertices.Max(v => (v - Pos).Magnitude);
            BoundingCirc = new Circle(Pos, r);
            Color = color;
        }

        private Polygon Construct(Vector from, Vector to, int w)
        {
            //TODO: move this to polygon class? No
 
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
            Body[0].Draw(Color);
        }
    }
}
