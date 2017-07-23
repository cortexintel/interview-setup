using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
	public static class ConfigConstants
	{
		public static readonly string DarkSkyApiSecret = Environment.GetEnvironmentVariable("DARK_SKY_TOKEN");
		public static readonly string SlackApiSecret = Environment.GetEnvironmentVariable("SLACK_API_TOKEN");
		public static readonly string StanfordNlpFolder = Environment.GetEnvironmentVariable("STANFORD_NLP_FOLDER");
	}
}
