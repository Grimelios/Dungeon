using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core._2D
{
	[Flags]
	public enum RenderTargetFlags
	{
		None = 0,
		Color = 1,
		Depth = 2,
		DepthStencil = Depth | Stencil,
		Stencil = 4,
	}
}
