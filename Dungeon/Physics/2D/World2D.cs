using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Shapes._2D;
using Engine.Utility;
using GlmSharp;

namespace Dungeon.Physics._2D
{
	public class World2D
	{
		private List<Body2D> dynamicBodies;
		private List<Body2D> staticBodies;

		public World2D()
		{
			dynamicBodies = new List<Body2D>();
			staticBodies = new List<Body2D>();
			Gravity = new vec2(0, 200);
		}

		public List<Body2D> DynamicBodies => dynamicBodies;
		public List<Body2D> StaticBodies => staticBodies;

		public vec2 Gravity { get; set; }

		public void Add(Body2D body)
		{
			if (body.IsStatic)
			{
				staticBodies.Add(body);
			}
			else
			{
				dynamicBodies.Add(body);
			}
		}

		public void Step(float dt)
		{
			foreach (var body in dynamicBodies)
			{
				body.Position += body.Velocity * dt;
				//body.Velocity += Gravity * dt;

				//var manifold = ComputeManifold(body);
				ComputeManifold(body);
			}
		}

		private void ComputeManifold(Body2D body)
		{
			List<vec2> list = new List<vec2>();

			// It's assumed that all dynamic bodies in this context will use capsules for character control. The
			// capsule will also always remain upright (i.e. rotation is fixed).
			Capsule capsule = (Capsule)body.Shape;

			foreach (var staticBody in staticBodies)
			{
				Rectangle rect = (Rectangle)staticBody.Shape;

				vec2 p1 = capsule.Position;
				vec2 p2 = rect.Position;

				float dX = Math.Abs(p1.x - p2.x);
				float dY = Math.Abs(p1.y - p2.y);
				float r = capsule.Radius;
				float sumX = rect.Width / 2 + capsule.Radius;
				float sumY = rect.Height / 2 + capsule.Height / 2 + r;

				bool overlapsX = dX <= sumX;
				bool overlapsY = dY <= sumY;

				if (!(overlapsX && overlapsY))
				{
					continue;
				}

				// This means the capsule must be colliding from the top or bottom.
				if (dX <= rect.Width / 2)
				{
					float correction = sumY - dY;

					if (p1.y > p2.y)
					{
						correction *= -1;
					}

					capsule.Y -= correction;

					return;
				}

				// This means the capsule must be colliding from the left or right.
				if (dY <= (rect.Height + capsule.Height) / 2)
				{
					float correction = sumX - dX;

					if (p1.x > p2.x)
					{
						correction *= -1;
					}

					capsule.X -= correction;

					return;
				}

				bool left = p1.x < p2.x;
				bool above = p1.y < p2.y;

				float x = left ? rect.Left : rect.Right;
				float y = above ? rect.Top : rect.Bottom;

				vec2 corner = new vec2(x, y);
				vec2 center = p1 + new vec2(0, capsule.Height / 2 * (above ? 1 : -1));

				float squared = Utilities.DistanceSquared(corner, center);

				if (squared >= r * r)
				{
					continue;
				}

				float distance = (float)Math.Sqrt(squared);
				float delta = r - distance;

				capsule.Position += Utilities.Normalize(center - corner) * delta;
			}

			//return ComputeFinalCorrection(list);
		}

		private vec2 ComputeFinalCorrection(List<vec2> list)
		{
			int count = list.Count;

			if (count == 0)
			{
				return vec2.Zero;
			}

			vec2 final = vec2.Zero;

			if (list.Count == 1)
			{
				return list[0];
			}

			for (int i = 0; i < list.Count; i++)
			{
				vec2 v = list[i];
				final += v;

				for (int j = i + 1; j < count; j++)
				{
					list[i] = Utilities.Project(v, list[i]);
				}
			}

			return final;
		}
	}
}
