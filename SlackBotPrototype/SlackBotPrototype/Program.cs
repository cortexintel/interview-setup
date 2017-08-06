using System;
using System.Linq;
using System.Threading;
using API;
using SlackAPI;
using SlackAPI.WebSocketMessages;

namespace SlackBotPrototype
{
	internal static class Program
	{
		private static SlackSocketClient _client;
		private static IMessageToCommandConverter _messageToCommandConverter;
		private static IWeatherProvider _weatherProvider;

		private static void Main(string[] args)
		{
			var clientReady = new ManualResetEventSlim(false);
			var slackToken = ConfigConstants.SlackApiSecret;
			var darkSkyToken = ConfigConstants.DarkSkyApiSecret;
			
			if (string.IsNullOrWhiteSpace(slackToken))
			{
				// normally would use logging library instead
				Console.WriteLine("Slack token not found in environment var name = `{SLACK_API_TOKEN}`");
				return;
			}

			if (string.IsNullOrWhiteSpace(darkSkyToken))
			{
				Console.WriteLine("Dark sky API token not available var name = `{DARK_SKY_TOKEN}`");
				return;
			}

			_client = new SlackSocketClient(slackToken);
			_weatherProvider = new DarkSkyWeatherProvider(darkSkyToken, new RestClientHttpProvider(ConfigConstants.DarkSkyBaseUrl));
			_messageToCommandConverter = new MessageToCommandConverter(_weatherProvider, new SqLitePersistence());

			_client.Connect((connected) => {
				// This is called once the client has emitted the RTM start command
				Console.WriteLine($"RTM started -- {string.Join(",", connected.channels.Select(c => c.name))}");
				clientReady.Set();
				
			}, () => {
				// This is called once the RTM client has connected to the end point
				Console.WriteLine("RTM connected");
			});

			_client.GetUserList((ulr) =>
			{
				Console.WriteLine("Got users");
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
				Console.WriteLine($"Mentioned by {message.user}:{message.id}");
				var resp = _messageToCommandConverter.HandleRequest(message.user, message.text);
				var msg = resp.Command.Invoke();
				
				_client.PostMessage((response) => { }, message.channel, !string.IsNullOrWhiteSpace(msg) ? msg : "Huh?");
			}
			else
			{
				Console.WriteLine("Not mentioned");
			}
		}
	}
	

}
