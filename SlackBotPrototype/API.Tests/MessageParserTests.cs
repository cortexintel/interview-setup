using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace API.Tests
{

	[TestFixture]
	public class MessageParserTests : EmbeddedResourceTestBase
	{
		private MessageToCommandConverter _messageToCommandConverter;

		[OneTimeSetUp]
		public void Init()
		{
			var mockHttp = new Mock<IHttpRequestClient>();

			var dailyForecastJson = GetFromResource("dc-daily.json");
			mockHttp.Setup(m => m.GetContent(It.IsAny<string>())).Returns(dailyForecastJson);

			var weatherProvider = new DarkSkyWeatherProvider(ConfigConstants.DarkSkyApiSecret, mockHttp.Object);

			var mockRepo = new Mock<IRepository>();
			mockRepo.Setup(m => m.GetLocation("San Francisco", "CA"))
				.Returns(new Location() {City = "San Francisco", State = "CA", DisplayName = "San Francisco, CA"});
			mockRepo.Setup(m => m.GetLocation("new york", "ny"))
				.Returns(new Location() { City = "New York", State = "NY", DisplayName = "New York, NY" });
			mockRepo.Setup(m => m.GetLocation("New York", "NY"))
				.Returns(new Location() { City = "New York", State = "NY", DisplayName = "New York, NY" });

			_messageToCommandConverter = new MessageToCommandConverter(weatherProvider, mockRepo.Object);
		}
		
		[TestCase("<@U6BQE1XGD> weather now", typeof(WeatherNowCommand))]
		[TestCase("<@U6BQE1XGD> weather tomorrow", typeof(WeatherNowCommand))]
		[TestCase("<@U6BQE1XGD> weather today", typeof(InvalidCommand))]
		[TestCase("<@U6BQE1XGD> WEATHER NOW!", typeof(NoOpCommand))]
		[TestCase("<@U6BQE1XGD> WEATHER TOMORROW", typeof(WeatherNowCommand))]
		[TestCase("<@U6BQE1XGD> What should I wear?", typeof(WhatToWearCommand))]
		[TestCase("<@U6BQE1XGD> Set me to San Fransisco, CA", typeof(SetUserLocationCommand))]
		[TestCase("<@U6BQE1XGD> Set me to new york, ny", typeof(SetUserLocationCommand))]
		[TestCase("<@U6BQE1XGD> Set me to Washington, DC", typeof(SetUserLocationCommand))]
		[TestCase("<@U6BQE1XGD> Set me to Nowheresville123, NY", typeof(SetUserLocationCommand))]
		[TestCase("<@U6BQE1XGD> Weather now San Francisco, CA", typeof(WeatherNowCommand))]
		[TestCase("<@U6BQE1XGD> set me to new york, ny", typeof(SetUserLocationCommand))]
		[TestCase("<@U6BQE1XGD> set me to Unrecognizable location, OkayCity", typeof(SetUserLocationCommand))]
		[TestCase("<@U6BQE1XGD> set me to Unrecognizable location!!! !!, OkayCity", typeof(SetUserLocationCommand))]
		[TestCase("<@U6BQE1XGD> Weather now New York, NY", typeof(WeatherNowCommand))]
		[TestCase("<@U6BQE1XGD> Hello", typeof(NoOpCommand))]
		[TestCase("set me to new york, ny", typeof(NoOpCommand))]
		[TestCase("weather now", typeof(NoOpCommand))]
		[TestCase("", typeof(NoOpCommand))]
		[TestCase(null, typeof(NoOpCommand))]
		public void VerifyCommands(string message, Type expectedType)
		{
			var resp = _messageToCommandConverter.HandleRequest("test", message);
			var command = resp.Command;

			Assert.IsTrue(expectedType == command.GetType(), $"command did not match expected for message: {message} --> {command.GetType()}");
		}
	}
}
