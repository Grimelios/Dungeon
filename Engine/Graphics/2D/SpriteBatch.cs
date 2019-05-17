using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Engine.Core._2D;
using Engine.Interfaces;
using Engine.Messaging;
using Engine.Shaders;
using Engine.Shapes._2D;
using Engine.Utility;
using GlmSharp;
using static Engine.GL;

namespace Engine.Graphics._2D
{
	public class SpriteBatch : IReceiver
	{
		private Shader spriteShader;
		private Shader primitiveShader;
		private Shader activeShader;
		private PrimitiveBuffer buffer;
		private mat4 mvp;

		private uint bufferId;
		private uint indexBufferId;
		private uint mode;
		private uint activeTexture;

		public SpriteBatch()
		{
			const int BufferCapacity = 30000;
			const int IndexCapacity = 3000;

			buffer = new PrimitiveBuffer(BufferCapacity, IndexCapacity);

			GLUtilities.AllocateBuffers(BufferCapacity, IndexCapacity, out bufferId, out indexBufferId,
				GL_DYNAMIC_DRAW);

			// These two shaders (owned by the sprite batch) can be completed here (in terms of binding a buffer).
			// External shaders are bound when first applied.
			spriteShader = new Shader();
			spriteShader.Attach(ShaderTypes.Vertex, "Sprite.vert");
			spriteShader.Attach(ShaderTypes.Fragment, "Sprite.frag");
			spriteShader.AddAttribute<float>(2, GL_FLOAT);
			spriteShader.AddAttribute<float>(2, GL_FLOAT);
			spriteShader.AddAttribute<byte>(4, GL_UNSIGNED_BYTE, false, true);
			spriteShader.CreateProgram();
			spriteShader.Bind(bufferId, indexBufferId);

			primitiveShader = new Shader();
			primitiveShader.Attach(ShaderTypes.Vertex, "Primitives2D.vert");
			primitiveShader.Attach(ShaderTypes.Fragment, "Primitives.frag");
			primitiveShader.CreateProgram();
			primitiveShader.AddAttribute<float>(2, GL_FLOAT);
			primitiveShader.AddAttribute<byte>(4, GL_UNSIGNED_BYTE, false, true);
			primitiveShader.Bind(bufferId, indexBufferId);

			MessageSystem.Subscribe(this, CoreMessageTypes.ResizeWindow, (messageType, data, dt) => { OnResize(); });
		}

		public uint Mode
		{
			get => mode;
			set
			{
				if (mode != value)
				{
					Flush();

					mode = value;
					buffer.Mode = value;
				}
			}
		}

		public List<MessageHandle> MessageHandles { get; set; }

		private void OnResize()
		{
			var halfDimensions = Resolution.WindowDimensions / 2;

			mvp = mat4.Scale(1f / halfDimensions.x, 1f / halfDimensions.y, 1);
			mvp *= mat4.Translate(-halfDimensions.x, -halfDimensions.y, 0);
		}

		public void Buffer(float[] data, ushort[] indices = null, int start = 0, int length = -1)
		{
			if (activeShader == null)
			{
				activeShader = spriteShader;
			}

			if (indices == null)
			{
				// Each 2D vertex has position and color.
				indices = new ushort[data.Length / 3];

				for (int i = 0; i < indices.Length; i++)
				{
					indices[i] = (ushort)i;
				}
			}
			
			buffer.Buffer(data, indices, start, length);
		}

		public void Apply(Shader shader, uint mode)          
		{
			if (activeShader == shader && this.mode == mode)
			{
				return;
			}

			Flush();
			activeShader = shader;
			Mode = mode;

			if (!activeShader.BindingComplete)
			{
				activeShader.Bind(bufferId, indexBufferId);
			}
		}

		public void BindTexture(uint id)
		{
			if (activeTexture == id)
			{
				return;
			}

			Flush();
			activeTexture = id;
		}

		public void Draw(Line line, Color color)
		{
			DrawLine(line.P1, line.P2, color);
		}

		public void Draw(Circle circle, int segments, Color color)
		{
			Apply(primitiveShader, GL_LINE_LOOP);

			float increment = Constants.TwoPi / segments;
			float[] data = new float[segments * 3];
			float f = color.ToFloat();

			for (int i = 0; i < segments; i++)
			{
				vec2 p = circle.Position + Utilities.Direction(increment * i + circle.Rotation) * circle.Radius;

				int start = i * 3;

				data[start] = p.x;
				data[start + 1] = p.y;
				data[start + 2] = f;
			}

			Buffer(data);
		}

		public void Draw(Bounds2D bounds, Color color)
		{
			Draw(bounds.ToRectangle(), color);
		}

		public void Draw(Rectangle rect, Color color)
		{
			Apply(primitiveShader, GL_LINE_LOOP);

			var corners = rect.Corners;

			float f = color.ToFloat();
			float[] data = new float[12];

			for (int i = 0; i < 4; i++)
			{
				vec2 p = corners[i];

				int start = i * 3;

				data[start] = p.x;
				data[start + 1] = p.y;
				data[start + 2] = f;
			}

			Buffer(data);
		}

		public void DrawLine(vec2 p1, vec2 p2, Color color)
		{
			Apply(primitiveShader, GL_LINES);

			float f = color.ToFloat();
			float[] data =
			{
				p1.x,
				p1.y,
				f,
				p2.x,
				p2.y,
				f
			};

			ushort[] indices = { 0, 1 };

			Buffer(data, indices);
		}

		public unsafe void Flush()
		{
			if (buffer.Size == 0)
			{
				return;
			}

			// This assumes that all 2D shaders will contain a uniform matrix called "mvp".
			activeShader.Apply();
			activeShader.SetUniform("mvp", mvp);

			if (activeTexture != 0)
			{
				glActiveTexture(GL_TEXTURE0);
				glBindTexture(GL_TEXTURE_2D, activeTexture);
			}

			glDrawElements(mode, buffer.Flush(), GL_UNSIGNED_SHORT, null);

			activeShader = null;
			activeTexture = 0;
		}
	}
}
