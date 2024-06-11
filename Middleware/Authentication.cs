
using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using CustomClaimsPOC3.Models;
using Azure.Core;

namespace CustomClaimsPOC3.Middleware
{
    public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ITokenService tokenService;

         private readonly ILogger<AuthenticationMiddleware> _logger;
        public AuthenticationMiddleware(ITokenService tokenService, ILogger<AuthenticationMiddleware> logger)
        {
            this.tokenService = tokenService;
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
           HttpRequestData request = await context.GetHttpRequestDataAsync();
            ILogger logger = _logger;

            logger.LogInformation($"[{nameof(AuthenticationMiddleware)}]: attempting token validation");
            logger.LogInformation($"[{nameof(AuthenticationMiddleware)}]: attempting token validation Token:" + request.Headers.ToString());

            try
            {
                TokenClaims claims = await tokenService.ValidateToken(request);

                tokenService.AddToContext(context, claims);

                logger.LogInformation($"[{nameof(AuthenticationMiddleware)}]: token validation successful");
                await next(context);

                //Run

            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                var response = request.CreateResponse(HttpStatusCode.Unauthorized);
                response.WriteString(e.Message);
                context.GetInvocationResult().Value = response;
            }
        }
    }
}