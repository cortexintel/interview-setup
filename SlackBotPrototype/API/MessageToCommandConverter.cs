using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace API
{
	public class MessageToCommandConverter : IMessageToCommandConverter
	{
		private readonly IWeatherProvider _weatherProvider;
		private readonly IRepository _repository;

		// Build a dictionary of regex patterns to match for commands, we iterate through sequentially so
		// order of patterns matters, since the first match is the one we go with. Once we reach a certain number of commands this 
		// will become unwieldy. At that point we'd need to switch to something more robust for parsing like NLP 
		//
		// Shortcut: We use a tuple here but this could be a class instead, reducing boilerplate by skipping another class
		private readonly ICollection<Tuple<Regex, Func<SlackQueryDto, ICommandImpl>>> _messageMap = 
			new List<Tuple<Regex, Func<SlackQueryDto, ICommandImpl>>>
			{
				new Tuple<Regex, Func<SlackQueryDto, ICommandImpl>>(new Regex("^<@.*>\\sset me to\\s(.*)\\s?[,]\\s(.*)$", RegexOptions.IgnoreCase), BuildSetLocation),
				new Tuple<Regex, Func<SlackQueryDto, ICommandImpl>>(new Regex("^<@.*>\\sweather\\s(now|today|tomorrow)$", RegexOptions.IgnoreCase), BuildWeatherNow),
				new Tuple<Regex, Func<SlackQueryDto, ICommandImpl>>(new Regex("^<@.*>\\sweather\\s(now|today|tomorrow)\\s(.*)\\s?[,]\\s(.*)$", RegexOptions.IgnoreCase), BuildWeatherNowWithLocation),
				new Tuple<Regex, Func<SlackQueryDto, ICommandImpl>>(new Regex("^<@.*>\\swhat should i wear", RegexOptions.IgnoreCase), BuildWhatToWear)
			};

		private readonly LocationService _locationService;

		private static ICommandImpl BuildSetLocation(SlackQueryDto dto)
		{
			var matches = dto.QueryParams;
			var invalid = new InvalidCommand();

			if (matches.Count == 0)
			{
				return invalid;
			}

			var city = matches.Skip(0).Take(1).First();
			var state = matches.Skip(1).Take(1).First();

			var location = dto.LocationService.GetLocation(city, state);

			return new SetUserLocationCommand(dto.Repository, dto.UserSlackId, dto.UserSlackId, location);
		}

		private static ICommandImpl BuildWhatToWear(SlackQueryDto dto)
		{
			var weatherProvider = dto.WeatherProvider;
			// Require user to set location first??
			return new WhatToWearCommand(weatherProvider);
		}

		private static ICommandImpl BuildWeatherNow(SlackQueryDto dto)
		{
			var matches = dto.QueryParams;
			var weatherProvider = dto.WeatherProvider;
			
			if (matches.Count == 0)
			{
				return new InvalidCommand("Timme for weather command not specified");
			}

			var timeParam = matches.First();

			if (string.Equals(timeParam, "now", StringComparison.InvariantCultureIgnoreCase))
			{
				return new WeatherNowCommand(weatherProvider, dto.Location, DateTime.Now);
			}

			if (string.Equals(timeParam, "tomorrow", StringComparison.InvariantCultureIgnoreCase))
			{
				return new WeatherNowCommand(weatherProvider, dto.Location, DateTime.Now.AddDays(1));
			}

			return new InvalidCommand("Expected `now` or `tomorrow`");
		}

		private static ICommandImpl BuildWeatherNowWithLocation(SlackQueryDto dto)
		{
			var matches = dto.QueryParams;
			var weatherProvider = dto.WeatherProvider;

			if (matches.Count == 0)
			{
				return new InvalidCommand("Timme for weather command not specified");
			}

			var timeParam = matches.First();
			var city = matches.Skip(1).Take(1).First();
			var state = matches.Skip(2).Take(1).First();


			var location = dto.LocationService.GetLocation(city, state);

			if (location == null || location.IsInvalid())
			{
				return new InvalidCommand($"Location not found for: {city} {state}");
			}

			if (string.Equals(timeParam, "now", StringComparison.InvariantCultureIgnoreCase))
			{
				return new WeatherNowCommand(weatherProvider, location, DateTime.Now);
			}

			if (string.Equals(timeParam, "tomorrow", StringComparison.InvariantCultureIgnoreCase))
			{
				return new WeatherNowCommand(weatherProvider, location, DateTime.Now.AddDays(1));
			}

			return new InvalidCommand("Expected `now` or `tomorrow`");
		}

		public MessageToCommandConverter(IWeatherProvider weatherProvider, IRepository repository)
		{
			_weatherProvider = weatherProvider;
			_repository = repository;
			_locationService = new LocationService(_repository);
		}

		/// <summary>
		/// Parse a message and return a command that can be invoked
		/// </summary>
		/// <param name="slackId">the Slack Id of the user who queried bott</param>
		/// <param name="message">the message user sent</param>
		/// <returns></returns>
		public MessageParserResponse HandleRequest(string slackId, string message)
		{
			var noop = new MessageParserResponse
			{
				Command = new NoOpCommand(),
				OriginalMessage = message
			};

			if (message == null)
			{
				return noop;
			}

			var user = _repository.Get(slackId);
			var location = _locationService.GetLocation(user?.LocationId ?? 0);
			
			foreach (var pair in _messageMap)
			{
				var match = pair.Item1.Match(message.Trim());

				if (!match.Success)
				{
					continue;
				}

				var capturedWords = new HashSet<string>();
				
				// first group is always the full original matched string so skip
				for (var i = 1; i < match.Groups.Count; i++)
				{
					capturedWords.Add(match.Groups[i].Value);
				}
				
				var queryDto = new SlackQueryDto
				{
					UserSlackId = slackId,
					QueryParams = capturedWords,
					// NOTE: We need to pass dependencies so static
					// builders can use it.
					WeatherProvider = _weatherProvider,
					Repository = _repository,
					LocationService = _locationService,
					Location = location
				};

				// Shortcut: We could have each command handle the group matches
				// themselves in order to handle more complex parameters
				return new MessageParserResponse
				{
					Command = pair.Item2(queryDto),
					OriginalMessage = message
				};
			}

			return noop;
		}
	}
	
}
