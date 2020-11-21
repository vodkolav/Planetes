using System.Drawing;

namespace PolygonCollision
{
    public interface IGraphicsContainer
    {
        void FillEllipse(Color c , Circle circ);

        void FillPolygon(Color c, Polygon poly);

        void DrawRay(Color c, Ray ray);
        //void FillQuad(Color c, Polygon poly);
    }
}