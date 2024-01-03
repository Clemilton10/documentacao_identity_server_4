# WebClient

> [Início](../README.md)

### Criando o MVC

```sh
dotnet new mvc -f net6.0 -n webclient
dotnet sln add webclient
```

### Instalando os pacotes

📝 documentacao_identity_server_4/003_webclient/webclient/webclient.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
	<PropertyGroup>
		<TargetFramework>net6.0</TargetFramework>
		<Nullable>enable</Nullable>
		<ImplicitUsings>enable</ImplicitUsings>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="Microsoft.AspNetCore.Authentication.Cookies" Version="2.2.0" />
		<PackageReference Include="Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="6.0.25" />
	</ItemGroup>
</Project>
```

### Configurando o servidor e as portas

📝 documentacao_identity_server_4/003_webclient/webclient/Properties/launchSettings.json

```json
{
	"profiles": {
		"webclient": {
			"commandName": "Project",
			"dotnetRunMessages": true,
			"launchBrowser": true,
			"applicationUrl": "https://localhost:5011;http://localhost:5010",
			"environmentVariables": {
				"ASPNETCORE_ENVIRONMENT": "Development"
			}
		}
	}
}
```

### Configurando o Program.cs

📝 documentacao_identity_server_4/003_webclient/webclient/Program.cs

```csharp
builder.Services
	.AddAuthentication(
		options =>
		{
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
		}
	)
	.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddOpenIdConnect(
		OpenIdConnectDefaults.AuthenticationScheme,
		options =>
		{
			options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
			options.Authority = "https://localhost:5001";
			options.ClientId = "mvc";
			options.ClientSecret = "secret";
			options.ResponseType = "code";
			options.SaveTokens = true;
			options.GetClaimsFromUserInfoEndpoint = true;
		}
	);
```

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### Configurando a HomeController.cs

📝 documentacao_identity_server_4/003_webclient/webclient/Controller/HomeController.cs

```csharp
[Authorize]
public IActionResult Privacy()
{
	return View();
}
```

### Criando as rotas de Login e Logout

```sh
cd webclient
touch Controllers/AccountController.cs
```

📝 documentacao_identity_server_4/003_webclient/webclient/Controller/AccountController.cs

```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult LogIn(string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.Content("~/");
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = redirectUri,
            },
            OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult LogOut(string redirectUri)
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "https://localhost:5011"
            },
            OpenIdConnectDefaults.AuthenticationScheme,
            CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
```

### Criando o link de Login e Logout

📝 documentacao_identity_server_4/003_webclient/webclient/Views/Shared/\_Layout.cshtml

```csharp
@if (User.Identity.IsAuthenticated)
{
	<li class="nav-item">
		<a class="nav-link text-dark" asp-area="" asp-controller="Account" asp-action="Logout">LogOut</a>
	</li>
}
else
{
	<li class="nav-item">
		<a class="nav-link text-dark" asp-area="" asp-controller="Account" asp-action="Login">Login</a>
	</li>
}
```

### Exibindo os dados do client logado

📝 documentacao_identity_server_4/003_webclient/webclient/Views/Home/Privacy.cshtml

```csharp
<dl>
    @foreach (var claim in User.Claims)
    {
        <dt>@claim.Type</dt>
        <dd>@claim.Value</dd>
    }
</dl>
```

> [Início](../README.md)
