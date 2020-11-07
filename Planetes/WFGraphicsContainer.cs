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
        Graphics G;
        public WFGraphicsContainer(Graphics G)
        {
            this.G = G;
        }

        public void DrawLine(Color c, int Size, Vector Start, Vector End)
        {
            G.DrawLine(new Pen(c, Size), Start.AsPoint, End.AsPoint);
        }

        public void FillEllipse(Color c, Vector Pos, int R)
        {
            G.FillEllipse(new SolidBrush(c), Pos.X, Pos.Y, R, R);
        }

        public void FillPolygon(Color c, PointF[] PointFs)
        {
            G.FillPolygon(new SolidBrush(c), PointFs);
        }
    }
}
