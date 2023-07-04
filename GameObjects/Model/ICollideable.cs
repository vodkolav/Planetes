using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public abstract class ICollideable //TODO: make this a gameObject : ICollidable class 
    {

        public virtual Player Owner { get; set; }

        public virtual Vector Pos { get; set; }

        public virtual bool HasHit { get; set; }

        public virtual Circle BoundingCirc { get { return new Circle(Pos, 10); }}

        public virtual PolygonCollisionResult Collides(Wall w)
        {
            return w.Body.Collides(Pos);
        }

        public PolygonCollisionResult Collides(Map WorldEdge)
        {
            //Object is out of world bounds
            if (Pos.X > WorldEdge.size.Width || Pos.X < 0 || Pos.Y > WorldEdge.size.Height || Pos.Y < 0)
            {
                return PolygonCollisionResult.yesCollision;
            }
            return PolygonCollisionResult.noCollision;
        }

        public abstract void HandleCollision(Map WorldEdge, PolygonCollisionResult r);

        public virtual void HandleCollision(Wall w, PolygonCollisionResult r)
        {
            HasHit = true;
        }

        public virtual PolygonCollisionResult Collides(Jet j)
        {
            return PolygonCollisionResult.noCollision;
        }

        public virtual void HandleCollision(Jet j, PolygonCollisionResult r)
        {
            HasHit = true;
        }

        public virtual PolygonCollisionResult Collides(Astroid a)
        {
            // collisions of Jets with Astroids are handled in astroid class
            // Astroids don't collide each other
            return PolygonCollisionResult.noCollision;
        }

        public virtual void HandleCollision(Astroid a, PolygonCollisionResult r)
        {
            HasHit = true;
            a.HasHit = true;
        }

        public float Dist(ICollideable a)
        {
            return Pos.Dist(a.Pos);
        }

        public abstract void Move(GameState gameObjects);
        public abstract void Draw();
    }
}