using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utility;
using GlmSharp;

namespace Engine.Core
{
	public class Color
	{
		public static readonly Color Black = new Color(0);
		public static readonly Color White = new Color(255);
		public static readonly Color Red = new Color(255, 0, 0);
		public static readonly Color Green = new Color(0, 255, 0);
		public static readonly Color Blue = new Color(0, 0, 255);
		public static readonly Color Yellow = new Color(255, 255, 0);
		public static readonly Color Cyan = new Color(0, 255, 255);
		public static readonly Color Magenta = new Color(255, 0, 255);

		public static Color Lerp(Color start, Color end, float t)
		{
			byte r = Utilities.Lerp(start.r, end.r, t);
			byte g = Utilities.Lerp(start.g, end.g, t);
			byte b = Utilities.Lerp(start.b, end.b, t);
			byte a = Utilities.Lerp(start.a, end.a, t);

			return new Color(r, g, b, a);
		}

		private byte r;
		private byte g;
		private byte b;
		private byte a;

		public Color(byte value) : this(value, value, value, 255)
		{
		}

		public Color(byte r, byte g, byte b) : this(r, g, b, 255)
		{
		}

		public Color(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public float ToFloat()
		{
			return BitConverter.ToSingle(new [] { r, g, b, a }, 0);
		}

		public vec3 ToVec3()
		{
			return new vec3(r, g, b) / 255;
		}

		public vec4 ToVec4()
		{
			return new vec4(r, g, b, a) / 255;
		}
	}
}
