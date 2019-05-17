using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using static Engine.GL;

namespace Engine.Graphics
{
	public class PrimitiveBuffer
	{
		private static readonly uint[] restartModes =
		{
			GL_LINE_LOOP,
			GL_LINE_STRIP,
			GL_TRIANGLE_FAN,
			GL_TRIANGLE_STRIP
		};

		private int bufferSize;
		private int maxIndex;

		private byte[] buffer;
		private ushort[] indexBuffer;
		private bool primitiveRestartEnabled;

		public PrimitiveBuffer(int bufferCapacity, int indexCapacity)
		{
			buffer = new byte[bufferCapacity];
			indexBuffer = new ushort[indexCapacity];
			maxIndex = -1;
		}

		public int Size => bufferSize;
		public int IndexCount { get; private set; }

		public uint Mode
		{
			set => primitiveRestartEnabled = restartModes.Contains(value);
		}

		public void Buffer<T>(T[] data, ushort[] indices, int start = 0, int length = -1) where T : struct
		{
			int l = data.Length;
			int size = Marshal.SizeOf(typeof(T));
			int sizeInBytes = size * (length != -1 ? length : l);

			// See https://stackoverflow.com/a/4636735/7281613.
			System.Buffer.BlockCopy(data, start * size, buffer, bufferSize, sizeInBytes);
			
			int max = -1;

			for (int i = 0; i < indices.Length; i++)
			{
				int index = indices[i];

				indexBuffer[IndexCount + i] = (ushort)(maxIndex + index + 1);
				max = Math.Max(max, index);
			}

			IndexCount += indices.Length;
			maxIndex += max + 1;

			bufferSize += sizeInBytes;

			if (primitiveRestartEnabled)
			{
				indexBuffer[IndexCount] = Constants.RestartIndex;
				IndexCount++;
			}
		}

		public unsafe uint Flush()
		{
			if (primitiveRestartEnabled)
			{
				glEnable(GL_PRIMITIVE_RESTART);
			}
			else
			{
				glDisable(GL_PRIMITIVE_RESTART);
			}

			// It's assumed that a relevant shader will be applied before calling this function (such that the correct
			// buffers are already bound).
			fixed (byte* address = &buffer[0])
			{
				glBufferSubData(GL_ARRAY_BUFFER, 0, (uint)bufferSize, address);
			}

			fixed (ushort* address = &indexBuffer[0])
			{
				glBufferSubData(GL_ELEMENT_ARRAY_BUFFER, 0, sizeof(ushort) * (uint)IndexCount, address);
			}

			uint count = (uint)IndexCount;

			bufferSize = 0;
			IndexCount = 0;
			maxIndex = -1;

			return count;
		}
	}
}
