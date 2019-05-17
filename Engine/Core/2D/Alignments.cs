using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core._2D
{
	[Flags]
	public enum Alignments
	{
		Center = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8
	}
}
