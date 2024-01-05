using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer.Web
{
    public static class Config
    {

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new("IdentityWebApi", "Identity Web Api")
        };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new("IdentityWebApi", "Identity Web Api",
                new[]{JwtClaimTypes.Name})
            {
                Scopes = { "IdentityWebApi" }
            }
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new ()
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "api1" }
            },
            new() {
                ClientId = "epclient",
                ClientName = "Education Platform Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                AllowOfflineAccess = true,
                RedirectUris =
                {
                    "https://localhost:3000/signin-oidc"
                },
                AllowedCorsOrigins =
                {
                    "https://localhost:3000"
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:3000/signout-oidc"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "IdentityWebApi"
                },
                AllowAccessTokensViaBrowser = true
            }
        };

    }
}
