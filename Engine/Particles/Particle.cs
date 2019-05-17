using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces._3D;
using GlmSharp;

namespace Engine.Particles
{
	public class Particle : IPositionable3D
	{
		public vec3 Position { get; set; }
	}
}
