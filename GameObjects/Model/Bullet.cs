using Newtonsoft.Json;
using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Bullet
    {
        public Vector Pos { get => Body.Pos; }     
        
        public static int linearSpeed = 20;

        public Vector Speed { get => Body.Tail; }

        public int Size { get => Body.Size; }       
        
        Ray Body { get; set; }

        public Color Color { get; set; }

        public bool HasHit { get; set; }

        public int Power { get; set; } = 1;

        public Bullet(Vector pos, Vector speed, int size, Color color)
        {
            Body = new Ray(pos, speed, size);
            Color = color;
            HasHit = false;
        }

        public bool Collides(Astroid a)
        {
            return a.Body.Collides(Body);
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
            //check for collision with wall
            foreach (Wall w in gameObjects.World.Walls)
            {
                if (w.Body.Collides(Pos))
                {
                    HasHit = true;
                    return;
                }
            }

            //check whether a bullet as way outside of world - can remove it then
            if (Pos.Magnitude > new Vector(gameObjects.World.size).Magnitude * 2)
            {
                HasHit = true;
                return;
            }

            foreach (Astroid ast in gameObjects.Astroids)
            {
                if (Collides(ast))
                {
                    HasHit = true;
                    ast.HasHit = true;
                }
            }

            Offset(Speed);
        }

        public void Offset(Vector by)
        {
           Body.Pos += by;
        }
    }
}
