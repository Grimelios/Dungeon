using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Engine.Utility;

namespace Engine.Smoothers
{
	public abstract class Smoother<T> : DynamicComponent
	{
		private float elapsed;
		private float duration;

		private EaseTypes easeType;

		protected Smoother(T start, T end, float duration, EaseTypes easeType)
		{
			this.duration = duration;
			this.easeType = easeType;

			Start = start;
			End = end;
		}

		protected T Start { get; }
		protected T End { get; }

		public override void Update(float dt)
		{
			elapsed += dt;

			float t;

			if (elapsed >= duration)
			{
				// This ignores any leftover time (such that the end state is set exactly).
				t = 1;
				Complete = true;
			}
			else
			{
				t = elapsed / duration;
			}

			Smooth(Ease.Compute(t, easeType));
		}

		protected abstract void Smooth(float t);
	}
}
