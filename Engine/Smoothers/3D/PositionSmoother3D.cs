using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces._3D;
using Engine.Utility;
using GlmSharp;

namespace Engine.Smoothers._3D
{
	public class PositionSmoother3D : Smoother<vec3>
	{
		private IPositionable3D target;

		public PositionSmoother3D(IPositionable3D target, vec3 start, vec3 end, float duration, EaseTypes easeType) :
			base(start, end, duration, easeType)
		{
			this.target = target;
		}

		protected override void Smooth(float t)
		{
			target.Position = vec3.Lerp(Start, End, t);
		}
	}
}
