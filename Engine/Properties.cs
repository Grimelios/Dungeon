using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	public static class Properties
	{
		private static Dictionary<string, string> map = new Dictionary<string, string>();

		public static void Load(string filename)
		{
			string[] lines = File.ReadAllLines("Content/Text/" + filename);

			foreach (string line in lines)
			{
				if (line.Length == 0)
				{
					continue;
				}

				// The expected format of each line is "key = value" (although it'll work without the spaces as well).
				string[] tokens = line.Split('=');
				string key = tokens[0].TrimEnd();
				string value = tokens[1].TrimStart();

				// This assumes that no two properties will share the same name (across all loaded property files).
				map.Add(key, value);
			}
		}

		public static int GetInt(string key)
		{
			return int.Parse(map[key]);
		}

		public static float GetFloat(string key)
		{
			return float.Parse(map[key]);
		}

		public static string GetString(string key)
		{
			return map[key];
		}
	}
}
