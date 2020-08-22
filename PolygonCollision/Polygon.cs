using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;

namespace PolygonCollision
{

	// Structure that stores the results of the PolygonCollision function
	public struct PolygonCollisionResult
	{
		public bool WillIntersect; // Are the polygons going to intersect forward in time?
		public bool Intersect; // Are the polygons currently intersecting
		public Vector MinimumTranslationVector; // The translation to apply to polygon A to push the polygons appart.
		public Vector translationAxis;
	}

	public class Polygon
	{
		public List<Vector> BuildEdges()
		{
			Vector p1;
			Vector p2;
			List<Vector> Edges = new List<Vector>();
			for (int i = 0; i < Vertices.Count; i++)
			{
				p1 = Vertices[i];
				if (i + 1 >= Vertices.Count)
				{
					p2 = Vertices[0];
				}
				else
				{
					p2 = Vertices[i + 1];
				}
				Edges.Add(p2 - p1);
			}
			return Edges;
		}


		public void AddVertex(Vector vertex)
		{
			Vertices.Add(vertex);
		}

		[JsonIgnore]
		public List<Vector> Edges { get { return BuildEdges(); } }


		public Vector Edge(int i)
		{		
			if (i > Vertices.Count)
			{ throw new IndexOutOfRangeException(string.Format("There are only {0} vertices", Vertices.Count)); }

			Vector p1 = Vertices[i];
			Vector p2;
			if (i + 1 >= Vertices.Count)
			{
				p2 = Vertices[0];
			}
			else
			{
				p2 = Vertices[i + 1];
			}
			return p2 - p1;			 
		}


		public List<Vector> Vertices { get; private set; } = new List<Vector>();
		[JsonIgnore]
		public PointF[] PointFs
		{
			get { return Vertices.ConvertAll(new Converter<Vector, PointF>(Vector.asPointF)).ToArray(); }
			set { Vertices = new List<PointF>(value).ConvertAll(new Converter<PointF, Vector>(Vector.FromPointF)); }
		}

		[JsonIgnore]
		public Vector Center
		{
			get
			{
				float totalX = 0;
				float totalY = 0;
				for (int i = 0; i < Vertices.Count; i++)
				{
					totalX += Vertices[i].X;
					totalY += Vertices[i].Y;
				}

				return new Vector(totalX / (float)Vertices.Count, totalY / (float)Vertices.Count);
			}
		}


		public Vector MTV { get; set; }


		public void Offset(Vector v)
		{
			Offset(v.X, v.Y);
		}


