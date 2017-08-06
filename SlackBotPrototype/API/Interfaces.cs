using System;
using System.Collections.Generic;

namespace API
{
	/// <summary>
	/// Internal interface to decouple weather service
	/// </summary>
	public interface IWeatherProvider
	{
		string GetForecast(Location location, DateTime when);
		ChatBotForecast GetForecastRaw(Location location, DateTime when);
	}

	public interface IMessageToCommandConverter
	{
		MessageParserResponse HandleRequest(string slackId, string message);
	}

	public interface IHttpRequestClient
	{
		/// <summary>
		/// Makes an http GET request and returns the content. 
		///  
		/// Normally we encapsulate the whole response object in a DTO object so we can handle different HTTP status
		/// codes and error conditions. To save time, we're only going to handle HTTP Status = 200, other other status codes
		/// will return no content.
		/// </summary>
		/// <param name="url">the url to resource to request</param>
		/// <returns>the content of the response if successful otherwise <code>null</code></returns>
		string GetContent(string url);
	}

	/// <summary>
	/// Define a interface for user persistence, in a production app the de-coupling
	/// this allows us to swap ersistence strategies as needed from in-memory object, flat file,
	/// or whatever DB desired
	/// </summary>
	public interface IRepository
	{
		User Get(string slackId);
		Location GetLocation(string city, string state);
		Location GetLocation(int id);
		int CreateOrUpdate(string name, string slackId, Location location = null);

		// Shortcut: skipping some CRUD
	}

	public interface ICommandImpl
	{
		// Invoke the command, expects a single string result.
		// This could be changed later as needed to a more complex object.
		string Invoke();
	}

	/// <summary>
	/// Define an interface for a service that will provide location information
	/// </summary>
	public interface ILocationService
	{
		Location GetLocation(string city, string stateAbbreviation);
		Location GetLocation(int id);
	}

}
