using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Engine.Interfaces._3D;
using Engine.Shaders;
using Engine.Shapes._2D;
using Engine.Shapes._3D;
using Engine.Utility;
using Engine.View;
using GlmSharp;
using static Engine.GL;

namespace Engine.Graphics._3D
{
	public class PrimitiveRenderer3D
	{
		private Camera3D camera;
		private Shader primitiveShader;
		private PrimitiveBuffer buffer;

		private uint bufferId;
		private uint indexBufferId;
		private uint mode;

		public PrimitiveRenderer3D(Camera3D camera)
		{
			const int BufferCapacity = 2048;
			const int IndexCapacity = 256;

			this.camera = camera;

			buffer = new PrimitiveBuffer(BufferCapacity, IndexCapacity);

			GLUtilities.AllocateBuffers(BufferCapacity, IndexCapacity, out bufferId, out indexBufferId,
				GL_DYNAMIC_DRAW);

			primitiveShader = new Shader();
			primitiveShader.Attach(ShaderTypes.Vertex, "Primitives3D.vert");
			primitiveShader.Attach(ShaderTypes.Fragment, "Primitives.frag");
			primitiveShader.CreateProgram();
			primitiveShader.AddAttribute<float>(3, GL_FLOAT);
			primitiveShader.AddAttribute<byte>(4, GL_UNSIGNED_BYTE, true);
			primitiveShader.Bind(bufferId, indexBufferId);
		}

		public void Draw(Box box, Color color)
		{
			vec3 center = box.Position;
			vec3 halfSize = new vec3(box.Width, box.Height, box.Depth) / 2;
			vec3 min = center - halfSize;
			vec3 max = center + halfSize;
			vec3[] points =
			{
				min,
				new vec3(min.x, min.y, max.z),
				new vec3(min.x, max.y, max.z),
				new vec3(min.x, max.y, min.z),
				max,
				new vec3(max.x, max.y, min.z),
				new vec3(max.x, min.y, min.z),
				new vec3(max.x, min.y, max.z) 
			};

			quat orientation = box.Orientation;

			if (orientation != quat.Identity)
			{
				for (int i = 0; i < points.Length; i++)
				{
					points[i] *= orientation;
				}
			}

			ushort[] indices =
			{
				0, 1, 1, 2, 2, 3, 3, 0,
				4, 5, 5, 6, 6, 7, 7, 4,
				0, 6, 1, 7, 2, 4, 3, 5
			};

			Buffer(points, color, GL_LINES, indices);
		}

		public void Draw(Circle circle, float y, Color color, int segments)
		{
			vec3[] points = new vec3[segments];

			float increment = Constants.TwoPi / segments;

			for (int i = 0; i < segments; i++)
			{
				vec2 p = circle.Position + Utilities.Direction(circle.Rotation + increment * i) * circle.Radius;

				points[i] = new vec3(p.x, y, p.y);
			}

			Buffer(points, color, GL_LINE_LOOP);
		}

		public void Draw(Arc arc, float y, Color color, int segments)
		{
			vec2 center = arc.Position;
			vec3[] points = new vec3[segments + 2];

			float start = arc.Angle - arc.Spread / 2;
			float increment = arc.Spread / segments;

			for (int i = 0; i <= segments; i++)
			{
				vec2 p = center + Utilities.Direction(start + increment * i) * arc.Radius;

				points[i] = new vec3(p.x, y, p.y);
			}

			points[points.Length - 1] = new vec3(center.x, y, center.y);

			Buffer(points, color, GL_LINE_LOOP);
		}

		public void DrawLine(vec3 p1, vec3 p2, Color color)
		{
			vec3[] points = { p1, p2 };

			Buffer(points, color, GL_LINES);
		}

		public void DrawTriangle(vec3[] points, Color color)
		{
			float[] data = GetData(points, color);
			ushort[] indices = { 0, 1, 2, 0 };

			buffer.Buffer(data, indices);
		}

		private float[] GetData(vec3[] points, Color color)
		{
			float f = color.ToFloat();
			float[] data = new float[points.Length * 4];

			for (int i = 0; i < points.Length; i++)
			{
				int start = i * 4;

				vec3 p = points[i];

				data[start] = p.x;
				data[start + 1] = p.y;
				data[start + 2] = p.z;
				data[start + 3] = f;
			}

			return data;
		}

		private void Buffer(vec3[] points, Color color, uint mode, ushort[] indices = null)
		{
			if (this.mode != mode)
			{
				Flush();

				this.mode = mode;
		
				buffer.Mode = mode;
			}

			float[] data = GetData(points, color);

			// If the index array is null, it's assumed that a looped render mode is active (meaning that indices can
			// be added sequentially).
			if (indices == null)
			{
				indices = new ushort[points.Length];

				for (int i = 0; i < indices.Length; i++)
				{
					indices[i] = (ushort)i;
				}
			}

			buffer.Buffer(data, indices);
		}

		public unsafe void Flush()
		{
			if (buffer.Size == 0)
			{
				return;
			}

			primitiveShader.Apply();
			primitiveShader.SetUniform("mvp", camera.ViewProjection);

			glDrawElements(mode, buffer.Flush(), GL_UNSIGNED_SHORT, null);
		}
	}
}
