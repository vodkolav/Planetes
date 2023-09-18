
using Newtonsoft.Json;

namespace PolygonCollision
{
    public class Size : Vector
    {

        public Size() { }
        public Size(float w, float h) : base(w, h)
        {}

        public Size(Vector v) : base(v.X, v.Y)
        {}

        [JsonIgnore]
        public int Width { get { return (int)X; } set { X = value; } }

        [JsonIgnore]
        public int Height { get { return (int)Y; } set { Y = value; } }

        [JsonIgnore]
        public int Area { get => Width * Height; }
    }
}
