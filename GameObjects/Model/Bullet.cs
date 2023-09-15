using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public class Bullet : ICollideable
    {
        public int Width { get; set; }

        public Color Color { get; set; }

        public override int Power { get; internal set; } = 1;

        public override float Rot => 0f;

        public Bullet(Player owner, Vector pos, Vector direction, int speed, int width, Color color)
        {
            Owner = owner;
            Pos = pos;
            direction = direction.GetNormalized();
            _body = new List<Figure> { new Ray(new Vector(0,0), direction * width * 4, width) };
            Speed = direction * speed; 
            Width = width;
            Color = color;
            isAlive = true;
        }   

        public Bullet() { }

        public override PolygonCollisionResult Collides(Wall w)
        {
            return Body[0].Collides((Polygon)w.Body[0], Speed);
        }

        public override PolygonCollisionResult Collides(Astroid a)
        {
            return Body[0].Collides((Circle)a.Body[0],null);

            /* //this implementation might be needed if astroids or bullets become more 
               //complex objects in the future. for now the simpler solution is sufficient 
                        foreach (Figure f in Body)
                        {
                            foreach (Circle o in a.Body)
                            {
                                PolygonCollisionResult r = f.Collides(o, Speed);
                                if (r.WillIntersect)
                                {
                                    return r;
                                }
                            }
                        }
                        return PolygonCollisionResult.noCollision;     */
        }

        public override PolygonCollisionResult Collides(Jet j)
        {

            foreach (Polygon o in j.Body)
            {
                PolygonCollisionResult r = Body[0].Collides(o, Speed);
                if (r.WillIntersect)
                {
                    return r;
                }
            }
            return PolygonCollisionResult.noCollision;
        }

        public override void HandleCollision(Jet j, PolygonCollisionResult r)
        {
            //disables friendly and self fire. TODO: add to gameconfig
            if (Owner.Enemies.Contains(j.Owner))
            {
                isAlive = false;
                j.Hit(this);
            }
        }
        public override void HandleCollision(Map WorldEdge, PolygonCollisionResult r)
        {
            isAlive = false;
        }

        public override void Draw()
        {
            if (isAlive)
            {
                Body[0].Draw(Color);
            }
        }

        public override void Move()
        { 
            Offset(Speed * GameConfig.GameSpeed * GameTime.DeltaTime );
        }

        public void Offset(Vector by)
        {
           Pos += by;
        }
    }
}
