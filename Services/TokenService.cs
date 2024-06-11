
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using HttpRequestData = Microsoft.Azure.Functions.Worker.Http.HttpRequestData;
using System.Collections.Generic;
using CustomClaimsPOC3.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;


namespace CustomClaimsPOC3.Services
{
    public class TokenService : ITokenService
    {

        private readonly IConfigurationManager<OpenIdConnectConfiguration> openIdConfigManager;
        private readonly ISecurityTokenValidator securityTokenHandler;
        private readonly ILogger<TokenService> logger;
        // private readonly IConfidentialClientApplication apimClientApp;
        private readonly MemoryCache cachedAuthResults;

        //  public TokenService(IConfigurationManager<OpenIdConnectConfiguration> openIdConfigManager, ISecurityTokenValidator securityTokenHandler, IConfidentialClientApplication _apimClientApp, ILogger<TokenService> logger)
        /// <summary>
        ///   {
        /// </summary>
        /// <param name="openIdConfigManager"></param>
        /// <param name="securityTokenHandler"></param>
        /// <param name="_apimClientApp"></param>
        /// <param name="logger"></param>

        public TokenService(IConfigurationManager<OpenIdConnectConfiguration> openIdConfigManager, ISecurityTokenValidator securityTokenHandler,ILogger<TokenService> logger)
        {
            cachedAuthResults = new MemoryCache(new MemoryCacheOptions());
            this.openIdConfigManager = openIdConfigManager;
            this.securityTokenHandler = securityTokenHandler;
            this.logger = logger;
            //  apimClientApp = _apimClientApp;
        }

        public async Task<TokenClaims> ValidateToken(HttpRequestData request)
        {
            try
            {
                request.Headers.TryGetValues("Authorization", out IEnumerable<string> bearer);

                // unit test here for malformed token? "Authorization: bearer" does not throw. but it validates "bearer" so do I care
                string bearerToken = bearer.First().Split(' ').Last();
                
                
                //TODO where would we do this? if we have a list of clients
                string applicationIdOne = "cecccadd-5a9c-4b54-8e0b-a220d18de16a";
                string applicationIdTwo = "b6eb64df-e207-40b3-953d-8afa2241d7b9";



                List<string> validAudences = new List<string>() { applicationIdOne,
                    applicationIdTwo
                };
              /**
               *We are getting the metadata from the authorization server.Because, 
               *instead of keeping the sign key in the local configuration, will get from the authorization server using metadata endpoint.
               *The following endpoints return OpenID Connect or OAuth 2.0 metadata related to your Org Authorization Server.
               **/
                var wellKnownConfig = await openIdConfigManager.GetConfigurationAsync(CancellationToken.None);
                var tokenParams = new TokenValidationParameters()
                {
                RequireSignedTokens = true,  //It is indicating whether a SecurityToken can be considered valid if not signed.
                    ValidateAudience = true,  // It is audience value that will be used to check against the token's audience
                    ValidAudiences = validAudences, //Validate the recipients of the token
                    ValidateIssuer = true, //Validate the server where generates the token
                    ValidIssuer = wellKnownConfig.Issuer,  // It is the issuer value that will be used to check against the token's issuer  CONFIG
                    ValidateIssuerSigningKey = true,   //Validate signature of the token 
                    IssuerSigningKeys = wellKnownConfig.SigningKeys,   // Validate signature of the token
                    ValidateLifetime = true,  //Check if the token has expired or not
                };

                var claimsPrincipal = securityTokenHandler.ValidateToken(bearerToken, tokenParams, out SecurityToken securityToken);

                return new TokenClaims(claimsPrincipal);
            }
            catch (Exception ex)
            {
                logger.LogInformation("Token validation failed " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public void AddToContext(FunctionContext context, TokenClaims claims)
        {
            context.Items.Add(ContextItemKeys.JwtTokenClaims, claims);
        }

        public TokenClaims GetClaims(FunctionContext context)
        {

            try
            {
                context.Items.TryGetValue(ContextItemKeys.JwtTokenClaims, out object tokenClaim);
                TokenClaims claims = (TokenClaims)tokenClaim;

                return claims;
            }
            catch
            {
                throw new ArgumentNullException(ContextItemKeys.JwtTokenClaims.ToString());
            }
        }

   
    }
}