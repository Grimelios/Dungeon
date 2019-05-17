using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Engine.Input.Data
{
	public class MouseData : InputData
	{
		private InputStates[] buttons;

		public MouseData(ivec2 location, ivec2 previousLocation, InputStates[] buttons) : base(InputTypes.Mouse)
		{
			this.buttons = buttons;

			Location = location;
			PreviousLocation = previousLocation;
		}

		public ivec2 Location { get; }
		public ivec2 PreviousLocation { get; }

		public override bool AnyPressed()
		{
			// Mouse movement, holding down buttons, or using the scroll wheel don't count as an "any press" (for the
			// purposes of something like a "Press Start" screen).
			return buttons.Any(b => b == InputStates.PressedThisFrame);
		}

		public override bool Query(int data, InputStates state)
		{
			return (buttons[data] & state) == state;
		}
	}
}
