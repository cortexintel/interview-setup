using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
	/// <summary>
	/// Internal interface to decouple weather service
	/// </summary>
	public interface IWeatherProvider
	{
		string GetForecast(DateTime when);
		string GetForecastWarning();
	}

	public interface IMessageParser
	{
		string HandleRequest(string message);
	}
}
