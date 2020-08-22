using Newtonsoft.Json;
using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    public class Wall
	{
		[JsonIgnore]
		public Brush Color { get; set; }

		[JsonIgnore]
		public Polygon region { get; set; }


		public Wall(Point location, Size size, Brush color)
		{
			//Rectangle rectangle = new Rectangle(location, size);
			//region = new Region(rectangle);
			Vector v10 = new Vector(1, 0);
			Vector v01 = new Vector(0, 1);
			Vector vsize = new Vector(size.Width, size.Height);
			Vector vloc = new Vector(location);
			region = new Polygon();
			region.AddVertex(vloc);
			region.AddVertex(vloc + v10*vsize);
			region.AddVertex(vloc + vsize);
			region.AddVertex(vloc + v01 * vsize);
			
			//p.AddVertex(A - shift);
			Color = color;
		}

		public Wall(Point from, Point to, Brush color, int width = 20)
		{
			region = Construct(from, to, width);

			Color = color;
		}

		private Polygon Construct(Point from, Point to, int w)
		{
			Vector F = new Vector(from);
			Vector T = new Vector(to);

			float alp = (T - F).Angle(new Vector(1,0));

			Vector UP = new Vector(0, -w / 2).Rotated(-alp);
			Vector DN = new Vector(0, w / 2).Rotated(-alp);

			//Vector shift = width / 2 * new Vector((float)(Math.Cos(Math.PI / 2.0 - alp)), (float)(-Math.Sin(Math.PI / 2.0 - alp)));
			Polygon p = new Polygon();

			p.AddVertex(F + UP);
			p.AddVertex(T + UP);
			p.AddVertex(T + DN);
			p.AddVertex(F + DN);
			return p;
		}

		public void Draw(Graphics g)
		{
			g.FillPolygon(Color, region.PointFs);
		}
	}
}
