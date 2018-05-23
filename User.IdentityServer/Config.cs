using System.Collections;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace User.IdentityServer
{
    public class Config
    {

        public static IEnumerable<ApiResource> GetResource()
        {

            return new ApiResource[]{
                new ApiResource("api","myapi")
            };
        }

        public static IEnumerable<Client> GetClient()
        {

            return new Client[]{
                new Client{
                    ClientId="client",
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes = {"api"}
                },
                new Client{
                    ClientId="pwdClient",
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes = {"api"}
                }
            };
        }
        public static List<TestUser> GetTestUser()
        {

            return new List<TestUser>{
                new TestUser{
                    SubjectId="1",
                    Username="jesse",
                    Password="123456"
                }
            };
        }
    }
}