using System.Collections.Generic;
using Newtonsoft.Json;

namespace API
{
	// Notes: Normally in web context, we'd have have separate domain
	// models for back end and front end. In the case of the chatbot, we  
	// have model objects for external services and model objects for our our
	// domain model (e.g. Users)

	public class ForecastItem
	{
		[JsonProperty("daily")]
		public DailyForecastItem Daily { get; set; }
	}

	public class DailyForecastItem
	{
		[JsonProperty("data")]
		public List<TemporalDataBlock> Data { get; set; }
	}

	public class TemporalDataBlock
	{
		[JsonProperty("summary")]
		public string Summary { get; set; }
		[JsonProperty("icon")]
		public string Icon { get; set; }
		[JsonProperty("apparentTemperatureMax")]
		public string ApparentTemperatureMax { get; set; }
	}
}
