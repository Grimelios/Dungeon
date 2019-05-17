using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core._2D;

namespace Engine.Interfaces._2D
{
	public interface IBoundable2D : ILocatable
	{
		Bounds2D Bounds { get; }
	}
}
