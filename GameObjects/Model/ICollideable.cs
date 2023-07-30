using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public abstract class ICollideable //TODO: make this a gameObject : ICollidable class 
    {
        public ICollideable()
        {
            
        }

        public virtual Player Owner { get; set; }

        public virtual Vector Pos { get; set; }

        public virtual Vector Speed { get; set; }

        public virtual bool isAlive { get; set; }

        public virtual Circle BoundingCirc { get { return new Circle(Pos, 10); }}

        public virtual int Power { get; internal set; } = 0;

        public PolygonCollisionResult Collides(Map WorldEdge)
        {
            //Object is out of world bounds
            if (Pos.X > WorldEdge.Size.Width || Pos.X < 0 || Pos.Y > WorldEdge.Size.Height || Pos.Y < 0)
            {
                return PolygonCollisionResult.yesCollision;
            }
            return PolygonCollisionResult.noCollision;
        }

        public virtual PolygonCollisionResult Collides(Wall w)
        {
            return w.Body.Collides(Pos);
        }

        public virtual PolygonCollisionResult Collides(Jet j)
        {
            return PolygonCollisionResult.noCollision;
        }       
         
        public virtual PolygonCollisionResult Collides(Astroid a)
        {
            // collisions of Jets with Astroids are handled in astroid class
            // Astroids don't collide each other
            return PolygonCollisionResult.noCollision;
        }

        public abstract void HandleCollision(Map WorldEdge, PolygonCollisionResult r);

        public virtual void HandleCollision(Wall w, PolygonCollisionResult r)
        {
            isAlive = false;
        }
        
        public virtual void HandleCollision(Jet j, PolygonCollisionResult r)
        {
            isAlive = false;
        }
        
        public virtual void HandleCollision(Astroid a, PolygonCollisionResult r)
        {
            isAlive = false;
            a.isAlive = false;
        }

        public float Dist(ICollideable other)
        {
            return Pos.Dist(other.Pos);
        }

        public abstract void Move(GameState gameObjects);

        public abstract void Draw();
    }
}