using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
	public class RenderTargetException : Exception
	{
		public RenderTargetException(string message) : base(message)
		{
		}
	}
}
