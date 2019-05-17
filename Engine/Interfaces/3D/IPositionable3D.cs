using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Engine.Interfaces._3D
{
	public interface IPositionable3D
	{
		vec3 Position { get; set; }
	}
}
