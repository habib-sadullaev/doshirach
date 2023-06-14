using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace Doshirach.IdentityServer
{
	public static class Config
	{
		public static IEnumerable<IdentityResource> IdentityResources =>
			 new IdentityResource[]
			 {
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				new ()
				{
					Name = "role",
					DisplayName = "User role",
					Description = "Your user role information",
					Emphasize = true,
					UserClaims = { "role", JwtClaimTypes.Role },
				}
			 };

		public static IEnumerable<ApiScope> ApiScopes =>
			 new ApiScope[]
			 {
				 new("api1", "My API") ,
			 };

		public static IEnumerable<Client> Clients =>
			new Client[] {
				new()
				{
					ClientId = "catalog",
					ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

					AllowedGrantTypes = GrantTypes.Code,

					RedirectUris = { "https://localhost:5242/signin-oidc" },
					FrontChannelLogoutUri = "https://localhost:5242/signout-oidc",
					PostLogoutRedirectUris = { "https://localhost:5242/signout-callback-oidc" },

					AllowOfflineAccess = true,
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"role",
						"api1",
					},
				},
			 };
	}
}