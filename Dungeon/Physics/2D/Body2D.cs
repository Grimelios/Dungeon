using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Entities.Core;
using Engine.Interfaces._2D;
using Engine.Shapes._2D;
using GlmSharp;

namespace Dungeon.Physics._2D
{
	using CollisionCallback = Func<Manifold2D, bool>;

	public class Body2D : IPositionable2D, IRotatable
	{
		public Body2D(Shape2D shape, Entity owner)
		{
			Shape = shape;
			Tag = owner;
			IsAffectedByGravity = true;
		}

		public vec2 Position
		{
			get => Shape.Position;
			set => Shape.Position = value;
		}

		public vec2 Velocity { get; set; }
		public Shape2D Shape { get; }

		public float Rotation { get; set; }

		public bool IsStatic { get; set; }
		public bool IsAffectedByGravity { get; set; }

		public object Tag { get; set; }

		public CollisionCallback OnCollision { get; set; }
	}
}
