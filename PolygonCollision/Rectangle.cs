
using Newtonsoft.Json;
using System.Windows.Media;

namespace PolygonCollision
{
    public class Rectangle
    {

        public Rectangle()
        {

        }

        public Rectangle(Vector origin, Size size)
        {
            Origin = origin;
            Size = size;
        }

        public Rectangle(float ox, float oy, float sx, float sy) : this(new Vector(ox, oy), new Size(sx, sy))
        { }

        public  Vector Origin { get; set; }
        public  Size Size { get; set; }
        [JsonIgnore]
        public float Left {  get { return Origin.X; }  }
        [JsonIgnore]
        public float Right { get { return Origin.X + Size.X; }  }
        [JsonIgnore]
        public float Top { get { return Origin.Y; }  }
        [JsonIgnore]
        public float Bottom { get { return Origin.Y + Size.Y; } }
        [JsonIgnore]
        public Vector Center { get { return Origin + Size/2; } }

        public PolygonCollisionResult Collides(Circle bcirc)
        {
            if (Left < bcirc.Pos.X + bcirc.R && bcirc.Pos.X - bcirc.R < Right
                && Top < bcirc.Pos.Y + bcirc.R && bcirc.Pos.Y - bcirc.R < Bottom)
                return PolygonCollisionResult.yesCollision;
            else return PolygonCollisionResult.noCollision;
        }

        public void Clear()
        {
            DrawingContext.GraphicsContainer.Clear();
        }

        public void Draw(Color color)
        {
            DrawingContext.GraphicsContainer.FillRectangle(color, this);
        }

        public void Offset(Vector ofst)
        {
            Origin += ofst; 
        }
    }
}