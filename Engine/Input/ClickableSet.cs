using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input.Data;
using Engine.Interfaces._2D;
using GlmSharp;
using static Engine.GLFW;

namespace Engine.Input
{
	public class ClickableSet
	{
		private IClickable hoveredItem;

		public ClickableSet()
		{
		}

		public ClickableSet(List<IClickable> items)
		{
			Items = items;
		}

		public List<IClickable> Items { get; }

		public void ProcessMouse(MouseData data)
		{
			ivec2 mouseLocation = data.Location;

			if (hoveredItem != null && !hoveredItem.Contains(mouseLocation))
			{
				hoveredItem.OnUnhover();
				hoveredItem = null;
			}

			foreach (IClickable item in Items)
			{
				if (item != hoveredItem && item.Contains(mouseLocation))
				{
					// It's possible for the mouse to move between items in a single frame.
					hoveredItem?.OnUnhover();
					hoveredItem = item;
					hoveredItem.OnHover(mouseLocation);

					break;
				}
			}

			// It's also possible for the mouse to move to a new item and click on the same frame (which is rare, but
			// considered a valid click).
			if (hoveredItem != null && data.Query(GLFW_MOUSE_BUTTON_LEFT, InputStates.PressedThisFrame))
			{
				hoveredItem.OnClick(mouseLocation);
			}
		}
	}
}
