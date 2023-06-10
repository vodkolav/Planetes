using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void DrawRay(System.Drawing.Color c, Ray ray)
        {
            ray = ray.Offseted(ViewPortOffset);
            Vector End = ray.Pos - ray.Tail;
            B.DrawLineAa((int)ray.Pos.X, (int)ray.Pos.Y, (int)End.X, (int)End.Y, System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B),ray.Size);
        }

        public void FillEllipse(System.Drawing.Color c, Circle circ)
        {
            circ = circ.Offseted(ViewPortOffset);
            B.FillEllipseCentered((int)circ.Pos.X, (int)circ.Pos.Y, (int)circ.R/2, (int)circ.R/2, System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B));
        }

        public void FillPolygon(System.Drawing.Color c, Polygon poly)
        {
            B.FillPolygon(poly.Offseted(ViewPortOffset).ints, System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B));
        }
    }
}
