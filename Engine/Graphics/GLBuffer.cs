using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.GL;

namespace Engine.Graphics
{
	public class GLBuffer : IDisposable
	{
		/**
		 * Note that buffer size should be given in bytes, while index size should be given in indices. Using this
		 * constructor implies GL_DYNAMIC_DRAW.
		 */
		public unsafe GLBuffer(int bufferSize, int indexSize)
		{
			uint[] buffers = new uint[2];

			fixed (uint* address = &buffers[0])
			{
				glGenBuffers(2, address);
			}

			BufferId = buffers[0];
			IndexId = buffers[1];

			// Note that buffer capacity should be given in bytes, while index capacity should be given in indexes
			// (i.e. unsigned shorts). This is meant to match how primitive buffers are created.
			glBindBuffer(GL_ARRAY_BUFFER, BufferId);
			glBufferData(GL_ARRAY_BUFFER, (uint)bufferSize, null, GL_DYNAMIC_DRAW);

			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IndexId);
			glBufferData(GL_ELEMENT_ARRAY_BUFFER, (uint)indexSize * sizeof(ushort), null, GL_DYNAMIC_DRAW);
		}

		public uint BufferId { get; }
		public uint IndexId { get; }

		public unsafe void Dispose()
		{
			uint[] buffers =
			{
				BufferId,
				IndexId
			};

			fixed (uint* address = &buffers[0])
			{
				glDeleteBuffers(2, address);
			}
		}
	}
}
