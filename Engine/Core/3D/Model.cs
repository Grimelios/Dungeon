using Engine.Graphics._3D;
using Engine.Interfaces._3D;
using GlmSharp;

namespace Engine.Core._3D
{
	public class Model : ITransformable3D
	{
		public Model(string filename)
		{
			Mesh = ContentCache.GetMesh(filename);
			Scale = vec3.Ones;
			Orientation = quat.Identity;
		}

		public vec3 Position { get; set; }
		public vec3 Scale { get; set; }
		public quat Orientation { get; set; }
		public Mesh Mesh { get; }
		public mat4 WorldMatrix { get; private set; }

		public void SetTransform(vec3 position, quat orientation)
		{
			Position = position;
			Orientation = orientation;
		}

		public void RecomputeWorldMatrix()
		{
			WorldMatrix = mat4.Translate(Position) * Orientation.ToMat4 * mat4.Scale(Scale);
		}
	}
}
