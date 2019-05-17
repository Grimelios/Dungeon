using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics;
using Engine.Graphics._2D;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using Engine.Utility;
using GlmSharp;

namespace Engine.Core._2D
{
	public class Sprite : Component2D
	{
		private QuadSource source;
		private Bounds2D sourceRect;

		public Sprite(string filename, Bounds2D sourceRect = null, Alignments alignment = Alignments.Center) :
			this (ContentCache.GetTexture(filename), sourceRect, alignment)
		{
		}

		public Sprite(QuadSource source, Bounds2D sourceRect = null, Alignments alignment = Alignments.Center) :
			base(alignment)
		{
			this.source = source;
			this.sourceRect = sourceRect;

			data = new float[QuadSize];
		}

		public Bounds2D SourceRect
		{
			get => sourceRect;
			set
			{
				sourceRect = value;
				sourceChanged = true;
			}
		}

		public void ScaleTo(int width, int height)
		{
			float w;
			float h;

			if (sourceRect != null)
			{
				w = sourceRect.Width;
				h = sourceRect.Height;
			}
			else
			{
				w = source.Width;
				h = source.Height;
			}

			Scale = new vec2(width / w, height / h);
		}

		protected override void RecomputePositionData()
		{
			int width = source.Width;
			int height = source.Height;

			vec2[] points =
			{
				new vec2(0, 0),
				new vec2(0, height),
				new vec2(width, 0),
				new vec2(width, height)
			};

			for (int i = 0; i < 4; i++)
			{
				vec2 p = points[i];
				p -= origin;
				p *= scale;
				points[i] = p;
			}

			if (rotation != 0)
			{
				mat2 matrix = Utilities.RotationMatrix2D(rotation);

				for (int i = 0; i < 4; i++)
				{
					vec2 p = points[i];
					p = matrix * p;
					points[i] = p;
				}
			}

			int[] order = Enumerable.Range(0, 4).ToArray();

			bool flipHorizontal = (mods & SpriteModifiers.FlipHorizontal) > 0;
			bool flipVertical = (mods & SpriteModifiers.FlipVertical) > 0;

			if (flipHorizontal)
			{
				order[0] = 2;
				order[1] = 3;
				order[2] = 0;
				order[3] = 1;
			}

			if (flipVertical)
			{
				int temp1 = order[0];
				int temp2 = order[2];

				order[0] = order[1];
				order[1] = temp1;
				order[2] = order[3];
				order[3] = temp2;
			}

			for (int i = 0; i < 4; i++)
			{
				int start = i * VertexSize;

				vec2 p = points[order[i]] + position;

				data[start] = p.x;
				data[start + 1] = p.y;
			}
		}

		protected override void RecomputeSourceData()
		{
			vec2[] coords = new vec2[4];

			if (sourceRect != null)
			{
				coords[0] = new vec2(sourceRect.Left, sourceRect.Top);
				coords[1] = new vec2(sourceRect.Left, sourceRect.Bottom);
				coords[2] = new vec2(sourceRect.Right, sourceRect.Top);
				coords[3] = new vec2(sourceRect.Right, sourceRect.Bottom);

				vec2 dimensions = new vec2(source.Width, source.Height);

				for (int i = 0; i < 4; i++)
				{
					coords[i] /= dimensions;
				}
			}
			else
			{
				coords[0] = vec2.Zero;
				coords[1] = vec2.UnitY;
				coords[2] = vec2.UnitX;
				coords[3] = vec2.Ones;
			}

			for (int i = 0; i < 4; i++)
			{
				vec2 value = coords[i];

				int start = i * VertexSize + 2;

				data[start] = value.x;
				data[start + 1] = value.y;
			}

			RecomputeOrigin();
		}

		private void RecomputeOrigin()
		{
			int width;
			int height;

			if (sourceRect == null)
			{
				width = source.Width;
				height = source.Height;
			}
			else
			{
				width = sourceRect.Width;
				height = sourceRect.Height;
			}

			origin = Utilities.ComputeOrigin(width, height, alignment);
		}

		public override void Draw(SpriteBatch sb)
		{
			Draw(sb, source.Id, data);
		}	
	}
}
