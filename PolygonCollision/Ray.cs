using System.Windows.Media;

namespace PolygonCollision
{
    public class Ray
    {
        //A kind of hybrid between a point and a line: 
        //In terms of collision, only it's pointy head (Pos) is relevant, 
        //In terms of drawing, it is represented by a short line, like a blaster shot from star wars
        //Also, has line Thickness (Size property)

        public Ray(Vector pos, Vector tail, int size)
        {
            Pos = pos;
            Tail = tail;
            Size = size;
        }

        public Vector Pos { get; set; }

        public Vector Tail { get; set; }

        public int Size { get; set; }

        public void Offset(Vector by)
        {            
            Pos = new Vector(Pos.X + by.X, Pos.Y + by.Y);
        }
        public void Draw(Color c)
        {
            DrawingContext.GraphicsContainer.DrawRay(c, this);
        }

        /// <summary>
        /// Get an offsetted copy of this Ray, without affecting this one
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Ray Offseted(Vector offset)
        {
            return new Ray(Pos + offset, Tail, Size);          
        }
    }
}
