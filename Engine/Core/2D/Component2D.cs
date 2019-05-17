using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics;
using Engine.Graphics._2D;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using Engine.Shaders;
using GlmSharp;
using static Engine.GL;

namespace Engine.Core._2D
{
	public abstract class Component2D : IPositionable2D, IRotatable, IScalable2D, IColorable, IRenderable2D,
		IDisposable
	{
		// Each vertex contains position, texture coordinates, and color.
		protected const int VertexSize = 5;
		protected const int QuadSize = VertexSize * 4;

		protected vec2 position;
		protected vec2 scale;
		protected ivec2 origin;
		protected Color color;

		protected float rotation;
		protected float[] data;

		protected Alignments alignment;
		protected SpriteModifiers mods;

		protected bool positionChanged;
		protected bool sourceChanged;
		protected bool colorChanged;

		protected Component2D(Alignments alignment)
		{
			this.alignment = alignment;

			scale = vec2.Ones;
			color = Color.White;
			mods = SpriteModifiers.None;
			positionChanged = true;
			sourceChanged = true;
			colorChanged = true;
		}

		public vec2 Position
		{
			get => position;
			set
			{
				position = value;
				positionChanged = true;
			}
		}

		public vec2 Scale
		{
			get => scale;
			set
			{
				scale = value;
				positionChanged = true;
			}
		}

		public float X
		{
			get => position.x;
			set
			{
				position.x = value;
				positionChanged = true;
			}
		}

		public float Y
		{
			get => position.y;
			set
			{
				position.y = value;
				positionChanged = true;
			}
		}

		public float Rotation
		{
			get => rotation;
			set
			{
				rotation = value;
				positionChanged = true;
			}
		}

		public Color Color
		{
			get => color;
			set
			{
				color = value;
				colorChanged = true;
			}
		}

		public Shader Shader { get; set; }

		public SpriteModifiers Mods
		{
			get => mods;
			set
			{
				if (mods != value)
				{
					mods = value;
					positionChanged = true;
				}
			}
		}

		protected abstract void RecomputePositionData();
		protected abstract void RecomputeSourceData();

		private void RecomputeColorData()
		{
			// The data array can be null for strings with no value.
			if (data == null)
			{
				return;
			}

			float f = color.ToFloat();

			for (int i = 0; i < data.Length / VertexSize; i++)
			{
				data[i * VertexSize + 4] = f;
			}
		}

		public void Dispose()
		{
			Shader?.Dispose();
		}

		public abstract void Draw(SpriteBatch sb);

		protected void Draw(SpriteBatch sb, uint textureId, float[] data)
		{
			// Source is intentionally computed before position to make sure the proper origin is applied.
			if (sourceChanged)
			{
				RecomputeSourceData();
				sourceChanged = false;
			}

			if (positionChanged)
			{
				RecomputePositionData();
				positionChanged = false;
			}

			if (colorChanged)
			{
				RecomputeColorData();
				colorChanged = false;
			}

			sb.BindTexture(textureId);

			if (Shader != null)
			{
				sb.Apply(Shader, GL_TRIANGLE_STRIP);
			}
			else
			{
				sb.Mode = GL_TRIANGLE_STRIP;
			}

			// Strings need to buffer each character individually in order to add the primitive restart index each
			// time.
			int quads = data.Length / QuadSize;

			ushort[] indices =
			{
				0, 1, 2, 3
			};

			for (int i = 0; i < quads; i++)
			{
				sb.Buffer(data, indices, i * QuadSize, QuadSize);
			}
		}
	}
}
