# Identity Server 4

> [Início](../README.md)

[Protecting an API using Client Credentials](https://identityserver4.readthedocs.io/en/aspnetcore1/quickstarts/1_client_credentials.html)

### Criando o Identity Server 4

```sh
dotnet new web -f net6.0 -n is4
dotnet sln add is4
touch .prettierrc.json
```

### Configurando o servidor e as portas

📝 documentacao_identity_server_4/001_protegendo_api/is4/Properties/launchSettings.json

```json
{
	"profiles": {
		"is4": {
			"commandName": "Project",
			"dotnetRunMessages": true,
			"launchBrowser": true,
			"applicationUrl": "https://localhost:5001;http://localhost:5000",
			"environmentVariables": {
				"ASPNETCORE_ENVIRONMENT": "Development"
			}
		}
	}
}
```

### Instando os pacotes

📝 documentacao_identity_server_4/001_protegendo_api/is4/is4.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
	<PropertyGroup>
		<TargetFramework>net6.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="IdentityServer4" Version="4.1.1" />
	</ItemGroup>
</Project>
```

### Criando a configuração

```sh
cd is4
touch Config.cs
```

📝 documentacao_identity_server_4/001_protegendo_api/is4/Config.cs

```csharp
using IdentityServer4;
using IdentityServer4.Models;

public class Config
{
	public static IEnumerable<IdentityResource> IdentityResources =>
		new IdentityResource[]
		{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
		};
	public static IEnumerable<ApiResource> ApiResources =>
		new ApiResource[]
			{
				new ApiResource("myApi")
				{
					Scopes = new List<string>{ "myApi.read","myApi.write" },
					ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
				}
			};
	public static IEnumerable<ApiScope> ApiScopes =>
		new ApiScope[]
		{
			new ApiScope("myApi.read"),
			new ApiScope("myApi.write"),
		};
	public static IEnumerable<Client> Clients =>
		new Client[]
		{
				new Client
				{
					ClientId = "client",

                    // Sem usuario interativo, use o cliente/secreto para autenticacao
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // segredo para autenticacao
                    ClientSecrets =
					{
						new Secret("secret".Sha256())
					},

                    // escopos a que o cliente tem acesso a
					AllowedScopes = { "myApi.read" }
				},

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

                // Cliente de fluxo hibrido OpenID Connect (MVC)
                new Client
				{
					ClientId = "mvc",
					ClientName = "MVC Client",
					AllowedGrantTypes = GrantTypes.Code,

					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},

					RedirectUris           = { "https://localhost:7088/signin-oidc" },
					PostLogoutRedirectUris = { "https://localhost:7088/signout-callback-oidc" },

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"myApi.read"
					},

					AllowOfflineAccess = true,
					RequirePkce = true,
				}
		};
}
```

### Configurando o Program.cs

📝 documentacao_identity_server_4/001_protegendo_api/is4/Program.cs

```csharp

// Configurações do Identity Server
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

	// certificado de assinatura temporário usado para assinar tokens
	// Desenvolvimento
	.AddDeveloperSigningCredential()
	// Produção
	//.AddSigningCredential()

	.AddInMemoryIdentityResources(Config.IdentityResources)
	.AddInMemoryApiResources(Config.ApiResources)
	.AddInMemoryApiScopes(Config.ApiScopes)
	.AddInMemoryClients(Config.Clients);

// ativa os controladores com visualizadores(views)
builder.Services.AddControllersWithViews();
```

```csharp
// ativa o https
app.UseHttpsRedirection();

// habilita os arquivos da pasta wwwroot
app.UseStaticFiles();

// ativa as rotas
app.UseRouting();

// Ativa o Identity Server
app.UseIdentityServer();

// [autorize] será verificado antes de ser executado
app.UseAuthorization();

// ativa a rota padrão
app.UseEndpoints(endpoints =>
{
	endpoints.MapDefaultControllerRoute();
});

