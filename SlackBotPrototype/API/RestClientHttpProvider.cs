using System.Net;
using RestSharp;

namespace API
{
	public class RestClientHttpProvider : IHttpRequestClient
	{
		private readonly RestClient _client;

		public RestClientHttpProvider(string baseUrl)
		{
			_client= new RestClient(baseUrl);
		}
		
		public string GetContent(string url)
		{
			var resp = _client.Get(new RestRequest(Method.GET)
			{
				// To save time, hard code location
				Resource = url
			});

			return resp.StatusCode == HttpStatusCode.OK ? resp.Content : null;
		}
	}
}
