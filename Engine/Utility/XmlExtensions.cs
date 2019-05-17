using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Engine.Utility
{
	public static class XmlExtensions
	{
		public static XElement Local(this XElement element, string tag)
		{
			return element.Elements().FirstOrDefault(e => e.Name.LocalName == tag);
		}

		public static XElement[] Locals(this XElement element, string tag)
		{
			return element.Elements().Where(e => e.Name.LocalName == tag).ToArray();
		}
	}
}
