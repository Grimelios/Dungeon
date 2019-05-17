using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Input.Data
{
	public class InputBind
	{
		public InputBind(InputTypes inputType, int data)
		{
			InputType = inputType;
			Data = data;
		}

		public InputTypes InputType { get; set; }

		public int Data { get; set; }
	}
}
