using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input.Data;
using static Engine.GLFW;

namespace Dungeon.Entities
{
	public class PlayerControls
	{
		public PlayerControls()
		{
			RunLeft = new List<InputBind>
			{
				new InputBind(InputTypes.Keyboard, GLFW_KEY_A),
				new InputBind(InputTypes.Keyboard, GLFW_KEY_LEFT)
			};

			RunRight = new List<InputBind>
			{
				new InputBind(InputTypes.Keyboard, GLFW_KEY_D),
				new InputBind(InputTypes.Keyboard, GLFW_KEY_RIGHT)
			};

			Crouch = new List<InputBind>
			{
				new InputBind(InputTypes.Keyboard, GLFW_KEY_S),
				new InputBind(InputTypes.Keyboard, GLFW_KEY_DOWN)
			};

			Jump = new List<InputBind>
			{
				new InputBind(InputTypes.Keyboard, GLFW_KEY_SPACE)
			};
		}

		public List<InputBind> RunLeft { get; }
		public List<InputBind> RunRight { get; }
		public List<InputBind> Crouch { get; }
		public List<InputBind> Jump { get; }
		public List<InputBind> Ascend { get; }
		public List<InputBind> Attack { get; }
		public List<InputBind> Interact { get; }
	}
}
