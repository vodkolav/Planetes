using Newtonsoft.Json;
using System.Windows.Media;

namespace PolygonCollision
{
    public class Ray : Figure
    {
        //A kind of hybrid between a point and a line: 
        //In terms of collision, only it's pointy head (Pos) is relevant, 
        //In terms of drawing, it is represented by a short line, like a blaster shot from star wars
        //Also, has line Thickness (Width property)

       
        public Vector Tail { get; set; }

        [JsonIgnore]
        public override Vector Center => Pos;

        [JsonIgnore]
        public int Width => Size.Width;

        public Ray(Vector pos, Vector tail, int width)
        {
            Pos = pos;
            Tail = tail;
            Size = new Size(width, 0);            
        }

        public override PolygonCollisionResult Collides(Polygon other, Vector speed)
        {
            return other.Collides(Pos);
        }

        public override PolygonCollisionResult Collides(Circle other, Vector speed)
        {
            if ((other.Pos - Pos).Magnitude <= other.R)
                return PolygonCollisionResult.yesCollision; 
            else 
                return PolygonCollisionResult.noCollision;
        }

        public override void Transformed(Figure blueprint, Vector offset, float rotation = 0f)
        {
            Pos = blueprint.Pos + offset;
            if (rotation != 0f)
            {
                Tail.RotateAt(rotation, Pos);
            }
        }

        public override void Offset(Vector by)
        {
            Pos += by;
        }

        public override void Draw(Color c)
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
            return new Ray(Pos + offset, Tail, Width);          
        }

        public override object Clone()
        {
            return Offseted(new Vector(0, 0)); 
        }
    }
}
