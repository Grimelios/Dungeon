using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces._2D;
using GlmSharp;

namespace Dungeon.Physics
{
	[DebuggerDisplay("Position={Position}, OldPosition={OldPosition}, Rotation={Rotation}")]
	public class VerletPoint : IPositionable2D, IRotatable
	{
		public vec2 Position { get; set; }
		public vec2 OldPosition { get; set; }

		public float Rotation { get; set; }
	}
}
