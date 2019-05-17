using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Input.Data
{
	public class KeyPress
	{
		public KeyPress(int key, KeyModifiers mods)
		{
			Key = key;
			Mods = mods;
		}

		public int Key { get; }

		public KeyModifiers Mods { get; }
	}
}