//app.MapGet("/", () => "Hello World!");
```

### Instalando a interface do Identity Server

Clique no link [IdentityServer4.Quickstart.UI](https://github.com/IdentityServer/IdentityServer4.Quickstart.UI), baixe o arquivo zip depois extraia os itens:

-   📁 wwwroot
-   📁 Quickstart
-   📁 Views
-   📄 getmain.ps1
-   📄 getmain.sh

```sh
# Feche os aplicativos caso dê ocupado
taskkill -f -im devenv.exe
taskkill -f -im dotnet.exe
taskkill -f -im Code.exe
./getmain.sh
```

[https://localhost:5001/.well-known/openid-configuration](https://localhost:5001/.well-known/openid-configuration)

# API

### Criando a API

```sh
dotnet new webapi -f net6.0 -n api
dotnet sln add api
```

### Configurando o servidor e as portas

📝 documentacao_identity_server_4/001_protegendo_api/api/Properties/launchSettings.json

```json
{
	"$schema": "https://json.schemastore.org/launchsettings.json",
	"profiles": {
		"api": {
			"commandName": "Project",
			"dotnetRunMessages": true,
			"launchBrowser": true,
			"launchUrl": "swagger",
			"applicationUrl": "https://localhost:5006;http://localhost:5005",
			"environmentVariables": {
				"ASPNETCORE_ENVIRONMENT": "Development"
			}
		}
	}
}
```

### Instalando os pacotes

📝 documentacao_identity_server_4/001_protegendo_api/api/api.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
	<PropertyGroup>
		<TargetFramework>net6.0</TargetFramework>
		<Nullable>enable</Nullable>
		<ImplicitUsings>enable</ImplicitUsings>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
		<PackageReference Include="IdentityServer4.AccessTokenValidation" Version="3.0.1" />
	</ItemGroup>
</Project>
```

### Criando a Controller protegida

```sh
cd api
touch Controllers/IdentityController.cs
```

📝 documentacao_identity_server_4/001_protegendo_api/api/Controllers/IdentityController.cs

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("identity")]
[Authorize]
public class Identity : ControllerBase
{
	[HttpGet]
	public IActionResult Get()
	{
		return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
	}
}
```

### Configurando o Program.cs

📝 documentacao_identity_server_4/001_protegendo_api/api/Program.cs

```csharp
builder.Services
	.AddAuthentication("Bearer")
	.AddIdentityServerAuthentication(
		"Bearer",
		options =>
		{
			options.ApiName = "myApi";
			options.Authority = "https://localhost:5001";
			// Permite sem https
			// options.RequireHttpsMetadata = false;
		}
	);
```

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### Adicionando o Authorization no Swagger

📝 documentacao_identity_server_4/001_protegendo_api/api/Program.cs

```csharp
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});
```

# Client 02 - Console

### Criando o Console

```sh
dotnet new console -f net6.0 -n Client2
dotnet sln add Client2
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

📝 documentacao_identity_server_4/001_protegendo_api/Client2/Program.cs

```csharp
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

var disco = await DiscoveryClient.GetAsync("https://localhost:5001");

// request token
var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
var tokenResponse = await tokenClient.RequestClientCredentialsAsync("myApi.read");

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

# Client 01 - Console

### Criando o Console

```sh
dotnet new console -f net6.0 -n Client
dotnet sln add Client
```

### Instalando os pacotes

📝 documentacao_identity_server_4/001_protegendo_api/Client/Client.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<PropertyGroup>
		<OutputType>Exe</OutputType>
		<TargetFramework>net6.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<Nullable>enable</Nullable>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="IdentityModel" Version="6.0.0" />
	</ItemGroup>
</Project>
```

### Criando a interface do Access Token

```sh
cd Client
touch IAccessToken.cs
```

📝 documentacao_identity_server_4/001_protegendo_api/Client/IAccessToken.cs

```csharp
namespace Client.AccessToken
{
	internal class IAccessToken
	{
		public string access_token { get; set; }
		public string expires_in { get; set; }
		public string token_type { get; set; }
		public string scope { get; set; }
	}
}
```

### Configurando o Program.cs

📝 documentacao_identity_server_4/001_protegendo_api/Client/Program.cs

```csharp
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
								Console.WriteLine(JArray.Parse(rs));
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
```

> [Início](../README.md)
