using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Input.Data
{
	[Flags]
	public enum InputStates
	{
		// These values are set such that the "this frame" entries contain held and released (in terms of bits), but
		// not vice-versa. In other words, pressing/releasing on a frame also counts as being held/released on that
		// frame, but not the other way around.
		Held = 1,
		Released = 4,
		PressedThisFrame = 3,
		ReleasedThisFrame = 12
	}
}
