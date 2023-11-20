using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public class ViewPort
    {     
        [JsonIgnore]
        public Rectangle Body
        {
            get
            {
                return new Rectangle(Origin, Size);
            }
        }

        [JsonIgnore]
        public Vector Origin { get { return Pos - Size / 2; } }

        public Vector velocity { get; set; }

        public Player P { get; set; }

        [JsonIgnore]
        public Vector Pos { get { return P.Jet.Pos; } }

        public Size Size { get; set; }                
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
            Size = new Size(800, 600); //? 
        }
    }
}
