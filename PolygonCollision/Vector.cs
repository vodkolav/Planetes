using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PolygonCollision
{

	public struct Vector {

		public float X;
		public float Y;

		public Vector(float x, float y) {
			X = x;
			Y = y;
		}

		public Vector(int x, int y) {
			X = x;
			Y = y;
		}

		public Vector (Point p)
		{
			this = new Vector(p.X, p.Y);
		}

		public Vector(PointF p)
		{
			this = new Vector(p.X, p.Y);
		}

		static public Vector FromPoint(Point p) {
			return new Vector(p.X, p.Y);
		}

		static public Vector FromPointF(PointF p)
		{
			return new Vector(p.X, p.Y);
		}

		public Point AsPoint
		{
			get { return new Point((int)X, (int)Y); }
		}

		public float Magnitude {
			get { return (float)Math.Sqrt(X * X + Y * Y); }
		}

		public void Normalize() {
			float magnitude = Magnitude;
			X = X / magnitude;
			Y = Y / magnitude;
		}

		public void RotateAt(float angle, Vector at)
		{
			Matrix myMatrix = new Matrix();
			myMatrix.RotateAt(angle, at);
			//This conversion is bad, but I don't know how to rotate vector
			PointF[] p = new PointF[] { AsPoint };
			myMatrix.TransformPoints(p);
			FromPointF(p[0]);
		}

		public Vector GetNormalized() {
			float magnitude = Magnitude;

			return new Vector(X / magnitude, Y / magnitude);
		}

		public float Dot(Vector other) {
			return (this * other).Sum();
		}

		public float Sum()
		{
			return X + Y;
		}

		public float DistanceTo(Vector vector) {
			return (float)Math.Sqrt(Math.Pow(vector.X - this.X, 2) + Math.Pow(vector.Y - this.Y, 2));
		}

		static float angle(Vector v1, Vector v2)
		{
			float dot = v1.Dot(v2);      // dot product between [x1, y1] and [x2, y2]
			float det = v1.X * v2.Y - v1.Y * v2.X;      // determinant
			float angle = (float)(Math.Atan2(det, dot) * 57.2958);
			//Console.WriteLine("X:{0}| Y:{1} | Angle:{2})", v2.x, v2.y, angle);
			return angle;
		}


		public float Angle()
		{
				return angle(this, new Vector(1, 0));
		}

		public float Angle( Vector other)
		{
			return angle(this,other);
		}


		public static implicit operator Point(Vector p) {
			return new Point((int)p.X, (int)p.Y);
		}

		public static implicit operator PointF(Vector p) {
			return new PointF(p.X, p.Y);
		}

		public static Vector operator +(Vector a, Vector b) {
			return new Vector(a.X + b.X, a.Y + b.Y);
		}

		public static Vector operator -(Vector a) {
			return new Vector(-a.X, -a.Y);
		}

		public static Vector operator -(Vector a, Vector b) {
			return new Vector(a.X - b.X, a.Y - b.Y);
		}

		public static Vector operator *(Vector a, float b) {
			return new Vector(a.X * b, a.Y * b);
		}

		public static Vector operator *(float b,Vector a)
		{
			return new Vector(a.X * b, a.Y * b);
		}

		public static Vector operator *(Vector a, int b) {
			return new Vector(a.X * b, a.Y * b);
		}

		public static Vector operator *(Vector a, double b) {
			return new Vector((float)(a.X * b), (float)(a.Y * b));
		}

		public static Vector operator *(Vector a, Vector b)
		{
			return new Vector((float)(a.X * b.X), (float)(a.Y * b.Y));
		}

		public override bool Equals(object obj) {
			Vector v = (Vector)obj;

			return X == v.X && Y == v.Y;
		}

		public bool Equals(Vector v) {
			return X == v.X && Y == v.Y;
		}

		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public static bool operator ==(Vector a, Vector b) {
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(Vector a, Vector b) {
			return a.X != b.X || a.Y != b.Y;
		}

		public override string ToString() {
			return X + ", " + Y;
		}

		public string ToString(bool rounded) {
			if (rounded) {
				return (int)Math.Round(X) + ", " + (int)Math.Round(Y);
			} else {
				return ToString();
			}
		}

		public static PointF asPointF(Vector v)
		{
			return new PointF(v.X, v.Y);
		}
	}

}
