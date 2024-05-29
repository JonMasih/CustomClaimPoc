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
using Newtonsoft.Json;



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
             var BodyReader = new StreamReader(req.Body);
            var reqbody = await BodyReader.ReadToEndAsync();
            
            TokenClaimsExtensionRequest data = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenClaimsExtensionRequest>(reqbody);

            if (data == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    string correlationId = data?.data.AuthenticationContext?.correlationId ?? string.Empty;
                    if (correlationId != null)
                    {
                        
 
                        // Read the correlation ID from the Microsoft Entra request    
                        // Claims to return to Microsoft Entra
                        ExtendedTokenClaimsResponse r = new ExtendedTokenClaimsResponse();
                        r.data.actions[0].claims.correlationId = correlationId;

                        //look up database for partner access



                        r.data.actions[0].claims.partnerName = "Belong health"; 
                        r.data.actions[0].claims.groupAccess.Add("490643");
                        r.data.actions[0].claims.lobAccess.Add("MC44");
                        r.data.actions[0].claims.tinAccess.Add("490643");
                        r.data.actions[0].claims.customRoles.Add("Reader");
                        r.data.actions[0].claims.customRoles.Add("Editor");
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(r);
                        return response;
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
