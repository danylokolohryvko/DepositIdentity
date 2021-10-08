using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepositIdentity.IdentityServer
{
    public class IdentityConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("depositapi", "Deposit Api")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "blazor",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { "https://localhost:44325" },
                    AllowedScopes = { "openid", "profile", "depositapi"},
                    RedirectUris = { "https://localhost:44325/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:44325/" },
                    Enabled = true
                }
            };
    }
}
