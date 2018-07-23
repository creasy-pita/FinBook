using System.Collections;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace User.Identity
{
    public class Config
    {

        public static IEnumerable<ApiResource> GetResource()
        {

            return new ApiResource[]{
                new ApiResource("gateway_api","gateway service"),
                new ApiResource("user_api","user service"),
                new ApiResource("project_api","project service"),
                new ApiResource("contact_api","contact service")
            };
        }

        public static IEnumerable<Client> GetClient()
        {

            return new Client[]{
                new Client{
                    ClientId="android",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowOfflineAccess=true,
                    RequireClientSecret=true,
                    AllowedGrantTypes=new List<string>{ "sms_auth_code"},
                    AlwaysIncludeUserClaimsInIdToken=true,
                    AllowedScopes = new List<string>{
                        "gateway_api",
                        "user_api",
                        "project_api",
                        "contact_api",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]{
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
    }
}