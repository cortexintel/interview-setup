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
	public class CommandTests : EmbeddedResourceTestBase
	{
		private static readonly Location TestLocation =
			new Location {Latitude = 38.889931, Longitude = -77.009003, DisplayName = "Washington, DC"};

		private IWeatherProvider _weatherProvider;
		private IRepository _repository;

		[OneTimeSetUp]
		public void Init()
		{
			var mockHttp = new Mock<IHttpRequestClient>();

			var dailyForecastJson = GetFromResource("dc-daily.json");
			mockHttp.Setup(m => m.GetContent(It.IsAny<string>())).Returns(dailyForecastJson);
			_weatherProvider = new DarkSkyWeatherProvider(ConfigConstants.DarkSkyApiSecret, mockHttp.Object);

			var mockRepo = new Mock<IRepository>();
			mockRepo.Setup(m => m.GetLocation("San Francisco", "CA"))
				.Returns(new Location() { City = "San Francisco", State = "CA", DisplayName = "San Francisco, CA" });

			_repository = mockRepo.Object;
		}

		[Test]
		public void TestLocationInvalidWeatherCommand()
		{
			
			var location = new Location()
			{
				Latitude = 0,
				Longitude = 0
			};

			var when = DateTime.Parse("2017/07/01");

			var weatherCommand = new WeatherNowCommand(_weatherProvider, location, when);
			var msg = weatherCommand.Invoke();

			// SHORTCUT: A bit of shortcut here, normally we'd define a
			// some sort of reponse code enum and test based on the response code.
			// Since we don't return a result code right now (requires more boilerplate), just test based on expected string.
			Assert.IsTrue(msg.Contains("No location specified"));
		}

		[Test]
		public void TestValidLocationWeatherCommand()
		{
			var when = DateTime.Parse("2017/07/01");

			var weatherCommand = new WeatherNowCommand(_weatherProvider, TestLocation, when);
			var msg = weatherCommand.Invoke();
			
			// we know that we always include the city in weather results, this would have 
			// to be updated as things change
			Assert.IsTrue(msg.Contains("Washington, DC"));
		}

		[Test]
		public void TestSetLocationCommand()
		{
			var location = new Location();

			var setLocationCommand = new SetUserLocationCommand(_repository, "Test", "123", location);
			var msg = setLocationCommand.Invoke();

			Assert.IsTrue(msg.Contains("Location invalid"));
		}

		[Test]
		public void TestSetValidLocationCommand()
		{
			var setLocationCommand = new SetUserLocationCommand(_repository, "Test", "123", TestLocation);

			// Don't think there's much of a point to testing the repository inserts since
			// without setting up an integration test with SQLite
			var msg = setLocationCommand.Invoke();

			Assert.IsTrue(msg.Contains("Set location"));
		}

		[Test]
		public void TestSetNullLocation()
		{
			var setLocationCommand = new SetUserLocationCommand(_repository, "Test", "123", null);

			// Don't think there's much of a point to testing the repository inserts since
			// without setting up an integration test with SQLite
			var msg = setLocationCommand.Invoke();

			Assert.IsTrue(msg.Contains("Location not found"));
		}

		[TestCase(ExpectedResult = "")]
		public string TestNoopCommand()
		{
			var noop = new NoOpCommand();
			return noop.Invoke();
		}

		[Test]
		public void TestWhatToWearCommand()
		{
			var whatToWear = new WhatToWearCommand(_weatherProvider);
			var msg = whatToWear.Invoke();

			Assert.IsTrue(msg == "Bring a jacket");
		}
	}
}
