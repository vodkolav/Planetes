using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planetes
{
    class WFGraphicsContainer : IGraphicsContainer
    {
        readonly Graphics G;
        public WFGraphicsContainer(Graphics G)
        {
            this.G = G;
        }      

        public void DrawRay(Color c, Ray ray)
        {
            G.DrawLine(new Pen(c, ray.Size ), ray.Pos.AsPoint, (ray.Pos - ray.Tail).AsPoint);
        }

        public void FillEllipse(Color c, Circle circ)
        {
            G.FillEllipse(new SolidBrush(c), circ.Pos.X, circ.Pos.Y, circ.R, circ.R);
        }

        public void FillPolygon(Color c, Polygon poly)
        {
            G.FillPolygon(new SolidBrush(c), poly.PointFs);
        }
    }
}
