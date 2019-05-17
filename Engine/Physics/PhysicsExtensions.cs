using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Jitter.LinearMath;

namespace Engine.Physics
{
	public static class PhysicsExtensions
	{
		public static vec3 ToVec3(this JVector v)
		{
			return new vec3(v.X, v.Y, v.Z);
		}

		public static JVector ToJVector(this vec3 v)
		{
			return new JVector(v.x, v.y, v.z);
		}

		public static quat ToQuat(this JMatrix m)
		{
			var q = JQuaternion.CreateFromMatrix(m);

			return new quat(q.X, q.Y, q.Z, q.W);
		}

		public static JMatrix ToJMatrix(this quat q)
		{
			mat3 m = new mat3(q);

			return new JMatrix(m.m00, m.m10, m.m20, m.m01, m.m11, m.m21, m.m02, m.m12, m.m22);
		}
	}
}
