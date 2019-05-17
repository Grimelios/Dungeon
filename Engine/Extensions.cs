using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Engine
{
	public static class Extensions
	{
		public static int ToRgba(this System.Drawing.Color color)
		{
			// The Color class already has a function called "ToARGB", but that's the wrong ordering for what the
			// engine's GL code expects.
			byte[] bytes =
			{
				color.R,
				color.G,
				color.B,
				color.A
			};

			return BitConverter.ToInt32(bytes, 0);
		}

		public static vec4 ToVec4(this quat quat)
		{
			float[] values = quat.Values;

			return new vec4(values[0], values[1], values[2], values[3]);
		}
	}
}
