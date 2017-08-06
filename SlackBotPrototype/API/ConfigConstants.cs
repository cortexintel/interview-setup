using System;
using System.Collections.Generic;

namespace API
{
	public static class ConfigConstants
	{
		public static readonly string DarkSkyApiSecret = Environment.GetEnvironmentVariable("DARK_SKY_TOKEN");
		public static readonly string SlackApiSecret = Environment.GetEnvironmentVariable("SLACK_API_TOKEN");
		public const string DbName = "SlackBotDb.db";
		public const string DarkSkyBaseUrl = "https://api.darksky.net";
	}
}
