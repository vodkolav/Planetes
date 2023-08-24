
namespace PolygonCollision
{
    public class Size : Vector
    {

        public Size() { }
        public Size(float w, float h) : base(w, h)
        {}

        public Size(Vector v) : base(v.X, v.Y)
        {}

        public int Width { get { return (int)X; } set { X = value; } }
        public int Height { get { return (int)Y; } set { Y = value; } }

        public int Area { get => Width * Height; }
    }
}
