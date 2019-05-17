using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Engine.Interfaces._2D
{
	public interface ILocatable
	{
		ivec2 Location { get; set; }
	}
}
