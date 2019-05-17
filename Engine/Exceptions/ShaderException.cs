using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Shaders;

namespace Engine.Exceptions
{
	public class ShaderException : Exception
	{
		public ShaderException(ShaderStages stage, string message) : base($"[{stage}] {message}")
		{
		}
	}
}