		public void Offset(float x, float y)
		{
			for (int i = 0; i < Vertices.Count; i++)
			{
				Vector p = Vertices[i];
				Vertices[i] = new Vector(p.X + x, p.Y + y);
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


		public void RotateAt(float angle, Vector at)
		{
			Matrix myMatrix = new Matrix();
			myMatrix.RotateAt(angle, at);
			//PointF[] toRotate = PointFs;
			PointF[] p = PointFs;
			myMatrix.TransformPoints(p);
			PointFs = p;
		}


		public override string ToString()
		{
			string result = "";

			for (int i = 0; i < Vertices.Count; i++)
			{
				if (result != "") result += " ";
				result += "{" + Vertices[i].ToString(true) + "}";
			}
			return result;
		}

		[Obsolete]
		public GraphicsPath toGraphicsPath()
		{
			List<PointF> p = Vertices.ConvertAll(new Converter<Vector, PointF>(Vector.asPointF));

			List<byte> pp = new List<byte>() { (byte)PathPointType.Start };
			for (int i = 0; i < p.Count - 2; i++)
			{
				pp.Add((byte)PathPointType.Line);
			}
			pp.Add((byte)PathPointType.CloseSubpath);

			return new GraphicsPath(p.ToArray(), pp.ToArray());
		}

		// POLYGON/POINT
		// only needed if you're going to check if the circle
		// is INSIDE the polygon
		public bool Collides(Vector p)
		{
			bool collision = false;

			// go through each of the vertices, plus the next
			// vertex in the list
			int next ;
			for (int current = 0; current < Vertices.Count; current++)
			{

				// get next vertex in list
				// if we've hit the end, wrap around to 0
				next = current + 1;
				if (next == Vertices.Count) next = 0;

				// get the PVectors at our current position
				// this makes our if statement a little cleaner
				Vector vc = Vertices[current];    // c for "current"
				Vector vn = Vertices[next];       // n for "next"

				// compare position, flip 'collision' variable
				// back and forth
				if (((vc.Y > p.Y && vn.Y < p.Y) || (vc.Y < p.Y && vn.Y > p.Y)) &&
					 (p.X < (vn.X - vc.X) * (p.Y - vc.Y) / (vn.Y - vc.Y) + vc.X))
				{
					collision = !collision;
				}
			}
			return collision;
		}

		// Check if polygon A is going to collide with polygon B for the given velocity
		public PolygonCollisionResult Collides(Polygon Other, Vector velocity)
		{
            PolygonCollisionResult result = new PolygonCollisionResult
            {
                Intersect = true,
                WillIntersect = true
            };

            int edgeCountA = Vertices.Count;  //because the num of edges and vertices are the same
			int edgeCountB = Other.Vertices.Count;
			float minIntervalDistance = float.PositiveInfinity;
			Vector translationAxis = new Vector();
			Vector edge;

			// Loop through all the edges of both polygons
			for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
			{
				if (edgeIndex < edgeCountA)
				{
					edge = Edge(edgeIndex);
				}
				else
				{
					edge = Other.Edge(edgeIndex - edgeCountA);
				}

				// ===== 1. Find if the polygons are currently intersecting =====

				// Find the axis perpendicular to the current edge
				Vector axis = new Vector(-edge.Y, edge.X);
				axis.Normalize();

				// Find the projection of the polygon on the current axis
				float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
				ProjectPolygon(axis, this, ref minA, ref maxA);
				ProjectPolygon(axis, Other, ref minB, ref maxB);

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

					Vector d = Center - Other.Center;
					if (d.Dot(translationAxis) < 0) translationAxis = -translationAxis;
					MTV = translationAxis;
					result.translationAxis = translationAxis;
				}
			}

			// The minimum translation vector can be used to push the polygons appart.
			// First moves the polygons by their velocity
			// then move polygonA by MinimumTranslationVector.
			if (result.WillIntersect) result.MinimumTranslationVector = translationAxis * minIntervalDistance;

			return result;
		}

		public void Draw(Graphics g, Brush color)
		{
			g.FillPolygon(color, PointFs);
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
			float d = axis.Dot(polygon.Vertices[0]);
			min = d;
			max = d;
			for (int i = 0; i < polygon.Vertices.Count; i++)
			{
				d = polygon.Vertices[i].Dot(axis);
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
			   		 	  	  	   	
		/// <summary>
		/// Whether this polygon collides with circle
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public bool Collides(Circle c)
		{

			// go through each of the Vertices, plus
			// the next vertex in the list
			int next ;
			for (int current = 0; current < Vertices.Count; current++)
			{

				// get next vertex in list
				// if we've hit the end, wrap around to 0
				next = current + 1;
				if (next == Vertices.Count) next = 0;

				// get the Vectors at our current position
				// this makes our if statement a little cleaner
				Vector vc = Vertices[current];    // c for "current"
				Vector vn = Vertices[next];       // n for "next"

				// check for collision between the circle and
				// a line formed between the two Vertices
				bool collision = c.Collides(vc, vn);
				if (collision) return true;
			}

			// the above algorithm only checks if the circle
			// is touching the edges of the polygon – in most
			// cases this is enough, but you can un-comment the
			// following code to also test if the center of the
			// circle is inside the polygon

			// bool centerInside = polygonPoint(Vertices, cx,cy);
			// if (centerInside) return true;

			// otherwise, after all that, return false
			return false;
		}
	}
}

