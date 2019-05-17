using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Input.Data
{
	public abstract class InputData
	{
		protected InputData(InputTypes inputType)
		{
			InputType = inputType;
		}

		public InputTypes InputType { get; }

		// This function is useful for something like a "Press any button to continue" screen.
		public abstract bool AnyPressed();
		public abstract bool Query(int data, InputStates state);
	}
}
