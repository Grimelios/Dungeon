using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Input.Data
{
	public class KeyboardData : InputData
	{
		private InputStates[] keys;

		public KeyboardData(InputStates[] keys, KeyPress[] keysPressedThisFrame) : base(InputTypes.Keyboard)
		{
			this.keys = keys;

			KeysPressedThisFrame = keysPressedThisFrame;
		}

		public KeyPress[] KeysPressedThisFrame { get; }

		public override bool AnyPressed()
		{
			return keys.Any(k => k == InputStates.PressedThisFrame);
		}

		public override bool Query(int data, InputStates state)
		{
			return (keys[data] & state) == state;
		}
	}
}
