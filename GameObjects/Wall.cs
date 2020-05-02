using PolygonCollision;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GameObjects
{
	public class Wall : MarshalByRefObject
	{
		public Brush Color { get; set; }

		public Polygon region { get; set; }


		public Wall(Brush color, Point location, Size size)
		{
			//Rectangle rectangle = new Rectangle(location, size);
			//region = new Region(rectangle);
			Vector v10 = new Vector(1, 0);
			Vector v01 = new Vector(0, 1);
			Vector vsize = new Vector(size.Width, size.Height);
			Vector vloc = Vector.FromPoint(location);
			region = new Polygon();
			region.AddVertex(vloc);
			region.AddVertex(vloc + v10*vsize);
			region.AddVertex(vloc + vsize);
			region.AddVertex(vloc + v01 * vsize);
			
			//p.AddVertex(A - shift);
			Color = color;
		}

		public Wall(Brush color, Point from, Point to, int width = 5)
		{
			region = Construct(from, to, width);

			Color = color;
		}

		private Polygon Construct(Point from, Point to, int width)
		{
			Vector A = new Vector(from);
			Vector B = new Vector(to);

			double alp = B.Angle(A);

			Vector shift = width / 2 * new Vector((float)(Math.Cos(Math.PI / 2.0 - alp)), (float)(-Math.Sin(Math.PI / 2.0 - alp)));
			Polygon p = new Polygon();

			p.AddVertex(A + shift);
			p.AddVertex(B + shift);
			p.AddVertex(B - shift);
			p.AddVertex(A - shift);
			return p;
		}


		//private Region Construct(Point from, Point to)//, int w = 5)
		//{
		//	Vector A = new Vector(from);
		//	Vector B = new Vector(to);

		//	double alp = (B - A).Angle;

		//	Vector shift = w / 2 * new Vector((float)(Math.Cos(Math.PI / 2.0 - alp)), (float)(-Math.Sin(Math.PI / 2.0 - alp)));

		//	GraphicsPath path = new GraphicsPath();
		//	path.StartFigure();
		//	path.AddLine((A + shift).asPoint, (B + shift).asPoint);
		//	path.AddLine((B + shift).asPoint, (B - shift).asPoint);
		//	path.AddLine((B - shift).asPoint, (A - shift).asPoint);
		//	path.AddLine((A - shift).asPoint, (A + shift).asPoint);
		//	path.CloseFigure();
		//	Region reg = new Region(path);
		//	return reg;

		//	//Point[] pts = new Point[] { (A + shift).asPoint, (B + shift).asPoint, (B - shift).asPoint, (A - shift).asPoint, };
		//	//byte[] types = new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.CloseSubpath };
		//	//return new Region(new GraphicsPath(pts, types));

		//	//g.FillRegion(Brushes.AliceBlue, r);

		//}

		public void Draw(Graphics g)
		{
			//g.FillRectangle(Color, rectangle);
			//g.FillRegion(Color, region);
			g.FillPolygon(Color, region.PointFs);
		}

		//internal bool IntersectsWith(Polygon hull)
		//{
		//	bool vis = region.Collides(hull);
		//	return vis; //  false;// 
		//}

		public Vector Reflect(Polygon p)
		{
			//Region reflectionPlace = region.Clone();
			//reflectionPlace.Intersect(r);

			//RegionData rd = reflectionPlace.GetRegionData();

			Vector reflectionPoint = new Vector(-10, 5);
			return reflectionPoint;
		}
	}
}
