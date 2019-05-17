using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utility;
using GlmSharp;

namespace Engine.Shapes._2D
{
	public class Circle : Shape2D
	{
		public Circle() : this(0)
		{
		}

		public Circle(float radius) : base(ShapeTypes2D.Circle)
		{
			Radius = radius;
		}

		public float Radius { get; set; }

		public override bool Contains(vec2 p)
		{
			float squared = Utilities.DistanceSquared(p, Position);

			return squared <= Radius * Radius;
		}
	}
}
