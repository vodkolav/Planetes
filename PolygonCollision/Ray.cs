using System.Drawing;

namespace PolygonCollision
{
    public class Ray
    {
        //A kind of hybrid between a point and a line: 
        //In terms of collision, only it's pointy head (Pos) is relevant, 
        //In terms of drawing, it is represented by a short line, like a blaster shot from star wars

        public Vector Pos { get; set; }

        public Vector Tail { get; set; }

        public int Size { get; set; }

        public bool Collides(Circle a)
        {
            return (Pos - a.Pos).Magnitude < a.R;
        }

        public void Draw(Color c)
        {
            DrawingContext.G.DrawLine(c, Size, Pos - (Tail * 0.5), Pos);
        }
    }
}
