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
