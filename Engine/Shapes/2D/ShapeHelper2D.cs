using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utility;
using GlmSharp;

namespace Engine.Shapes._2D
{
	public static class ShapeHelper2D
	{
		public static bool CheckOverlap(Shape2D shape1, Shape2D shape2)
		{
			// By ensuring that the second shape sorts after the first (as far as shape type), the total number of
			// switch cases can be lowered.
			if ((int)shape1.ShapeType > (int)shape2.ShapeType)
			{
				Shape2D temp = shape1;
				shape1 = shape2;
				shape2 = temp;
			}

			switch (shape1.ShapeType)
			{
				case ShapeTypes2D.Arc: return CheckOverlap((Arc)shape1, shape2);
				case ShapeTypes2D.Circle: return CheckOverlap((Circle)shape1, shape2);
				case ShapeTypes2D.Rectangle: return CheckOverlap((Rectangle)shape1, (Rectangle)shape2);
			}

			return false;
		}

		private static bool CheckOverlap(Arc arc, Shape2D other)
		{
			switch (other.ShapeType)
			{
				case ShapeTypes2D.Arc: return CheckOverlap(arc, (Arc)other);
				case ShapeTypes2D.Circle: return CheckOverlap(arc, (Circle)other);
				case ShapeTypes2D.Rectangle: return CheckOverlap(arc, (Rectangle)other);
			}

			return false;
		}

		private static bool CheckOverlap(Circle circle, Shape2D other)
		{
			switch (other.ShapeType)
			{
				case ShapeTypes2D.Circle: return CheckOverlap(circle, (Circle)other);
				case ShapeTypes2D.Rectangle: return CheckOverlap(circle, (Rectangle)other);
			}

			return false;
		}

		private static bool CheckOverlap(Arc arc1, Arc arc2)
		{
			return false;
		}

		private static bool CheckOverlap(Arc arc, Circle circle)
		{
			vec2 p1 = arc.Position;
			vec2 p2 = circle.Position;
			
			float sumRadius = arc.Radius + circle.Radius;
			float distanceSquared = Utilities.DistanceSquared(p1, p2);

			if (distanceSquared > sumRadius * sumRadius)
			{
				return false;
			}

			float angle = Utilities.Angle(p1, p2);
			float delta = Utilities.CorrectAngle(Math.Abs(angle - arc.Angle));
			float halfSpread = arc.Spread / 2;

			if (delta <= halfSpread)
			{
				return true;
			}

			vec2[] points = new vec2[2];

			for (int i = 0; i < 2; i++)
			{
				float a = arc.Angle + halfSpread * (i == 0 ? -1 : 1);

				points[i] = p1 + Utilities.Direction(a) * arc.Radius;
			}

			if (points.Any(circle.Contains))
			{
				return true;
			}

			float radiusSquared = circle.Radius * circle.Radius;

			return points.Any(p => Utilities.DistanceSquaredToLine(p2, p1, p) <= radiusSquared);
		}

		private static bool CheckOverlap(Arc arc, Rectangle rect)
		{
			return false;
		}

		private static bool CheckOverlap(Circle circle1, Circle circle2)
		{
			float sumRadius = circle1.Radius + circle2.Radius;
			float distanceSquared = Utilities.DistanceSquared(circle1.Position, circle2.Position);

			return distanceSquared <= sumRadius * sumRadius;
		}

		private static bool CheckOverlap(Circle circle, Rectangle rect)
		{
			return false;
		}

		private static bool CheckOverlap(Rectangle rect1, Rectangle rect2)
		{
			return false;
		}
	}
}
