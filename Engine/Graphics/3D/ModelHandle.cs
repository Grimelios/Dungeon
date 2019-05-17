using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core._3D;

namespace Engine.Graphics._3D
{
	public class ModelHandle
	{
		public ModelHandle(Model model, int count, int offset, int baseVertex)
		{
			Model = model;
			Count = count;
			Offset = offset;
			BaseVertex = baseVertex;
		}

		public Model Model { get; }
		
		public int Count { get; }
		public int Offset { get; }
		public int BaseVertex { get; }
	}
}
