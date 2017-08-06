using System.Collections.Generic;
using SQLite;

namespace API
{
	public class User
	{
		[PrimaryKey, AutoIncrement,  Column("_id")]
		public int Id { get; set; }

		[Indexed]
		public string SlackId { get; set; }
		public string Name { get; set; }

		// SHORTCUT: Reference the lookup in memory rather than using FK on actual DB table
		public int? LocationId { get; set; }
	}

	// SHORTCUT: Skip storing this as actual entity in DB
	public class Location
	{
		[PrimaryKey]
		public int Id { get; set; }
		[Indexed]
		public string City { get; set; }
		[Indexed]
		public string State { get; set; }
		[Indexed]
		public string StateAbbreviation { get; set; }
		public string DisplayName { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public bool IsInvalid()
		{
			return string.IsNullOrWhiteSpace(DisplayName) || DisplayName == "Invalid";
		}
	}

	public class ChatBotForecast
	{
		public string Summary { get; set; }
		public string Icon { get; set; }
		public Location Location { get; set; }
	}
	
	public enum BotCommands
	{
		Unknown,
		WeatherNow,
		WeatherTomorrow,
		SetLocation,
		WeatherLocation
	}

	/// <summary>
	/// Property bag for query context information that we need for building
	/// commands.
	/// </summary>
	public class SlackQueryDto
	{
		// SHORTCUT: Instead of having to pass the weather provider around,
		// we could use some sort of dependency injection here.
		public IWeatherProvider WeatherProvider { get; set; }
		public ILocationService LocationService { get; set; }
		public IRepository Repository { get; set; }
		public ICollection<string> QueryParams { get; set; }
		public string UserSlackId { get; set; }
		public Location Location { get; set; }
	}

	public class MessageParserResponse
	{

		public ICommandImpl Command { get; set; }

		public string OriginalMessage { get; set; }
	}

}
