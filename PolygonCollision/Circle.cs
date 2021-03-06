﻿using System.Drawing;

namespace PolygonCollision
{

    public class Circle
    {
        public Circle(Vector center, int r)
        {
            Pos = center;
            R = r;
        }
        public Vector Pos { get; set; }
        public float R { get; set; }

        public void Offset(Vector by)
        {
            Pos += by;
        }

        public void Draw(Color color)
        {
            DrawingContext.GraphicsContainer.FillEllipse(color,this);
        }

        /// <summary>
        /// LINE/CIRCLE
        /// whether this circle collides with line
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public bool Collides(Vector l1, Vector l2)
        {
            float dot = ((Pos - l1) * (l2 - l1)).Sum / (l1 - l2).Pow(2).Sum;

            // find the closest point on the line
            Vector closest = l1 + (dot * (l2 - l1));

            // is this point actually on the line segment?
            // if so keep going, but if not, return false
            bool onSegment = closest.Collides(l1, l2);
            if (!onSegment) return false;

            // get distance to closest point
            float distance = (closest - Pos).Magnitude;

            // is the circle on the line?
            if (distance <= R)
            {
                return true;
            }
            return false;
        }

        public bool Collides(Ray l)
        {
            return (l.Pos - Pos).Magnitude < R;
        }
    }
}