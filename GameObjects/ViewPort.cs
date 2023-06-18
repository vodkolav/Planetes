using Newtonsoft.Json;
using PolygonCollision;
using System.Linq;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class ViewPort
    {
        public Polygon Body { get; set; }
        [JsonIgnore]
        public Vector Origin { get { return Body.Vertices[0]; } }
        public Vector velocity { get; set; }
        [JsonIgnore]
        public Player P { get; set; }

        private Vector size { get; set; }
      
        public Vector Size
        {
            get { return size; }

            internal set
            {                
                size = value;
                if (Body != null) {
                    Body  = new Polygon().FromRectangle(Body.Vertices.Min(v => v.X), Body.Vertices.Min(v => v.Y), value.X, value.Y);
                }
                else
                {
                    Body = new Polygon().FromRectangle(0, 0, value.X, value.Y);
                }
            }
        }

        public ViewPort() 
        {

        }

        public ViewPort(Player player) 
        {
            velocity = new Vector(0,0);
            P = player;
            Size = new Vector(800, 600);
        }

        internal void Update()
        {
            Vector target = P.Jet.Pos;
            Vector source = Body.Center;
            Vector ofst = target - source;
            Body.Offset(ofst);
        }
    }
}
