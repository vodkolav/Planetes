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

        public void DrawRay(System.Drawing.Color c, Ray ray)
        {
            Vector End = ray.Pos - ray.Tail;
            B.DrawLineAa((int)ray.Pos.X, (int)ray.Pos.Y, (int)End.X, (int)End.Y, System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B),ray.Size);
        }

        public void FillEllipse(System.Drawing.Color c, Circle cir)
        {
            B.FillEllipseCentered((int)cir.Pos.X, (int)cir.Pos.Y, (int)cir.R/2, (int)cir.R/2, System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B));
        }

        public void FillPolygon(System.Drawing.Color c, Polygon poly)
        {
            B.FillPolygon(poly.ints, System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B));
        }
    }
}
