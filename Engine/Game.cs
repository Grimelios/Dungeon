using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input;
using GlmSharp;
using static Engine.GL;
using static Engine.GLFW;

namespace Engine
{
	public abstract class Game
	{
		// The input processor (and associated callbacks) are static to avoid the garbage collector moving things
		// around at runtime (which would cause an error in GLFW).
		private static InputProcessor inputProcessor;
		private static GLFWkeyfun keyCallback;
		private static GLFWcursorposfun cursorPositionCallback;
		private static GLFWmousebuttonfun mouseButtonCallback;

		static Game()
		{
			keyCallback = KeyCallback;
			cursorPositionCallback = CursorPositionCallback;
			mouseButtonCallback = MouseButtonCallback;
		}

		private static void KeyCallback(IntPtr window, int key, int scancode, int action, int mods)
		{
			// Key can be negative in some cases (like alt + printscreen).
			if (key == -1)
			{
				return;
			}

			switch (action)
			{
				case GLFW_PRESS: inputProcessor.OnKeyPress(key, mods); break;
				case GLFW_RELEASE: inputProcessor.OnKeyRelease(key); break;
			}
		}

		private static void CursorPositionCallback(IntPtr window, double x, double y)
		{
			int mX = (int)Math.Round(x);
			int mY = (int)Math.Round(y);

			inputProcessor.OnMouseMove(mX, mY);
		}

		private static void MouseButtonCallback(IntPtr window, int button, int action, int mods)
		{
			switch (action)
			{
				case GLFW_PRESS: inputProcessor.OnMouseButtonPress(button); break;
				case GLFW_RELEASE: inputProcessor.OnMouseButtonRelease(button); break;
			}
		}

		private Window window;

		private float previousTime;
		private float accumulator;

		protected Game(string title)
		{
			glfwInit();
			glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
			glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 4);
			glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

			IntPtr address = glfwCreateWindow(Resolution.WindowWidth, Resolution.WindowHeight, title, IntPtr.Zero,
				IntPtr.Zero);

			if (address == IntPtr.Zero)
			{
				glfwTerminate();

				return;
			}

			window = new Window(title, 800, 600, address);
			inputProcessor = new InputProcessor();

			glfwMakeContextCurrent(address);
			glfwSetKeyCallback(address, keyCallback);
			glfwSetCursorPosCallback(address, cursorPositionCallback);
			glfwSetMouseButtonCallback(address, mouseButtonCallback);

			glClearColor(0, 0, 0, 1);
			glEnable(GL_BLEND);
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
			glPrimitiveRestartIndex(Constants.RestartIndex);
		}

		public void Run()
		{
			const float Target = 1.0f / 60;

			while (glfwWindowShouldClose(window.Address) == 0)
			{
				float time = (float)glfwGetTime();

				accumulator += time - previousTime;
				previousTime = time;

				bool shouldUpdate = accumulator >= Target;

				if (!shouldUpdate)
				{
					continue;
				}

				inputProcessor.Update(Target);

				while (accumulator >= Target)
				{
					glfwPollEvents();
					Update(Target);
					accumulator -= Target;
				}

				Draw();

				glfwSwapBuffers(window.Address);
			}

			glfwTerminate();
		}

		protected abstract void Update(float dt);
		protected abstract void Draw();
	}
}
