using System;
using System.Linq;
using Newtonsoft.Json;

namespace API
{

	public class DarkSkyWeatherProvider : IWeatherProvider
	{
		private readonly string _apiSecret;
		private readonly IHttpRequestClient _client;

		public DarkSkyWeatherProvider(string apiSecret, IHttpRequestClient client)
		{
			_apiSecret = apiSecret;
			_client = client;
		}

		public string GetForecast(Location location, DateTime when)
		{
			var forecastItem = GetForecastRaw(location, when);
			return forecastItem.Summary;
		}

		public ChatBotForecast GetForecastRaw(Location location, DateTime when)
		{
			
			var timeStamp = when.DateTimeToUnixTimestamp();
			// SHORTCUT: We could cache requests in memory for recent forecast requests so we don't have
			// to keep making network calls and improve performance. The tricky part would be
			// figuring out what's the right invalidation scheme. It would probably involve the time 
			// returned in each request, location, and UTC time. Skipping it for now.
			var content = _client.GetContent($"/forecast/{_apiSecret}/{location.Latitude},{location.Longitude},{(int)timeStamp}?exclude=currently,minutely,hourly,flags");
			var forecastItem = JsonConvert.DeserializeObject<ForecastItem>(content);
			
			return new ChatBotForecast
			{
				Location = location,
				Icon = forecastItem.Daily.Data.FirstOrDefault()?.Icon,
				Summary = forecastItem.Daily.Data.FirstOrDefault()?.Summary
			};
		}
	}
}
