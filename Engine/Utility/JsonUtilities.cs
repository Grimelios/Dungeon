using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Engine.Utility
{
	public static class JsonUtilities
	{
		public static T Deserialize<T>(string filename)
		{
			return JsonConvert.DeserializeObject<T>(File.ReadAllText("Content/Json/" + filename));
		}
	}
}
