using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects
{ 
    //not quite sure it's supposed to be interface. maybe abstract class
    [JsonObject(IsReference = true)]
    public abstract class ICollideable
    {

        public virtual Player Owner { get; set; }

        public virtual Vector Pos { get; set; }

        public virtual bool HasHit { get; set; }

        public virtual PolygonCollisionResult Collides(Wall w)
        {
            return w.Body.Collides(Pos);
        }

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

        public virtual void Move(GameState gameObjects)
        {
            
        }

        public virtual void Draw()
        {

        }

    }
}