using System;
using System.Collections.Generic;
using System.Linq;

namespace API
{
	public class WeatherNowCommand : ICommandImpl
	{
		private readonly IWeatherProvider _weatherProvider;
		private readonly Location _location;
		private readonly DateTime _when;

		// SHORTCUT: Could set up dependency injection here instead of using constructors.
		// We need to pass in dependencies for commands to make them easier to test
		public WeatherNowCommand(IWeatherProvider weatherProvider, Location location, DateTime when)
		{
			_weatherProvider = weatherProvider;
			_location = location;
			_when = when;
		}

		public string Invoke()
		{
			return _location.IsInvalid() 
				? "No location specified. Specify location or set your location with `set me to City, State`, eg. `set me to new york, ny" 
				: $"{_location.DisplayName}: {_weatherProvider.GetForecast(_location, _when)}";
		}
	}

	public class WhatToWearCommand : ICommandImpl
	{
		private readonly IWeatherProvider _weatherProvider;

		// SHORTCUT: We could choose to keep this lookup in a configuration file, or in persistence layer (DB).
		// This would allow us to update the mapping without having to deploy code.
		private readonly IDictionary<string, string> _weatherToClothingLookup = new Dictionary<string, string>
		{
			{"clear-day", "Wear your comfortable clothing, no jacket required"},
			{"clear-night", "Wear your comfortable clothing, no jacket required"},
			{"snow", "Wear winter jackets, bring boots!"},
			{"wind", "Bring a wind jacket, it\'s going to breezy"},
			{"partly-cloudy-day","Bring a jacket"},
			{"partly-cloudy-night","Bring a jacket"},
			{"cloudy","Bring a jacket"},
			{"fog","Bring a jacket"},
			{"rain", "Dress comfortably, bring an umbrella"}
		};

		public WhatToWearCommand(IWeatherProvider weatherProvider)
		{
			_weatherProvider = weatherProvider;
		}

		public string Invoke()
		{
			// SHORTCUT: Not implementing location support for this command to save time, it's not in scope of project.
			// Just use a hard coded location.
			var dcLocation = new Location {Latitude = 38.889931, Longitude = -77.009003, DisplayName = "Washington, DC"};
			var summaryIcon = _weatherProvider.GetForecastRaw(dcLocation, DateTime.Now).Icon;
			return _weatherToClothingLookup.TryGetValue(summaryIcon, out string recommendation) ? recommendation : "I don't know, anything you want?";
		}
	}

	public class SetUserLocationCommand : ICommandImpl
	{
		private readonly IRepository _repository;
		private readonly string _name;
		private readonly string _slackId;
		private readonly Location _location;

		// We need to pass in dependencies for commands to make them easier to test
		public SetUserLocationCommand(IRepository repository, string name, string slackId, Location location)
		{
			_repository = repository;
			_name = name;
			_slackId = slackId;
			_location = location;
		}

		public string Invoke()
		{
			if (_location == null)
			{
				return "Location not found";
			}

			if (_location.IsInvalid())
			{
				return "Location invalid";
			}

			_repository.CreateOrUpdate(_name, _slackId, _location);
			return $"Set location to: {_location.DisplayName}";
		}
	}

	public class InvalidCommand : ICommandImpl
	{
		private readonly string _message;

		public InvalidCommand()
		{
		}

		public InvalidCommand(string message)
		{
			_message = message;
		}

		public string Invoke()
		{
			return _message ?? "Huh?";
		}
	}

	public class NoOpCommand : ICommandImpl
	{
		public string Invoke()
		{
			return string.Empty;
		}
	}
}
