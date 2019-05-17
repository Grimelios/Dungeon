using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using GlmSharp;

namespace Engine.Shapes._2D
{
	public abstract class Shape2D : IPositionable2D, IRotatable
	{
		protected vec2 position;

		protected Shape2D(ShapeTypes2D shapeType)
		{
			ShapeType = shapeType;
		}

		public ShapeTypes2D ShapeType { get; set; }

		// Using a protected local field (rather than an auto-property) allows easier modification by extending classes
		// (since you don't need to copy the vector first before modifying values).
		public vec2 Position
		{
			get => position;
			set => position = value;
		}

		public float Rotation { get; set; }

		public abstract bool Contains(vec2 p);

		public bool Overlaps(Shape2D other)
		{
			return ShapeHelper2D.CheckOverlap(this, other);
		}
	}
}
