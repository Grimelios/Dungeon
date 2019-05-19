using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Engine.Shapes._2D
{
	public class Capsule : Shape2D
	{
		public Capsule() : this(0, 0)
		{
		}

		public Capsule(float height, float radius) : base(ShapeTypes2D.Capsule)
		{
			Height = height;
			Radius = radius;
		}

		public float Height { get; set; }
		public float Radius { get; set; }

		public override bool Contains(vec2 p)
		{
			return false;
		}
	}
}
