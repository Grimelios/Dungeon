using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics._3D.Loaders;
using GlmSharp;

namespace Engine.Graphics._3D
{
	public class Mesh
	{
		public const string Path = "Content/Meshes/";

		public static Mesh Load(string filename)
		{
			return filename.EndsWith(".obj") ? ObjLoader.Load(filename) : DaeLoader.Load(filename);
		}

		public Mesh(vec3[] points, vec2[] source, vec3[] normals, ivec3[] vertices, ushort[] indices, string texture,
			ivec2[] boneIndexes = null, vec2[] boneWeights = null)
		{
			Points = points;
			Source = source;
			Normals = normals;
			Vertices = vertices;
			Indices = indices;
			MaxIndex = indices.Max();
			BoneIndexes = boneIndexes;
			BoneWeights = boneWeights;
			Texture = ContentCache.GetTexture(texture ?? "Grey.png");
		}

		public vec3[] Points { get; }
		public vec2[] Source { get; }
		public vec3[] Normals { get; }
		public ivec3[] Vertices { get; }

		// These two arrays will be left null for non-animated meshes.
		public ivec2[] BoneIndexes { get; }
		public vec2[] BoneWeights { get; }

		public ushort[] Indices { get; }
		public ushort MaxIndex { get; }

		public Texture Texture { get; }
	}
}
