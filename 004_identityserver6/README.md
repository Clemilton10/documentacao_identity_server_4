# Identity Server

> [Início](../README.md)

### Meus links

[IdentityServer.Templates](../IdentityServer.Templates.md) | [IdentityServer4.Templates](../IdentityServer4.Templates.md)

### Duende Software

[https://github.com/duendesoftware](https://github.com/duendesoftware)

### IdentityServer.Templates

[https://github.com/DuendeSoftware/IdentityServer.Templates](https://github.com/DuendeSoftware/IdentityServer.Templates)

### Documentação

[https://docs.duendesoftware.com/identityserver/v6/samples/](https://docs.duendesoftware.com/identityserver/v6/samples/)

### Criação do projeto

```sh
dotnet new web -f net6.0 -n isaspid
dotnet sln add isaspid
cd isaspid
touch .prettierrc.json
```

### Instalação dos templates

```sh
dotnet new install Duende.IdentityServer.Templates
```

### Instalação do Identity Server na pasta do projeto

```sh
dotnet new isaspid --force
```

### Instalando os pacotes

📝 documentacao_identity_server_4/004_identityserver6/isaspid/isaspid.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
	<PropertyGroup>
		<TargetFramework>net6.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="Duende.IdentityServer.AspNetIdentity" Version="6.3.2" />
		<PackageReference Include="Microsoft.AspNetCore.Authentication.Google" Version="6.0.0" />
		<PackageReference Include="Serilog.AspNetCore" Version="6.0.0" />
		<PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="6.0.0" />
		<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="6.0.0" />
		<PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="6.0.0" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="6.0.0" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.0" />
	</ItemGroup>
</Project>
```

### Configurando o servidor e as portas

📝 documentacao_identity_server_4/004_identityserver6/isaspid/Properties/launchSettings.json

```json
{
	"profiles": {
		"isaspid": {
			"commandName": "Project",
			"launchBrowser": true,
			"environmentVariables": {
				"ASPNETCORE_ENVIRONMENT": "Development"
			},
			"applicationUrl": "https://localhost:5001;http://localhost:5000"
		}
	}
}
```

### Config.cs

📝 documentacao_identity_server_4/004_identityserver6/isaspid/Config.cs

```csharp
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace is4admin;

public static class Config
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
			// m2m client credentials flow client
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
			new Client
			{
				ClientId = "mvc",
				ClientName = "MVC Client",
				AllowedGrantTypes = GrantTypes.Code,

				ClientSecrets =
				{
					new Secret("secret".Sha256())
				},

				RedirectUris           = { "https://localhost:5011/signin-oidc" },
				PostLogoutRedirectUris = { "https://localhost:5011/signout-callback-oidc" },

				AllowedScopes =
				{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
				},

				AllowOfflineAccess = true,
				RequirePkce = false,
			}
		};
}
```

> [Início](../README.md)
