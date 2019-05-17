using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core._2D
{
	public abstract class QuadSource
	{
		protected QuadSource(int width, int height) : this(0, width, height)
		{
		}

		protected QuadSource(uint id, int width, int height)
		{
			Id = id;
			Width = width;
			Height = height;
		}

		public uint Id { get; protected set; }
		public int Width { get; }
		public int Height { get; }
	}
}
