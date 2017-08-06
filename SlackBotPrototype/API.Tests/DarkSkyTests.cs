using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace API.Tests
{

	/// <summary>
	/// Not a unit test, normally would do this in separate REPL but 
	/// put here for demonstrative purposes
	/// </summary>
	[TestFixture]
	public class TestDarkskyRequests : EmbeddedResourceTestBase
	{
		private Location _testLocation;
		private IWeatherProvider _weatherProvider;

		[OneTimeSetUp]
		public void Init()
		{
			_testLocation = new Location() { Latitude = 38.889931, Longitude = -77.0369 };

			var mockHttp = new Mock<IHttpRequestClient>();

			var dcDailyForecastJson = GetFromResource("dc-daily.json");
			mockHttp.Setup(m => m.GetContent(It.IsRegex("-77.0369"))).Returns(dcDailyForecastJson);
			_weatherProvider = new DarkSkyWeatherProvider(ConfigConstants.DarkSkyApiSecret, mockHttp.Object);
		}

		[Test]
		public void TestGetDailyForecast()
		{
			var forecast = _weatherProvider.GetForecast(_testLocation, DateTime.Now);
			Assert.IsNotNull(forecast);
		}
		
		[Test]
		public void TestSummary()
		{
			var forecast = _weatherProvider.GetForecast(_testLocation, DateTime.Now);
			Assert.IsTrue(forecast == "Partly cloudy starting in the evening.");
		}

		// SHORTCUT: Skip testing for time based weather requests, we'd need to mock out
		// the requests which doesn't really help testing the temporal component of the weather
		// requests as those are really mainly handled by DarkSky
	}
}
