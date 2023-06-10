using PolygonCollision;
using System.Drawing;

namespace Planetes
{
    class WFGraphicsContainer : IGraphicsContainer
    {
        readonly Graphics G;
        public WFGraphicsContainer(Graphics G)
        {
            this.G = G;
        }

        public Vector ViewPortOffset { get ; set; } = new Vector(0, 0);

        public void Clear()
        {
            G.Clear(Color.Black);
        }

        public void DrawRay(Color c, Ray ray)
        {
            ray = ray.Offseted(ViewPortOffset);
            G.DrawLine(new Pen(c, ray.Size ), ray.Pos.AsPoint, (ray.Pos - ray.Tail).AsPoint);
        }

        public void FillEllipse(Color c, Circle circ)
        {
            circ = circ.Offseted(ViewPortOffset);
            G.FillEllipse(new SolidBrush(c), circ.Pos.X, circ.Pos.Y, circ.R, circ.R);
        }

        public void FillPolygon(Color c, Polygon poly)
        {
            G.FillPolygon(new SolidBrush(c), poly.Offseted(ViewPortOffset).PointFs);
        }
    }
}
