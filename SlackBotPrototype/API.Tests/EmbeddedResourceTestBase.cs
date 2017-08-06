using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace API.Tests
{
	public class EmbeddedResourceTestBase
	{

		protected string GetFromResource(string fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));

			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = assembly.GetManifestResourceNames().First(x => x.Contains(fileName));

			using (var file = assembly.GetManifestResourceStream(resourceName))
			{
				if (file == null)
					Assert.Fail();

				using (var sr = new StreamReader(file))
				{
					var json = sr.ReadToEnd();
					return json;
				}
			}
		}
	}
}
