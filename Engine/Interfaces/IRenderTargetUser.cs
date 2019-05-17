using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core._2D;

namespace Engine.Interfaces
{
	public interface IRenderTargetUser : IDisposable
	{
		void DrawTargets();
	}
}
