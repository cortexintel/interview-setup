using System;
using System.Linq;
using System.Threading;
using API;
using FluentScheduler;
using SlackAPI;
using SlackAPI.WebSocketMessages;

namespace SlackBotPrototype
{
	internal static class Program
	{
		private static SlackSocketClient _client;
		private static IMessageParser _messageParser;
		private static IWeatherProvider _weatherProvider;

		//1. Can assume that all requests are for one location(no need to manage individual user location...an instance of the bot can serve "Washington, DC" only).
		//2. Respond to two commands triggered upon mention. "Weather now" "Weather tomorrow". They do what you'd expect.
		//3. When the weather is going to be materially different from yesterday, let @channel know in the morning.
		//4. One embellishment of your choice. Determine a feature you think this bot should have, and implement it.
		private static void Main(string[] args)
		{
			var clientReady = new ManualResetEventSlim(false);
			var slackToken = ConfigConstants.SlackApiSecret;
			var darkSkyToken = ConfigConstants.DarkSkyApiSecret;
			var nlpFolder = ConfigConstants.StanfordNlpFolder;

			if (string.IsNullOrWhiteSpace(slackToken))
			{
				// normally would use logging library instead
				Console.WriteLine("Slack token not found in environment var name = `{SLACK_API_TOKEN}`");
				return;
			}

			if (string.IsNullOrWhiteSpace(nlpFolder))
			{
				Console.WriteLine("NLP folder not found in environment var name = `{STANFORD_NLP_FOLDER}`");
				return;
			}

			if (string.IsNullOrWhiteSpace(darkSkyToken))
			{
				Console.WriteLine("Dark sky API token not available var name = `{DARK_SKY_TOKEN}`");
				return;
			}

			_client = new SlackSocketClient(slackToken);
			_messageParser = new MessageParser(nlpFolder);
			_weatherProvider = new DarkSkyWeatherProvider(darkSkyToken);

			_client.Connect((connected) => {
				// This is called once the client has emitted the RTM start command
				Console.WriteLine($"RTM started -- {string.Join(",", connected.channels.Select(c => c.name))}");
				clientReady.Set();
				
			}, () => {
				// This is called once the RTM client has connected to the end point
				Console.WriteLine("RTM connected");
			});
			
			Channel[] channels;
			_client.GetChannelList((response) =>
			{
				channels = response.channels.Where(c => c.is_member).ToArray();
				
				JobManager.AddJob(() =>
				{
					var forecastWarning = _weatherProvider.GetForecastWarning();

					if (channels.Length <= 0 || string.IsNullOrWhiteSpace(forecastWarning)) return;

					foreach (var channel in channels)
					{
						_client.PostMessage((r) => { }, channel.id, $"@channel Notification: weather changed -- {forecastWarning}", linkNames: true);
					}

				}, (s) => s.ToRunEvery(1).Days().At(6, 0));
			});

			_client.OnMessageReceived += OnClientOnOnMessageReceived;

			clientReady.Wait();
			Console.Read();
		}
		
		private static void OnClientOnOnMessageReceived(NewMessage message)
		{
			Console.WriteLine(_client.MyData.name);
			Console.WriteLine(message.type + " " + message.text);

			if (message.text.Contains($"@{_client.MyData.id}"))
			{
				Console.WriteLine("Mentioned");
				var resp = _messageParser.HandleRequest(message.text);
				_client.PostMessage((response) => { }, message.channel, !string.IsNullOrWhiteSpace(resp) ? $"{resp}" : "Huh?");
			}
			else
			{
				Console.WriteLine("Not mentioned");
			}
		}
	}
	

}
