using System.Windows.Media;

namespace PolygonCollision
{

    public class Circle : Figure
    {
        public Circle(Vector center, float r)
        {
            Pos = center;
            Size = new Size(r, 0);            
        }

        public float R => Size.X;

        public override Vector Center => Pos;

        public override void Offset(Vector by)
        {
            Pos += by;
        }

        /// <summary>
        /// Get an offsetted copy of this Circle without affecting this one
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Circle Offseted(Vector offset)
        {
            return new Circle(Pos + offset, R);            
        }

        public override void Transformed(Figure blueprint, Vector offset, float rotation)
        {            
            //rotation is irrelevant for circle
            Pos = blueprint.Pos + offset;
        }

        public override void Draw(Color color)
        {
            DrawingContext.GraphicsContainer.FillEllipse(color,this);
        }

        /// <summary>
        /// LINE/CIRCLE
        /// whether this circle collides with line
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public bool Collides(Vector l1, Vector l2)
        {
            float dot = ((Pos - l1) * (l2 - l1)).Sum / (l1 - l2).Pow(2).Sum;

            // find the closest point on the line
            Vector closest = l1 + (dot * (l2 - l1));

            // is this point actually on the line segment?
            // if so keep going, but if not, return false
            bool onSegment = closest.Collides(l1, l2);
            if (!onSegment) return false;

            // get distance to closest point
            float distance = (closest - Pos).Magnitude;

            // is the circle on the line?
            if (distance <= R)
            {
                return true;
            }
            return false;
        }

        public override PolygonCollisionResult Collides(Polygon other, Vector speed)
        {
            //logic for collision of this pair of figures sits in Polygon, so i'm calling it:
            return other.Collides(this, speed); 
        }

        public PolygonCollisionResult Collides(Ray l)
        {            
            bool t = (l.Pos - Pos).Magnitude < R;
            PolygonCollisionResult result = new PolygonCollisionResult()
            {
                Intersect = t,
                WillIntersect = t,
            };

            return result;
        }

        public override PolygonCollisionResult Collides(Circle other, Vector speed)
        {
            if ((Pos - other.Pos).Magnitude <= R + other.R)
                return PolygonCollisionResult.yesCollision;
            return PolygonCollisionResult.noCollision;
        }

        public override object Clone()
        {
            return new Circle(Pos , R);
        }


    }
}