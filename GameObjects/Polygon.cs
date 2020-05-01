using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GameObjects
{

	// Structure that stores the results of the PolygonCollision function
	public struct PolygonCollisionResult
	{
		public bool WillIntersect; // Are the polygons going to intersect forward in time?
		public bool Intersect; // Are the polygons currently intersecting
		public Vector MinimumTranslationVector; // The translation to apply to polygon A to push the polygons appart.
	}

	public class Polygon
	{

		public void BuildEdges()
		{
			Vector p1;
			Vector p2;
			Edges.Clear();
			for (int i = 0; i < Points.Count; i++)
			{
				p1 = Points[i];
				if (i + 1 >= Points.Count)
				{
					p2 = Points[0];
				}
				else
				{
					p2 = Points[i + 1];
				}
				Edges.Add(p2 - p1);
			}
		}

		public List<Vector> Edges { get; } = new List<Vector>();

		public List<Vector> Points { get; private set; } = new List<Vector>();

		public PointF[] PointFs
		{
			get { return Points.ConvertAll(new Converter<Vector, PointF>(Vector.asPointF)).ToArray(); }
			set { Points = new List<PointF>(value).ConvertAll(new Converter<PointF, Vector>(Vector.FromPointF)); }
		}

		public Vector Center
		{
			get
			{
				float totalX = 0;
				float totalY = 0;
				for (int i = 0; i < Points.Count; i++)
				{
					totalX += Points[i].X;
					totalY += Points[i].Y;
				}

				return new Vector(totalX / (float)Points.Count, totalY / (float)Points.Count);
			}
		}

		public Vector MTV { get; set; }

		public void Offset(Vector v)
		{
			Offset(v.X, v.Y);
		}

		public void Offset(float x, float y)
		{
			for (int i = 0; i < Points.Count; i++)
			{
				Vector p = Points[i];
				Points[i] = new Vector(p.X + x, p.Y + y);
			}
		}

		public void Rotate(float angle)
		{
			Matrix myMatrix = new Matrix();
			myMatrix.RotateAt(angle, Center);
			//PointF[] toRotate = PointFs;
			PointF[] p = PointFs;
			myMatrix.TransformPoints(p);
			PointFs = p;
		}

		public override string ToString()
		{
			string result = "";

			for (int i = 0; i < Points.Count; i++)
			{
				if (result != "") result += " ";
				result += "{" + Points[i].ToString(true) + "}";
			}
			return result;
		}

		[Obsolete]
		public GraphicsPath toGraphicsPath()
		{
			List<PointF> p = Points.ConvertAll(new Converter<Vector, PointF>(Vector.asPointF));

			List<byte> pp = new List<byte>() { (byte)PathPointType.Start };
			for (int i = 0; i < p.Count - 2; i++)
			{
				pp.Add((byte)PathPointType.Line);
			}
			pp.Add((byte)PathPointType.CloseSubpath);

			return new GraphicsPath(p.ToArray(), pp.ToArray());
		}


		// Check if polygon A is going to collide with polygon B for the given velocity
		public PolygonCollisionResult Collision(Polygon polygonA, Polygon polygonB, Vector velocity)
		{
			PolygonCollisionResult result = new PolygonCollisionResult();
			result.Intersect = true;
			result.WillIntersect = true;

			int edgeCountA = polygonA.Edges.Count;
			int edgeCountB = polygonB.Edges.Count;
			float minIntervalDistance = float.PositiveInfinity;
			Vector translationAxis = new Vector();
			Vector edge;

			// Loop through all the edges of both polygons
			for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
			{
				if (edgeIndex < edgeCountA)
				{
					edge = polygonA.Edges[edgeIndex];
				}
				else
				{
					edge = polygonB.Edges[edgeIndex - edgeCountA];
				}

				// ===== 1. Find if the polygons are currently intersecting =====

				// Find the axis perpendicular to the current edge
				Vector axis = new Vector(-edge.Y, edge.X);
				axis.Normalize();

				// Find the projection of the polygon on the current axis
				float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
				ProjectPolygon(axis, polygonA, ref minA, ref maxA);
				ProjectPolygon(axis, polygonB, ref minB, ref maxB);

				// Check if the polygon projections are currentlty intersecting
				if (IntervalDistance(minA, maxA, minB, maxB) > 0) result.Intersect = false;

				// ===== 2. Now find if the polygons *will* intersect =====

				// Project the velocity on the current axis
				float velocityProjection = axis.Dot(velocity);

				// Get the projection of polygon A during the movement
				if (velocityProjection < 0)
				{
					minA += velocityProjection;
				}
				else
				{
					maxA += velocityProjection;
				}

				// Do the same test as above for the new projection
				float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
				if (intervalDistance > 0) result.WillIntersect = false;

				// If the polygons are not intersecting and won't intersect, exit the loop
				if (!result.Intersect && !result.WillIntersect) break;

				// Check if the current interval distance is the minimum one. If so store
				// the interval distance and the current distance.
				// This will be used to calculate the minimum translation vector
				intervalDistance = Math.Abs(intervalDistance);
				if (intervalDistance < minIntervalDistance)
				{
					minIntervalDistance = intervalDistance;
					translationAxis = axis;

					Vector d = polygonA.Center - polygonB.Center;
					if (d.Dot(translationAxis) < 0) translationAxis = -translationAxis;
					MTV = translationAxis;
				}
			}

			// The minimum translation vector can be used to push the polygons appart.
			// First moves the polygons by their velocity
			// then move polygonA by MinimumTranslationVector.
			if (result.WillIntersect) result.MinimumTranslationVector = translationAxis * minIntervalDistance;

			return result;
		}

		// Calculate the distance between [minA, maxA] and [minB, maxB]
		// The distance will be negative if the intervals overlap
		public float IntervalDistance(float minA, float maxA, float minB, float maxB)
		{
			if (minA < minB)
			{
				return minB - maxA;
			}
			else
			{
				return minA - maxB;
			}
		}

		// Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
		public void ProjectPolygon(Vector axis, Polygon polygon, ref float min, ref float max)
		{
			// To project a point on an axis use the dot product
			float d = axis.Dot(polygon.Points[0]);
			min = d;
			max = d;
			for (int i = 0; i < polygon.Points.Count; i++)
			{
				d = polygon.Points[i].Dot(axis);
				if (d < min)
				{
					min = d;
				}
				else
				{
					if (d > max)
					{
						max = d;
					}
				}
			}
		}
	}
}

