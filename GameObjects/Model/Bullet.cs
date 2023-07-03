using Newtonsoft.Json;
using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Bullet : ICollideable
    {
        public override Vector Pos { get => Body.Pos; set => Body.Pos = value; }     
        
        public static int linearSpeed = 20;

        public Vector Speed { get => Body.Tail; }

        public int Size { get => Body.Size; }       
        
        Ray Body { get; set; }

        public Color Color { get; set; }



        public int Power { get; set; } = 1;

        public Bullet(Player owner, Vector pos, Vector speed, int size, Color color)
        {
            Owner = owner;            
            Body = new Ray(pos, speed, size);
            Color = color;
            HasHit = false;
        }   

        public override PolygonCollisionResult Collides(Astroid a)
        {
            return a.Body.Collides(Body);
        }

        public override PolygonCollisionResult Collides(Jet j)
        {
            PolygonCollisionResult r = j.Hull.Collides(Body);
            if (r.WillIntersect)
            {
                return r;
            }
            else
            {
                return j.Cockpit.Collides(Body);
            }
        }

        public override void HandleCollision(Jet j, PolygonCollisionResult r)
        {
            HasHit = true;
            j.Owner.Hit(Power);
        }

        public override void Draw()
        {
            if (!HasHit)
            {
                Body.Draw(Color);
            }
        }

        public override void Move(GameState gameObjects)
        {
            //check whether a bullet is way outside of world - can remove it then
            if (Pos.Magnitude > new Vector(gameObjects.World.size).Magnitude * 2)
            {
                HasHit = true;
                return;
            }
            Offset(Speed);
        }

        public void Offset(Vector by)
        {
           Body.Offset(by);
        }
    }
}
