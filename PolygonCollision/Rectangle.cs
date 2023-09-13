
using Newtonsoft.Json;
using System.Windows.Media;

namespace PolygonCollision
{
    public class Rectangle : Figure
    {

        public Rectangle()
        {

        }

        public Rectangle(Vector origin, Size size)
        {            
            Size = size;
            Pos = origin + Size / 2;
        }

        public Rectangle(float ox, float oy, float sx, float sy) : this(new Vector(ox, oy), new Size(sx, sy))
        { }

        [JsonIgnore]
        public  Vector Origin => Pos - Size / 2;

        [JsonIgnore]
        public float Left {  get { return Origin.X; }  }
        [JsonIgnore]
        public float Right { get { return Origin.X + Size.X; }  }
        [JsonIgnore]
        public float Top { get { return Origin.Y; }  }
        [JsonIgnore]
        public float Bottom { get { return Origin.Y + Size.Y; } }
        [JsonIgnore]
        public override Vector Center { get { return Pos; } }

        public override PolygonCollisionResult Collides(Circle bcirc, Vector speed )
        {
            return Collides(bcirc);
        }

        public PolygonCollisionResult Collides(Circle bcirc)
        {
            if (Left < bcirc.Pos.X + bcirc.R && bcirc.Pos.X - bcirc.R < Right
                && Top < bcirc.Pos.Y + bcirc.R && bcirc.Pos.Y - bcirc.R < Bottom)
                return PolygonCollisionResult.yesCollision;
            else return PolygonCollisionResult.noCollision;
        }

        public override PolygonCollisionResult Collides(Polygon other, Vector speed)
        {
            //TODO: implement. For now i'm using the bounding circle approach.
            return PolygonCollisionResult.noCollision;
        }

        public void Clear()
        {
            DrawingContext.GraphicsContainer.Clear();
        }

        public override void Draw(Color color)
        {
            DrawingContext.GraphicsContainer.FillRectangle(color, this);
        }

        public override void Transformed(Figure blueprint, Vector offset, float rotation)
        {
            // rotation is irrelevant for rectangle. If you need it to rotate, use Polygon instead.
            Pos = blueprint.Pos = offset;
        }

        public override void Offset(Vector ofst)
        {
            Pos += ofst; 
        }

        public override object Clone()
        {
            return new Rectangle(Origin, Size);
        }
    }
}