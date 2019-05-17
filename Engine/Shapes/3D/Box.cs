using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Shapes._3D
{
	public class Box : Shape3D
	{
		public Box() : this(0, 0, 0)
		{
		}

		// This constructor creates a cube.
		public Box(float size) : this(size, size, size)
		{
		}

		public Box(float width, float height, float depth) : base(ShapeTypes3D.Cube)
		{
			Width = width;
			Height = height;
			Depth = depth;
		}
		
		public float Width { get; set; }
		public float Height { get; set; }
		public float Depth { get; set; }
	}
}
