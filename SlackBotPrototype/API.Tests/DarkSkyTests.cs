using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace API.Tests
{

	/// <summary>
	/// Not a unit test, normally would do this in separate REPL but 
	/// put here for demonstrative purposes
	/// </summary>
	[TestFixture]
	public class TestDarkskyRequests
	{
		private DarkSkyWeatherProvider _weatherProvider;

		[OneTimeSetUp]
		public void Init()
		{
			_weatherProvider = new DarkSkyWeatherProvider(ConfigConstants.DarkSkyApiSecret);
		}

		[Test]
		public void TestGetDailyForecast()
		{
		
			var forecast = _weatherProvider.GetForecast(DateTime.Now);
			Console.WriteLine(forecast);
			Assert.IsNotNull(forecast);
		}

		[TestCase("2017/06/01")]
		[TestCase("2017/06/05")]
		[TestCase("2017/06/10")]
		public void TestSummary(string dateStr)
		{
			var forecast = _weatherProvider.GetForecastRaw(DateTime.Parse(dateStr));
			Console.WriteLine(forecast.Daily.Data.FirstOrDefault()?.Summary);
			Assert.IsNotEmpty(forecast.Daily.Data.FirstOrDefault()?.Summary);
		}
	}
}
