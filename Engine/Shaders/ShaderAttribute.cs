using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.GL;

namespace Engine.Shaders
{
	public class ShaderAttribute
	{
		public ShaderAttribute(uint count, uint type, uint offset, bool isInteger, bool isNormalized)
		{
			Count = count;
			Type = type;
			Offset = offset;
			IsInteger = isInteger;
			IsNormalized = isNormalized;
		}

		public uint Count { get; }
		public uint Type { get; }
		public uint Offset { get; }

		public bool IsInteger { get; }
		public bool IsNormalized { get; }
	}
}
