using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utility;
using GlmSharp;

namespace Engine.Graphics._3D.Loaders
{
	public static class ObjLoader
	{
		public static Mesh Load(string filename)
		{
			string[] lines = File.ReadAllLines(Mesh.Path + filename);
			bool usesMaterial = lines[2][0] == 'm';
			int lineIndex = usesMaterial ? 3 : 2;
			string line = lines[lineIndex];

			// Parse points.
			List<vec3> points = new List<vec3>();

			do
			{
				points.Add(ParseVec3(line));
				line = lines[++lineIndex];
			}
			// The next lines will be either texture coordinates ("vt") or normals ("vn").
			while (line[1] == ' ');

			bool usesTexturing = line[1] == 't';

			// Parse source coordinates.
			List<vec2> source = new List<vec2>();

			if (usesTexturing)
			{
				do
				{
					source.Add(ParseVec2(line));
					line = lines[++lineIndex];
				}
				while (line[1] == 't');
			}
			else
			{
				// If no texture is bound, a default grey texture is used instead (with every vertex sampling from the
				// top-left corner).
				source.Add(vec2.Zero);
			}

			// Parse normals.
			List<vec3> normals = new List<vec3>();

			do
			{
				normals.Add(ParseVec3(line));
				line = lines[++lineIndex];
			}
			while (line[0] == 'v');

			// The next line (smoothing group) can be ignored for the time being.
			lineIndex++;

			// Parse triangles.
			List<ivec3> vertices = new List<ivec3>();
			List<ushort> indices = new List<ushort>();

			do
			{
				line = lines[lineIndex++];

				// Each mesh is assumed to use only a single texture. If certain faces don't have texturing applied,
				// additional "usemtl" lines can be present. Those lines are ignored.
				if (line[0] == 'u')
				{
					line = lines[lineIndex++];
				}

				// Each face line starts with 'f', then lists three vertices. Each vertex has three components
				// delimited by '/'. Faces are assumed to be triangulated on export.
				string[] tokens = line.Split(' ');

				for (int i = 1; i <= 3; i++)
				{
					string[] subTokens = tokens[i].Split('/');

					// .obj files use 1-indexing (rather than 0-indexing).
					int pointIndex = int.Parse(subTokens[0]) - 1;
					int sourceIndex = usesTexturing ? int.Parse(subTokens[1]) - 1 : 0;
					int normalIndex = int.Parse(subTokens[2]) - 1;

					ivec3 vertex = new ivec3(pointIndex, sourceIndex, normalIndex);

					int index;

					if ((index = vertices.IndexOf(vertex)) != -1)
					{
						indices.Add((ushort)index);
					}
					else
					{
						indices.Add((ushort)vertices.Count);
						vertices.Add(vertex);
					}
				}
			}
			while (lineIndex < lines.Length);

			string texture = usesTexturing
				? ParseTexture(Mesh.Path + Utilities.StripExtension(filename) + ".mtl")
				: null;

			return new Mesh(points.ToArray(), source.ToArray(), normals.ToArray(), vertices.ToArray(),
				indices.ToArray(), texture);
		}

		private static vec2 ParseVec2(string line)
		{
			string[] tokens = line.Split(' ');

			float x = float.Parse(tokens[1]);
			float y = float.Parse(tokens[2]);

			return new vec2(x, y);
		}

		private static vec3 ParseVec3(string line)
		{
			string[] tokens = line.Split(' ');

			float x = float.Parse(tokens[1]);
			float y = float.Parse(tokens[2]);
			float z = float.Parse(tokens[3]);

			return new vec3(x, y, z);
		}

		private static string ParseTexture(string filename)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException($"Material file \"{filename}\" not found.");
			}

			string[] lines = File.ReadAllLines(filename);
			string line = lines.First(l => l.StartsWith("map_Kd"));

			return Utilities.StripPath(line);
		}
	}
}
