using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Entities.Core;
using GlmSharp;

namespace Dungeon.Physics._2D
{
	public class Manifold2D
	{
		public Manifold2D(object target, vec2 position, vec2 normal, Entity entity)
		{
			Target = target;
			Position = position;
			Normal = normal;
			Entity = entity;
		}

		public object Target { get; }

		public vec2 Position { get; }
		public vec2 Normal { get; }
		public Entity Entity { get; }
	}
}
