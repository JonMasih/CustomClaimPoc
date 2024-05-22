using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;
using Azure.Core;
using System.Net;
using Azure;
using Microsoft.AspNetCore.Http.Features;
using CustomClaimsPOC3.models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using System.Threading.Tasks;



namespace CustomClaimsPOC3
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {

            _logger.LogInformation("Started enriching token");

            //authorizeReqest token 
            //validate the access token send by Microsoft ENT id


            using var bodyReader = new StreamReader(req.Body);
            var reqBody = await bodyReader.ReadToEndAsync();



            if (reqBody == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    var tokenClaimsExtensionRequest = JsonSerializer.Deserialize<TokenClaimsExtensionRequest>(reqBody);
                

                    if (tokenClaimsExtensionRequest != null)
                    {
                        string correlationId = tokenClaimsExtensionRequest?.Data.AuthenticationContext?.CorrelationId ?? string.Empty;
                        if (correlationId != null)
                        {

                            ExtendedTokenClaimsResponse extendedTokenClaimsResponse = new ExtendedTokenClaimsResponse
                                {
                                Data = new()
                                {
                                    Actions = [
                                                 new()
                                                 {
                                                     Claims = new()
                                                     {
                                                         // Read the correlation ID from the Azure AD request    
                                                         CorrelationId = correlationId,
                                                         ApiVersion = "1.0.0",
                                                         DateOfBirth = "01/01/2000",
                                                         CustomRoles =
                                                            [
                                                                "Writer",
                                                                "Editor"
                                                            ]
                                                     }
                                                 }
                                                ]
                                }
                            };




                            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                            //response.Headers.Add("Content-Type", "application/json");
                            //response.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");
                            //response.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(extendedTokenClaimsResponse)));

                            await response.WriteAsJsonAsync(extendedTokenClaimsResponse);

                         //   _logger.LogInformation("End reqest, " + JsonSerializer.Serialize<HttpResponseData>(response));

                            return response;

                        }

                    }
                   return req.CreateResponse(HttpStatusCode.BadRequest);
                  
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Ex" + ex.Message);
                    HttpResponseData response = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await response.WriteAsJsonAsync(ex.Message);
                    return response;

                }

            }

        }

    }
}