using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utility;
using GlmSharp;

namespace Engine.Shapes._2D
{
	public class Rectangle : Shape2D
	{
		public Rectangle() : this(0, 0, 0, 0)
		{
		}

		public Rectangle(float size) : this(0, 0, size, size)
		{
		}

		public Rectangle(float width, float height) : this(0, 0, width, height)
		{
		}

		public Rectangle(float x, float y, float size) : this(x, y, size, size)
		{
		}

		public Rectangle(float x, float y, float width, float height) : base(ShapeTypes2D.Rectangle)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public float X
		{
			get => Left;
			set => Left = value;
		}

		public float Y
		{
			get => Top;
			set => Top = value;
		}

		public float Width { get; set; }
		public float Height { get; set; }

		public float Left
		{
			get => position.x - Width / 2;
			set => position.x = value + Width / 2;
		}

		public float Right
		{
			get => position.x + Width / 2;
			set => position.x = value - Width / 2;
		}

		public float Top
		{
			get => position.y - Height / 2;
			set => position.y = value + Height / 2;
		}

		public float Bottom
		{
			get => position.y + Height / 2;
			set => position.y = value - Height / 2;
		}

		public vec2[] Corners => new []
		{
			new vec2(X, Y),
			new vec2(Right, Y),
			new vec2(Right, Bottom),
			new vec2(X, Bottom)
		};

		public override bool Contains(vec2 p)
		{
			if (Rotation != 0)
			{
				p = Utilities.Rotate(p, -Rotation);
			}

			return p.x >= Left && p.x <= Right && p.y >= Top && p.y <= Bottom;
		}
	}
}
