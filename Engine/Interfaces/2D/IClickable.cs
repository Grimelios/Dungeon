using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Engine.Interfaces._2D
{
	public interface IClickable : ILocatable, IBoundable2D
	{
		void OnHover(ivec2 mouseLocation);
		void OnUnhover();
		void OnClick(ivec2 mouseLocation);

		bool Contains(ivec2 mouseLocation);
	}
}
