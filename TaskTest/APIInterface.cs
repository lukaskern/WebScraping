namespace TaskTest
{
	public static class APIInterface
	{
		private const string DefaultUrl = "https://c2.scryfall.com";

		private static HttpClient Client = new HttpClient();

		#region Sync Methods
		public static HttpResponseMessage Get(string apiUrl, params string[] parameters)
		{
			return Client.GetAsync(CreateUri(apiUrl, DefaultUrl, parameters)).Result;
		}

		public static HttpResponseMessage Post(string apiUrl, HttpContent body = null, params string[] parameters)
		{
			return Client.PostAsync(CreateUri(apiUrl, DefaultUrl, parameters), body).Result;
		}
		#endregion

		#region Async Methods
		public static async Task<HttpResponseMessage> GetAsync(string apiUrl, params string[] parameters)
		{
			return await Client.GetAsync(CreateUri(apiUrl, DefaultUrl, parameters));
		}

		public static async Task<HttpResponseMessage> PostAsync(string apiUrl, HttpContent body = null, params string[] parameters)
		{
			return await Client.PostAsync(CreateUri(apiUrl, DefaultUrl, parameters), body);
		}
		#endregion

		//Baut die API-URL + die Parameter zusammen
		//https://api.com + /pfad/zur/api/schnittstelle + ?param1=123&param2=456&param3=abc
		private static Uri CreateUri(string apiUrl, string url = DefaultUrl, params string[] par)
		{
			string full = url + apiUrl + (par.Length != 0 ? "?" : "");
			string req = string.Join("&", par);
			return new Uri(full + req);
		}
	}
}
