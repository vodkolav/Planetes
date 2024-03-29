﻿using PolygonCollision;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Planetes
{
    class WFGraphicsContainer : IGraphicsContainer
    {
        private Graphics G;

        public Bitmap B { get; set; }

        public WFGraphicsContainer(PictureBox pbxWorld)
        {
            UpdateBitmap(800,600);
            pbxWorld.Image = B;
            //Logger.Log(s.ToString(), LogLevel.Debug);
        }

        public Vector ViewPortOffset { get ; set; } = new Vector(0, 0);

        public void Clear()
        {
            G.Clear(Color.Black);
        }

        public void DrawRay(System.Windows.Media.Color c, Ray ray)
        {
            ray = ray.Offseted(ViewPortOffset);
            G.DrawLine(new Pen(ConvertColor(c), ray.Width ), Vector2Point(ray.Pos), Vector2Point(ray.Pos - ray.Tail));
        }

        public void FillEllipse(System.Windows.Media.Color c, Circle circ)
        {
            circ = circ.Offseted(ViewPortOffset);
            G.FillEllipse(new SolidBrush(ConvertColor(c)), circ.Pos.X, circ.Pos.Y, circ.R*2, circ.R*2);
        }

        public void FillPolygon(System.Windows.Media.Color c, Polygon poly)
        {
            PointF[] PointFs = poly.Offseted(ViewPortOffset).Vertices.ConvertAll(new Converter<Vector, PointF>(Vector2PointF)).ToArray();
            G.FillPolygon(new SolidBrush(ConvertColor(c)), PointFs.Append(PointFs[0]).ToArray()); //append first vertex of the polygon to close it.
        }

        public void FillRectangle(System.Windows.Media.Color c, PolygonCollision.Rectangle rect)
        {
            G.FillRectangle(new SolidBrush(ConvertColor(c)), rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public static Color ConvertColor(System.Windows.Media.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static Point Vector2Point(Vector v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public static PointF Vector2PointF(Vector v)
        {
            return new PointF((int)v.X, (int)v.Y);
        }

        public static Vector Point2Vector(Point p)
        {
            return new Vector(p.X, p.Y);
        }

        public static Vector PointF2Vector(PointF p)
        {
            return new Vector(p.X, p.Y);
        }

        public void UpdateBitmap(int width, int height)
        {
            B = new Bitmap(width, height);
            G = Graphics.FromImage(B);
            G.SmoothingMode = SmoothingMode.AntiAlias;
        }
    }
}
