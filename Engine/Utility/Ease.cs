using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utility
{
	public enum EaseTypes
	{
		Linear,
		QuadraticIn,
		QuadraticOut,
		CubicIn,
		CubicOut
	}

	public static class Ease
	{
		public static float Compute(float t, EaseTypes easeType)
		{
			switch (easeType)
			{
				case EaseTypes.QuadraticIn: return QuadraticIn(t);
				case EaseTypes.QuadraticOut: return QuadraticOut(t);
				case EaseTypes.CubicIn: return CubicIn(t);
				case EaseTypes.CubicOut: return CubicOut(t);
			}

			// This is equivalent to EaseTypes.Linear.
			return t;
		}

		private static float QuadraticIn(float t)
		{
			return t * t;
		}

		private static float QuadraticOut(float t)
		{
			return -(t * (t - 2));
		}

		private static float CubicIn(float t)
		{
			return t * t * t;
		}

		private static float CubicOut(float t)
		{
			float f = t - 1;

			return f * f * f + 1;
		}
	}
}
