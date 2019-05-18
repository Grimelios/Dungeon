using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using Engine.Utility;
using GlmSharp;

namespace Dungeon.Physics
{
	public class VerletRope : IPositionable2D, IDynamic
	{
		private int segmentLength;

		public VerletRope(int segmentCount, int segmentLength)
		{
			this.segmentLength = segmentLength;

			Points = new VerletPoint[segmentCount + 1];

			for (int i = 0; i < Points.Length; i++)
			{
				Points[i] = new VerletPoint();
			}
		}

		public vec2 Position
		{
			get => Points[0].Position;
			set => Points[0].Position = value;
		}

		public VerletPoint[] Points { get; }

		public void Update(float dt)
		{
			const int Iterations = 5;
			const int Gravity = 15;

			// It's assumed that the first point is always fixed. The other endpoint may or may not be fixed.
			for (int j = 1; j < Points.Length; j++)
			{
				var point = Points[j];
				var p = point.Position;
				var temp = p;

				p.y += Gravity * dt;
				p += (p - point.OldPosition) * 0.98f;
				point.Position = p;
				point.OldPosition = temp;
			}

			// Solve constraints.
			for (int i = 0; i < Iterations; i++)
			{
				vec2[] corrections = new vec2[Points.Length - 1];

				for (int j = 0; j < Points.Length - 1; j++)
				{
					VerletPoint point1 = Points[j];
					VerletPoint point2 = Points[j + 1];

					var p1 = point1.Position;
					var p2 = point2.Position;

					float squared = Utilities.DistanceSquared(p1, p2);

					if (squared <= segmentLength * segmentLength)
					{
						continue;
					}
					
					float distance = (float)Math.Sqrt(squared);
					float delta = distance - segmentLength;

					corrections[j] = (p2 - p1) / distance * delta / 2;
				}

				for (int j = 1; j < Points.Length; j++)
				{
					var point = Points[j];
					var p = point.Position;

					if (j == 1)
					{
						p -= corrections[0] * 2;
					}
					else if (j == Points.Length - 1)
					{
						p -= corrections.Last() * 2;
					}
					else
					{
						p += corrections[j] - corrections[j - 1];
					}
					
					point.Position = p;
				}
			}
		}
	}
}
