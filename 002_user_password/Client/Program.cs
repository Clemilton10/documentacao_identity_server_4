using IdentityModel.Client;
using Newtonsoft.Json.Linq;

var disco = await DiscoveryClient.GetAsync("https://localhost:5001");

// request token
var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "Senha123!", "myApi.read");

if (tokenResponse.IsError)
{
	Console.WriteLine(tokenResponse.Error);
	return;
}

Console.WriteLine(tokenResponse.Json);

// call api
var client = new HttpClient();
client.SetBearerToken(tokenResponse.AccessToken);

var response = await client.GetAsync("https://localhost:5006/identity");
if (!response.IsSuccessStatusCode)
{
	Console.WriteLine(response.StatusCode);
}
else
{
	var content = await response.Content.ReadAsStringAsync();
	Console.WriteLine(JArray.Parse(content));
}