using Newtonsoft.Json;
using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Bullet : ICollideable
    {
        public Vector Pos { get => Body.Pos; set => Body.Pos = value; }     
        
        public static int linearSpeed = 20;

        public Vector Speed { get => Body.Tail; }

        public int Size { get => Body.Size; }       
        
        Ray Body { get; set; }

        public Color Color { get; set; }

        public Player Owner { get; set; }

        public bool HasHit { get; set; }

        public int Power { get; set; } = 1;

        public Bullet(Player owner, Vector pos, Vector speed, int size, Color color)
        {
            Owner = owner;            
            Body = new Ray(pos, speed, size);
            Color = color;
            HasHit = false;
        }   

        public bool Collides(Astroid a)
        {
            return a.Body.Collides(Body);
        }

        public PolygonCollisionResult Collides(Wall w)
        {
             return w.Body.Collides(Pos);
        }

        public bool Collides(Jet j)
        {
            return j.Hull.Collides(Pos).Intersect || j.Cockpit.Collides(Pos).Intersect;
        }


        public void Draw()
        {
            if (!HasHit)
            {
                Body.Draw(Color);
            }
        }

        public void Move(GameState gameObjects)
        {
            //check whether a bullet is way outside of world - can remove it then
            if (Pos.Magnitude > new Vector(gameObjects.World.size).Magnitude * 2)
            {
                HasHit = true;
                return;
            }

            //check for collision with wall
            foreach (Wall w in gameObjects.World.Walls)
            {
                if (Collides(w).Intersect)
                {
                    HasHit = true;
                    return;
                }
            }

            foreach (Player e in Owner.Enemies)
            {
                if (Collides(e.Jet))
                {
                    HasHit = true;
                    e.Hit(Power);
                    return;
                }
            }

            foreach (Astroid ast in gameObjects.Astroids)
            {
                if (Collides(ast))
                {
                    HasHit = true;
                    ast.HasHit = true;
                    return;
                }
            }
            Offset(Speed);
        }

        public void Offset(Vector by)
        {
           Body.Offset(by);
        }


    }
}
