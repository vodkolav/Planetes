
namespace PolygonCollision
{
    public class Size : Vector
    {

        public Size() { }
        public Size(float w, float h) : base(w, h)
        {
        }

        public int Width { get { return (int)X; } set { X = value; } }
        public int Height { get { return (int)Y; } set { Y = value; } }
    }
}
