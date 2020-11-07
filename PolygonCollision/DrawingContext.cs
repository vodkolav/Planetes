using System.Drawing;

namespace PolygonCollision
{
    public static class DrawingContext 
    {
        public static IGraphicsContainer G { get; set; }
    }

    public interface IGraphicsContainer
    {
        void FillEllipse(Color c , Vector Pos, int R);

        void FillPolygon(Color c, PointF[] PointFs);

        void DrawLine(Color c, int Size, Vector Start , Vector End);
    }
}