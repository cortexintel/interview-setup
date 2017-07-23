using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace API
{
	public class DarkSkyWeatherProvider : IWeatherProvider
	{
		private readonly string _apiSecret;
		private readonly RestClient _client = new RestClient("https://api.darksky.net");

		public DarkSkyWeatherProvider(string apiSecret)
		{
			_apiSecret = apiSecret;
		}

		public string GetForecast(DateTime when)
		{
			var forecastItem = GetForecastRaw(when);
			return forecastItem.Daily.Data.FirstOrDefault()?.Summary;
		}

		public string GetForecastWarning()
		{
			var today = GetForecastRaw(DateTime.Now);
			var yesterday = GetForecastRaw(DateTime.Now.AddDays(-1));

			// Darksky recommends using "icon" for automated purposes and "summary" for human readable output
			return today.Daily.Data.FirstOrDefault()?.Icon != yesterday.Daily.Data.FirstOrDefault()?.Icon 
				? today.Daily.Data.FirstOrDefault()?.Summary 
				: string.Empty;
		}

		public ForecastItem GetForecastRaw(DateTime when)
		{
			var timeStamp = when.DateTimeToUnixTimestamp();
			var resp = _client.Get(new RestRequest(Method.GET)
			{
				// To save time, hard code location
				Resource = $"/forecast/{_apiSecret}/38.9072,-77.0369,{(int)timeStamp}?exclude=currently,minutely,hourly,flags"
			});

			if (resp.StatusCode != HttpStatusCode.OK)
			{
				return null;
			}

			var content = resp.Content;
			var forecastItem = JsonConvert.DeserializeObject<ForecastItem>(content);
			return forecastItem;
		}
	}
}
