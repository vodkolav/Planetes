using PolygonCollision;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PlanetesWPF
{
    public class WPFGraphicsContainer : IGraphicsContainer
    {
        private readonly WriteableBitmap B;

        public WPFGraphicsContainer(WriteableBitmap b)
        {
            B = b;
        }

        public Vector ViewPortOffset { get; set; } = new Vector(0, 0);

        public void Clear()
        {
            B.Clear(Colors.Black);
        }

        public void DrawRay(Color c, Ray ray)
        {
            ray = ray.Offseted(ViewPortOffset);
            Vector End = ray.Pos - ray.Tail*0.5;
            B.DrawLineAa((int)ray.Pos.X, (int)ray.Pos.Y, (int)End.X, (int)End.Y, c,ray.Size);
        }

        public void FillEllipse(Color c, Circle circ)
        {
            circ = circ.Offseted(ViewPortOffset);
            B.FillEllipseCentered((int)circ.Pos.X, (int)circ.Pos.Y, (int)circ.R / 2, (int)circ.R / 2, c);
        }

        public void FillPolygon(Color c, Polygon poly)
        {
            B.FillPolygon(poly.Offseted(ViewPortOffset).ints, c);
            B.DrawPolylineAa(poly.Offseted(ViewPortOffset).ints, c);
        }
    }
}
