using Duende.IdentityServer.Models;

namespace Doshirach.IdentityServer
{
	public static class Config
	{
		public static IEnumerable<IdentityResource> IdentityResources =>
			 new IdentityResource[]
			 {
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
			 };

		public static IEnumerable<ApiScope> ApiScopes =>
			 new ApiScope[]
			 {
				new ApiScope("appScope"),
			 };

		public static IEnumerable<Client> Clients =>
			 new Client[]
			 {
            new Client
				{
					 ClientId = "catalog",
					 ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

					 AllowedGrantTypes = GrantTypes.Code,

					 RedirectUris = { "https://localhost:5242/signin-oidc" },
					 FrontChannelLogoutUri = "https://localhost:5242/signout-oidc",
					 PostLogoutRedirectUris = { "https://localhost:5242/signout-callback-oidc" },

					 AllowOfflineAccess = true,
					 AllowedScopes = { "openid", "profile", "appScope" }
				},

				new Client
				{
					 ClientId = "carting",
					 ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

					 AllowedGrantTypes = GrantTypes.Code,

					 RedirectUris = { "https://localhost:5005/signin-oidc" },
					 FrontChannelLogoutUri = "https://localhost:5005/signout-oidc",
					 PostLogoutRedirectUris = { "https://localhost:5005/signout-callback-oidc" },

					 AllowOfflineAccess = true,
					 AllowedScopes = { "openid", "profile", "appScope" }
				},
			 };
	}
}