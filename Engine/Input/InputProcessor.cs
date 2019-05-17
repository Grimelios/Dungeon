using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input.Data;
using Engine.Interfaces;
using Engine.Messaging;
using GlmSharp;
using static Engine.GLFW;

namespace Engine.Input
{
	public class InputProcessor : IDynamic
	{
		private InputStates[] buttons;
		private InputStates[] keys;

		private List<KeyPress> keyPresses;
		private ivec2 mouseLocation;
		private ivec2 previousMouseLocation;

		// On the first frame, the mouse's previous location is artificially set to the current location in order to
		// avoid a false, large delta.
		private bool firstFrame;

		public InputProcessor()
		{
			buttons = Enumerable.Repeat(InputStates.Released, GLFW_MOUSE_BUTTON_LAST).ToArray();
			keys = Enumerable.Repeat(InputStates.Released, GLFW_KEY_LAST).ToArray();
			keyPresses = new List<KeyPress>();
			firstFrame = true;
		}

		public void KeyCallback(IntPtr window, int key, int scancode, int action, int mods)
		{
			if (key == -1)
			{
				return;
			}

			if (action == GLFW_PRESS)
			{
				keys[key] = InputStates.PressedThisFrame;
				keyPresses.Add(new KeyPress(key, (KeyModifiers)mods));
			}
		}

		public void OnKeyPress(int key, int mods)
		{
			keys[key] = InputStates.PressedThisFrame;
			keyPresses.Add(new KeyPress(key, (KeyModifiers)mods));
		}

		public void OnKeyRelease(int key)
		{
			keys[key] = InputStates.ReleasedThisFrame;
		}

		public void OnMouseButtonPress(int button)
		{
			buttons[button] = InputStates.PressedThisFrame;
		}

		public void OnMouseButtonRelease(int button)
		{
			buttons[button] = InputStates.ReleasedThisFrame;
		}

		public void OnMouseMove(int x, int y)
		{
			mouseLocation.x = x;
			mouseLocation.y = y;
		}

		public void Update(float dt)
		{
			var mouseData = GetMouseData();
			var keyboardData = GetKeyboardData();

			FullInputData fullData = new FullInputData();
			fullData.Add(InputTypes.Mouse, mouseData);
			fullData.Add(InputTypes.Keyboard, keyboardData);

			MessageSystem.Send(CoreMessageTypes.Keyboard, keyboardData, dt);
			MessageSystem.Send(CoreMessageTypes.Mouse, mouseData, dt);
			MessageSystem.Send(CoreMessageTypes.Input, fullData, dt);
		}

		private KeyboardData GetKeyboardData()
		{
			KeyboardData data = new KeyboardData((InputStates[])keys.Clone(), keyPresses.ToArray());

			for (int i = 0; i < keys.Length; i++)
			{
				switch (keys[i])
				{
					case InputStates.PressedThisFrame: keys[i] = InputStates.Held; break;
					case InputStates.ReleasedThisFrame: keys[i] = InputStates.Released; break;
				}
			}

			keyPresses.Clear();

			return data;
		}

		private MouseData GetMouseData()
		{
			if (firstFrame)
			{
				previousMouseLocation = mouseLocation;
				firstFrame = false;
			}

			MouseData data = new MouseData(mouseLocation, previousMouseLocation, (InputStates[])buttons.Clone());

			previousMouseLocation = mouseLocation;

			for (int i = 0; i < buttons.Length; i++)
			{
				switch (buttons[i])
				{
					case InputStates.PressedThisFrame: buttons[i] = InputStates.Held; break;
					case InputStates.ReleasedThisFrame: buttons[i] = InputStates.Released; break;
				}
			}

			return data;
		}
	}
}
