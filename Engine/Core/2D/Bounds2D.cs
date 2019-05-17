using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces._2D;
using Engine.Shapes._2D;
using GlmSharp;

namespace Engine.Core._2D
{
	public class Bounds2D : ILocatable
	{
		public Bounds2D() : this(0, 0, 0, 0)
		{
		}

		public Bounds2D(int size) : this(0, 0, size, size)
		{
		}

		public Bounds2D(int width, int height) : this(0, 0, width, height)
		{
		}

		public Bounds2D(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public int Left
		{
			get => X;
			set => X = value;
		}

		public int Right
		{
			get => X + Width - 1;
			set => X = value - Width + 1;
		}

		public int Top
		{
			get => Y;
			set => Y = value;
		}

		public int Bottom
		{
			get => Y + Height - 1;
			set => Y = value - Height + 1;
		}

		public ivec2 Location
		{
			get => new ivec2(X, Y);
			set
			{
				X = value.x;
				Y = value.y;
			}
		}

		public ivec2 Center
		{
			get => new ivec2(X + Width / 2, Y + Height / 2);
			set
			{
				X = value.x - Width / 2;
				Y = value.y - Height / 2;
			}
		}

		public ivec2[] Corners => new[]
		{
			new ivec2(X, Y),
			new ivec2(Right, Y),
			new ivec2(Right, Bottom),
			new ivec2(X, Bottom)
		};

		public bool Contains(ivec2 point)
		{
			return Contains(point.x, point.y);
		}

		public bool Contains(vec2 point)
		{
			return Contains(point.x, point.y);
		}

		private bool Contains(float x, float y)
		{
			return x >= Left && x <= Right && y >= Top && y <= Bottom;
		}

		public Bounds2D Expand(int value)
		{
			return new Bounds2D(X - value, Y - value, Width + value * 2, Height + value * 2);
		}

		public Rectangle ToRectangle()
		{
			return new Rectangle(X, Y, Width, Height);
		}
	}
}
