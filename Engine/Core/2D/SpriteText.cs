using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics;
using Engine.Graphics._2D;
using Engine.Utility;
using GlmSharp;

namespace Engine.Core._2D
{
	public class SpriteText : Component2D
	{
		private SpriteFont font;

		private string value;

		public SpriteText(string font, string value = null, Alignments alignment = Alignments.Left | Alignments.Top) :
			this(ContentCache.GetFont(font), value, alignment)
		{
		}

		public SpriteText(SpriteFont font, string value = null,
			Alignments alignment = Alignments.Left | Alignments.Top) : base(alignment)
		{
			this.font = font;
			
			Value = value;
		}

		public string Value
		{
			get => value;
			set
			{
				this.value = value;

				if (!string.IsNullOrEmpty(value))
				{
					ivec2 offset = ivec2.Zero;
					ivec2 dimensions = UseLiteralMeasuring
						? font.MeasureLiteral(value, out offset)
						: font.Measure(value);

					origin = Utilities.ComputeOrigin(dimensions.x, dimensions.y, alignment) - offset;

					// Each character from the string is rendered as a quad.
					data = new float[value.Length * QuadSize];
					positionChanged = true;
					sourceChanged = true;
					colorChanged = true;
				}
				else
				{
					data = null;
				}
			}
		}

		public bool UseLiteralMeasuring { get; set; }

		protected override void RecomputePositionData()
		{
			if (value == null)
			{
				return;
			}

			int index = 0;
			var glyphs = font.Glyphs;

			vec2 localPosition = position - origin;

			foreach (char c in value)
			{
				Glyph glyph = glyphs[c];

				// Spaces advance the next character position, but aren't rendered.
				if (c != ' ')
				{
					ivec2 offset = glyph.Offset;
					vec2 p = localPosition + offset;

					float left = p.x;
					float right = p.x + glyph.Width;
					float top = p.y;
					float bottom = p.y + glyph.Height;

					vec2[] array =
					{
						new vec2(left, top),
						new vec2(left, bottom),
						new vec2(right, top), 
						new vec2(right, bottom) 
					};

					for (int i = 0; i < 4; i++)
					{
						vec2 point = array[i];

						data[index] = point.x;
						data[index + 1] = point.y;

						index += VertexSize;
					}
				}

				localPosition.x += glyph.Advance;
			}
		}

		protected override void RecomputeSourceData()
		{
			if (value == null)
			{
				return;
			}

			int index = 2;
			var glyphs = font.Glyphs;

			foreach (char c in value)
			{
				if (c == ' ')
				{
					continue;
				}

				Glyph glyph = glyphs[c];

				foreach (vec2 p in glyph.Source)
				{
					data[index] = p.x;
					data[index + 1] = p.y;

					index += VertexSize;
				}
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			if (value == null)
			{
				return;
			}

			Draw(sb, font.TextureId, data);
		}
	}
}
