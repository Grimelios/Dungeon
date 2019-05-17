using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces;
using Engine.Interfaces._3D;
using GlmSharp;

namespace Dungeon.Entities.Core
{
	public abstract class Entity : IPositionable3D, IDynamic
	{
		protected Scene Scene { get; private set; }

		public vec3 Position { get; set; }

		public virtual void Initialize(Scene scene)
		{
			Scene = scene;
		}

		public virtual void Update(float dt)
		{
		}
	}
}
