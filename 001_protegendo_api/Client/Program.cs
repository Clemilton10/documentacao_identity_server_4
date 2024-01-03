using Client.AccessToken;
using Newtonsoft.Json;
using System.Net.Http.Headers;

class Program
{
	private
	static async Task Main()
	{
		string tokenEndpoint = "https://localhost:5001/connect/token";
		string grantType = "client_credentials";
		string clientId = "client";
		string clientSecret = "secret";
		string scope = "myApi.read";

		string requestBody = $"grant_type={grantType}&scope={scope}&client_id={clientId}&client_secret={clientSecret}";
		using (var httpClient = new HttpClient())
		{
			var content = new StringContent(
				requestBody,
				System.Text.Encoding.UTF8,
				"application/x-www-form-urlencoded"
			);

			var rp = await httpClient.PostAsync(tokenEndpoint, content);

			if (rp.IsSuccessStatusCode)
			{
				var rs = await rp.Content.ReadAsStringAsync();
				if (rs != null)
				{
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine(rs);
					var obj = JsonConvert.DeserializeObject<IAccessToken>(rs);
					if (obj != null)
					{
						Console.WriteLine();
						Console.WriteLine();
						Console.WriteLine(obj.access_token);
						httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", obj.access_token);
						tokenEndpoint = "https://localhost:5006/identity";
						rp = await httpClient.GetAsync(tokenEndpoint);
						if (rp.IsSuccessStatusCode)
						{
							rs = await rp.Content.ReadAsStringAsync();
							if (rs != null)
							{
								Console.WriteLine();
								Console.WriteLine();
								Console.WriteLine(rs);
							}
						}
						else
						{
							Console.WriteLine($"Erro na solicitação: {rp.StatusCode}");
							var rs2 = await rp.Content.ReadAsStringAsync();
							Console.WriteLine($"Erro: {rs2}");
						}
					}
				}
			}
			else
			{
				Console.WriteLine($"Erro na solicitação: {rp.StatusCode}");
				var rs = await rp.Content.ReadAsStringAsync();
				Console.WriteLine($"Erro: {rs}");
			}
		}
	}
}