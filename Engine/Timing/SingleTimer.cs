using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Timing
{
	public class SingleTimer : Timer
	{
		// The argument is the leftover time (since the duration is unlikely to be hit exactly).
		private Action<float> trigger;

		public SingleTimer(Action<float> trigger, float duration = 0, float elapsed = 0) : base(duration, elapsed)
		{
			this.trigger = trigger;
		}

		// Even "single" timers can be repeated. The idea is that a repeating timer triggers an action multiple times
		// in a row, whereas a single timer executes only a single callback, but can then be reset and paused
		// automatically (in preparation for the next call).
		public bool Repeatable { get; set; }

		public override void Update(float dt)
		{
			if (Paused)
			{
				return;
			}

			Elapsed += dt;

			if (Elapsed >= Duration)
			{
				trigger(Elapsed - Duration);

				if (Repeatable)
				{
					Elapsed = 0;
					Paused = true;
				}
				else
				{
					Complete = true;
				}

				return;
			}

			Tick?.Invoke(Elapsed / Duration);
		}
	}
}
