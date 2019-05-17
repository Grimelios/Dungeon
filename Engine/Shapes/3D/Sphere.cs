using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Shapes._3D
{
	public class Sphere : Shape3D
	{
		public Sphere() : this(0)
		{
		}

		public Sphere(float radius) : base(ShapeTypes3D.Sphere)
		{
			Radius = radius;
		}

		public float Radius { get; set; }
	}
}
