using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Engine.Core._2D;
using Engine.Graphics._2D;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using GlmSharp;

namespace Engine.UI
{
	public abstract class CanvasElement : IBoundable2D, IDynamic, IRenderable2D
	{
		private ivec2 location;

		protected CanvasElement()
		{
			Visible = true;
		}

		protected bool Centered { get; set; }

		public virtual ivec2 Location
		{
			get => location;
			set
			{
				location = value;

				if (Bounds == null)
				{
					return;
				}

				if (Centered)
				{
					Bounds.Center = value;
				}
				else
				{
					Bounds.Location = value;
				}
			}
		}

		public bool Visible { get; set; }

		public Alignments Anchor { get; set; }

		public ivec2 Offset { get; set; }
		public Bounds2D Bounds { get; protected set; }

		public virtual void Update(float dt)
		{
		}

		public abstract void Draw(SpriteBatch sb);
	}
}
