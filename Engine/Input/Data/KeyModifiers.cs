using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.GLFW;

namespace Engine.Input.Data
{
	public enum KeyModifiers
	{
		Alt = GLFW_MOD_ALT,
		Control = GLFW_MOD_CONTROL,
		None = 0,
		Shift = GLFW_MOD_SHIFT
	}
}
