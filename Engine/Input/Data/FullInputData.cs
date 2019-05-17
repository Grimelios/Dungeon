using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utility;

namespace Engine.Input.Data
{
	public class FullInputData
	{
		private InputData[] dataArray;

		public FullInputData()
		{
			dataArray = new InputData[Utilities.EnumCount<InputTypes>()];
		}

		public InputData GetData(InputTypes inputType)
		{
			return dataArray[(int)inputType];
		}

		public void Add(InputTypes inputType, InputData data)
		{
			dataArray[(int)inputType] = data;
		}

		public bool Query(List<InputBind> binds, InputStates state)
		{
			return binds.Any(b => dataArray[(int)b.InputType].Query(b.Data, state));
		}

		public bool Query(List<InputBind> binds, InputStates state, out InputBind bindUsed)
		{
			foreach (InputBind bind in binds)
			{
				if (dataArray[(int)bind.InputType].Query(bind.Data, state))
				{
					bindUsed = bind;

					return true;
				}
			}

			bindUsed = null;

			return false;
		}
	}
}
