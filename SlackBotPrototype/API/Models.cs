using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API
{

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
