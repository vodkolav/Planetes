using System.Windows.Media;

namespace PolygonCollision
{
    public interface IGraphicsContainer
    {
        Vector ViewPortOffset { get; set; }

        void FillEllipse(Color c, Circle circ);

        void FillPolygon(Color c, Polygon poly);

        void FillRectangle(Color c, Rectangle rect);

        void DrawRay(Color c, Ray ray);

        void Clear();

        void UpdateBitmap(int width, int height);
    }
}