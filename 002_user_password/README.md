# Usuario & Senha

> [Início](../README.md)

### Adicionando os usuários na Config.cs

📝 documentacao_identity_server_4/001_protegendo_api/is4/Config.cs

```csharp
public static IEnumerable<Client> Clients =>
	new Client[]
	{
		// Cliente de concessao de senha do proprietario do recurso
		new Client
		{
			ClientId = "ro.client",
			AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

			ClientSecrets =
			{
				new Secret("secret".Sha256())
			},
			AllowedScopes = { "myApi.read" }
		},
	};
public static List<TestUser> Users =>
	new List<TestUser>
	{
		new TestUser
		{
			SubjectId = "1",
			Username = "alice",
			Password = "Senha123!"
		},
		new TestUser
		{
			SubjectId = "2",
			Username = "bob",
			Password = "Senha123!"
		}
	};
```

### Reconfigurando a Program.cs

📝 documentacao_identity_server_4/001_protegendo_api/is4/Program.cs

```csharp
builder.Services
	.AddIdentityServer(
		options =>
		{
			options.Events.RaiseErrorEvents = true;
			options.Events.RaiseInformationEvents = true;
			options.Events.RaiseFailureEvents = true;
			options.Events.RaiseSuccessEvents = true;
			options.EmitStaticAudienceClaim = true;
		}
	)
	.AddDeveloperSigningCredential()
	.AddInMemoryIdentityResources(Config.IdentityResources)
	.AddInMemoryApiResources(Config.ApiResources)
	.AddInMemoryApiScopes(Config.ApiScopes)
	.AddInMemoryClients(Config.Clients)

	// acrescente esta linha
	.AddTestUsers(Config.Users);
```

# Client - Console

### Criando o Console

```sh
dotnet new console -f net6.0 -n Client
dotnet sln add Client
```

### Instalando os pacotes

📝 documentacao_identity_server_4/001_protegendo_api/Client2/Client2.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<PropertyGroup>
		<OutputType>Exe</OutputType>
		<TargetFramework>net6.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<Nullable>enable</Nullable>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="IdentityModel" Version="3.3.1" />
	</ItemGroup>
</Project>
```

### Configurando o Program.cs

📝 documentacao_identity_server_4/002_user_password/Client/Program.cs

```csharp
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
```

> [Início](../README.md)
