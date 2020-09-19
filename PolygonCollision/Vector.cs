using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PolygonCollision
{
    public class Vector : ICloneable
    {
        public float X { get; set; }

        public float Y { get; set; }

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Vector(PointF p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Vector()
        {
            X = 0;
            Y = 0;
        }

        public Vector(Size size)
        {
            X = size.Width;
            Y = size.Height;
        }

        static public Vector FromPoint(Point p)
        {
            return new Vector(p.X, p.Y);
        }

        static public Vector FromPointF(PointF p)
        {
            return new Vector(p);
        }

        [JsonIgnore]
        public Point AsPoint
        {
            get { return new Point((int)X, (int)Y); }
        }

        [JsonIgnore]
        public float Magnitude
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
        }

        public Vector Pow(float pow)
        {
            return new Vector((float)Math.Pow(X, pow), (float)Math.Pow(Y, pow));
        }

        [JsonIgnore]
        public float Magnitude_X
        {
            get { return Math.Abs(X); }
        }

        [JsonIgnore]
        public float Magnitude_Y
        {
            get { return Math.Abs(Y); }
        }

        public void Normalize()
        {
            float magnitude = Magnitude; //don't delete this line - will result in erroneous results
            X /= magnitude;
            Y /= magnitude;
        }

        /// <summary>
        /// Rotates the vector inplace 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="at"></param>
        public void RotateAt(float angle, Vector at)
        {
            Matrix myMatrix = new Matrix();
            myMatrix.RotateAt(angle, at);
            //This conversion is bad, but I don't know how to rotate vector
            PointF[] p = new PointF[] { AsPoint };
            myMatrix.TransformPoints(p);
            FromPointF(p[0]);
        }

        /// <summary>
        /// returns rotated version of angle 
        /// </summary>		
        public Vector Rotated(float angle)
        {
            double theta = angle * Math.PI / 180;
            float cs = (float)Math.Cos(theta);
            float sn = (float)Math.Sin(theta);
            return new Vector(X * cs - Y * sn, X * sn + Y * cs);
        }

        public Vector GetNormalized()
        {
            float magnitude = Magnitude;
            return new Vector(X / magnitude, Y / magnitude);
        }

        public float Dot(Vector other)
        {
            return (this * other).Sum;
        }

        [JsonIgnore]
        public float Sum
        {
            get { return X + Y; }
        }

        public float Dist(Vector other)
        {
            return (this - other).Magnitude;
        }

        static float Angle(Vector v1, Vector v2)
        {
            // dot product between [x1, y1] and [x2, y2]
            float dot = v1.Dot(v2);
            // determinant
            float det = v1.X * v2.Y - v1.Y * v2.X;
            float angle = (float)(Math.Atan2(det, dot) * 57.2958);
            return angle;
        }

        public float Angle()
        {
            return Angle(this, new Vector(1, 0));
        }

        public float Angle(Vector other)
        {
            return Angle(this, other);
        }


        public static implicit operator Point(Vector p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        public static implicit operator PointF(Vector p)
        {
            return new PointF(p.X, p.Y);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.X, -a.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, float b)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(float b, Vector a)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(Vector a, int b)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector((float)(a.X * b), (float)(a.Y * b));
        }

        public static Vector operator *(Vector a, Vector b)
        {
            return new Vector((float)(a.X * b.X), (float)(a.Y * b.Y));
        }

        public override bool Equals(object obj)
        {
            Vector v = (Vector)obj;

            return X == v.X && Y == v.Y;
        }

        public bool Equals(Vector v)
        {
            return X == v.X && Y == v.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public override string ToString()
        {
            return Math.Abs(X).ToString("+##.##;-##.##;0") + ", " + Math.Abs(Y).ToString("+##.##;-##.##;0");
        }

        public string ToString(bool rounded)
        {
            if (rounded)
            {
                return (int)Math.Round(X) + ", " + (int)Math.Round(Y);
            }
            else
            {
                return ToString();
            }
        }

        public static PointF asPointF(Vector v)
        {
            return new PointF(v.X, v.Y);
        }


        /// <summary>
        /// Whether this vector/point collides with line
        /// </summary>
        /// <param name="l1">point 1 of line </param>
        /// <param name="l2">point 2 of line </param>
        /// <returns></returns>
        public bool Collides(Vector l1, Vector l2)
        {
            // get distance from the point to the two ends of the line
            float d1 = (this - l1).Magnitude;
            float d2 = (this - l2).Magnitude;

            // get the length of the line
            float lineLen = (l1 - l2).Magnitude;

            // since floats are so minutely accurate, add
            // a little buffer zone that will give collision
            float buffer = 0.1f;    // higher # = less accurate

            // if the two distances are equal to the line's
            // length, the point is on the line!
            // note we use the buffer here to give a range, rather
            // than one #
            if (d1 + d2 >= lineLen - buffer && d1 + d2 <= lineLen + buffer)
            {
                return true;
            }
            return false;
        }

        public object Clone()
        {
            return new Vector(X, Y);
        }
    }
}
