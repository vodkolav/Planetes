using Newtonsoft.Json;
using PolygonCollision;
using System.Collections.Generic;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public abstract class ICollideable : ITrackable //TODO: make this a gameEntity : ICollidable class 
    {
        public ICollideable()
        {
            
        }
    
        public bool upToDate { get; set; }

        public virtual Player Owner { get; set; }

        public Vector Pos { get; set; } 

        public abstract float Rot { get; }

        public virtual Vector Speed { get; set; }//TODO: rename speed to velocity

        public virtual bool isAlive { get; set; }

        [JsonIgnore]
        public Corpus _body_cache { get; set; }

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public virtual Corpus _body { get; set; }

        [JsonIgnore]
        public  Corpus Body
        {
            get
            {
                if (_body_cache == null)
                {
                    _body_cache = new Corpus();
                    upToDate = false;
                }
                if (_body_cache.Count == 0)
                {
                    _body.ForEach(f => _body_cache.Add((Figure)f.Clone()));
                }
                if (!upToDate)
                {
                    for (int i = 0; i < _body.Count; i++)
                    {
                        //TODO: since we're only rotating the _body at 0,0, this may be replaced by Rotate
                        //f.RotateAt(angl, Center);
                        //f.Offset(Pos);
                        _body_cache[i].Transformed(_body[i], Pos, Rot);
                    }
                    upToDate = true;
                }
                return _body_cache;
            }
        }

        public virtual Circle BoundingCirc { get { return new Circle(Pos, 10); }}

        public virtual int Power { get; internal set; } = 0;

        /// <summary>
        /// Not yet sure what these are for, but might be useful going further
        /// </summary>
        public List<PolygonCollisionResult> Collisions { get; set; } = new List<PolygonCollisionResult>();

        public PolygonCollisionResult Collides(Map WorldEdge)
        {
            //Object is out of world bounds
            if (Pos.X > WorldEdge.Size.Width || Pos.X < 0 || Pos.Y > WorldEdge.Size.Height || Pos.Y < 0)
            {
                return PolygonCollisionResult.yesCollision;
            }
            return PolygonCollisionResult.noCollision;
        }

        public abstract PolygonCollisionResult Collides(Wall w);

        public abstract PolygonCollisionResult Collides(Jet j);         
         
        public virtual PolygonCollisionResult Collides(Astroid a)
        {
            // collisions of Jets with Astroids are handled in astroid class
            // Astroids don't collide each other
            return PolygonCollisionResult.noCollision;
            //TODO: when upgrade to C# 8 : make this function abstract with default implementation 
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

        public abstract void Move(float DeltaTime);

        public abstract void Draw();

        internal virtual bool OwnedBy(Player pl)
        {
            return Owner.ID == pl.ID;
        }
    }
}