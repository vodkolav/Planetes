using Newtonsoft.Json;
using System;
using System.Windows.Media;

namespace PolygonCollision
{
    [JsonObject(IsReference = true)]
    public abstract class Figure : ICloneable
    {
        public Vector Pos { get; set; }

        public Size Size { get; set; }

        public abstract Vector Center { get; }

        public abstract object Clone();

        public abstract PolygonCollisionResult Collides(Polygon other, Vector speed);
     
        public abstract PolygonCollisionResult Collides(Circle other, Vector speed);

        public abstract void Draw(Color color);

        public abstract void Offset(Vector pos);

        public abstract void Transformed(Figure blueprint, Vector offset, float rotation); 
     
    }
}