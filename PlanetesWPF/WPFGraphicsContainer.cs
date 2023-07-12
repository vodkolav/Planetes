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
            Vector End = ray.Pos - ray.Tail;
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

        public void FillRectangle(Color c, Rectangle rect)
        {
            B.FillRectangle((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom, c);
        }

        /// <summary>
        /// My attempt to reproduce WritableBitmapEx's polylineAa with width.
        /// Intention is to outline jets with anti-aliased line, 
        /// but it doesn't really look good on Jets.
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="points"></param>
        /// <param name="color"></param>
        public static void DrawPolylineAa(WriteableBitmap bmp, int[] points, Color color)
        { 
            int icol = WriteableBitmapExtensions.ConvertColor(color);
            int x = points[0];
            int y = points[1];
            for (int i = 2; i < points.Length; i += 2)
            {
                int num = points[i];
                int num2 = points[i + 1];
                WriteableBitmapExtensions.DrawLineAa(bmp, x, y, num, num2, icol, 2);                
                x = num;
                y = num2;
            }
        }
    }
}
