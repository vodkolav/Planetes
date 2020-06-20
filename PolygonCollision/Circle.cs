using System;
using System.Drawing;

namespace PolygonCollision
{
	public class Circle : MarshalByRefObject
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

		public void Draw(Graphics g, Brush color)
		{
			g.FillEllipse(color, Pos.X, Pos.Y , R, R);
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
			// get length of the line
			float len = (l1 - l2).Magnitude;
			//float distX = x1 - x2;
			//float distY = y1 - y2;
			//float len = (distX * distX) + (distY * distY);

			// get dot product of the line and circle

			//float dot = (((cx - x1) * (x2 - x1)) + ((cy - y1) * (y2 - y1))) / 
			//              (x1 - x2 * x1 - x2) + (y1 - y2 * y1 - y2);


			float dot = ((Pos - l1) * (l2 - l1)).Sum / (l1 - l2).Pow(2).Sum;

			// find the closest point on the line
			Vector closest = l1 + (dot * (l2 - l1));
			//float closestX = x1 + (dot * (x2 - x1));
			//float closestY = y1 + (dot * (y2 - y1));

			// is this point actually on the line segment?
			// if so keep going, but if not, return false
			bool onSegment = closest.Collides(l1, l2);  //x1, y1, x2, y2, closestX, closestY);
			if (!onSegment) return false;



			// get distance to closest point
			Vector dist = closest - Pos;
			//distX = closestX - cx;
			//distY = closestY - cy;
			float distance = dist.Magnitude; //sqrt((distX * distX) + (distY * distY));

			// is the circle on the line?
			if (distance <= R)
			{
				return true;
			}
			return false;
		}
	}
}