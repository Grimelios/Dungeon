using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.GLFW;

namespace Engine
{
	public class Window
	{
		public Window(string title, int width, int height, IntPtr address)
		{
			Width = width;
			Height = height;
			Address = address;
		}

		public int Width { get; }
		public int Height { get; }

		public IntPtr Address { get; }
	}
}
