
namespace PolygonCollision
{
    public class Size : Vector
    {

        public Size() { }
        public Size(float x, float y) : base(x, y)
        {
        }

        public int Width { get { return (int)X; } set { X = value; } }
        public int Height { get { return (int)Y; } set { Y = value; } }
    }
}
