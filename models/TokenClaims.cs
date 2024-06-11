using System.Security.Claims;
using System.Linq;
using System;
using System.Data;
using System.Security.Cryptography;

namespace CustomClaimsPOC3.Models
{
 

    public class TokenClaims
    {
        public string aud { get; set; }
        public string iss { get; set; }
        public int iat { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public string aio { get; set; }
        public string azp { get; set; }
        public string azpacr { get; set; }
        public string oid { get; set; }
        public string rh { get; set; }
        public string sub { get; set; }
        public string tid { get; set; }
        public string uti { get; set; }
        public string ver { get; set; }
        public TokenClaims(ClaimsPrincipal claimsPrincipal)
        {
            aud = GetClaim(claimsPrincipal, "aud");   // need to change when added to jwt
            iss = GetClaim(claimsPrincipal, "iss");
            aio = GetClaim(claimsPrincipal, "azp");
            azp = GetClaim(claimsPrincipal, "azp");
        }

        private string GetClaim(ClaimsPrincipal claimsPrincipal, string key)
        {
            return claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == key)?.Value ?? throw new ArgumentNullException(key);
        }

    }
}
