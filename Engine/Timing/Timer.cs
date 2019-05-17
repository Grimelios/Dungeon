using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;

namespace Engine.Timing
{
	public abstract class Timer : DynamicComponent
	{
		protected Timer(float duration, float elapsed = 0)
		{
			Elapsed = elapsed;
			Duration = duration;
		}

		public float Elapsed { get; set; }
		public float Duration { get; set; }

		public bool Paused { get; set; }

		public Action<float> Tick { get; set; }
	}
}
