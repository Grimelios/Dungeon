using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces._3D;
using GlmSharp;

namespace Engine.Shapes._3D
{
	public abstract class Shape3D : ITransformable3D
	{
		protected Shape3D(ShapeTypes3D shapeType)
		{
			ShapeType = shapeType;
			Orientation = quat.Identity;
		}

		public ShapeTypes3D ShapeType { get; }

		public vec3 Position { get; set; }
		public quat Orientation { get; set; }

		public void SetTransform(vec3 position, quat orientation)
		{
			Position = position;
			Orientation = orientation;
		}
	}
}
