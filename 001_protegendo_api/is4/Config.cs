﻿using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

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
				// 001_protegendo_api
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

				// 002_user_password
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

				// 003_webclient
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

	// 002_user_password & 003_webclient
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
}