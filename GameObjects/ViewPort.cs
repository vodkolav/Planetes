using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class ViewPort
    {
        public Rectangle Body { get; set; }

        [JsonIgnore]
        public Vector Origin { get { return Body.Origin; } }
        public Vector velocity { get; set; }
        [JsonIgnore]
        public Player P { get; set; }

      
        public Vector Size
        {
            get { return Body.Size; }

            internal set
            {
                if (Body != null) {
                    Body  = new Rectangle(Origin, value);
                }
                else
                {
                    Body = new Rectangle(0, 0, value.X, value.Y);
                }
            }
        }

        public override string ToString()
        {
            return "Origin: " + Origin + "Size: " + Body.Size;
        }

        public ViewPort() 
        {

        }

        internal PolygonCollisionResult Collides(ICollideable entity)
        {
            return Body.Collides(entity.BoundingCirc);
        }

        internal PolygonCollisionResult Collides(Wall entity)
        {
            return Body.Collides(entity.BoundingCirc);
        }

        internal PolygonCollisionResult Collides(Star entity)
        {
            return Body.Collides(entity.BoundingCirc);
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
