using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Core;
using System.Net;
using Azure;
using Microsoft.AspNetCore.Http.Features;
using CustomClaimsPOC3.models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
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

            //Token in the headers 
            //validate
            _logger.LogInformation("Req headers" + req.Headers.ToString());




            //authorizeReqest token 
            //validate the access token send by Microsoft ENT id
            var BodyReader = new StreamReader(req.Body);
            var reqbody = await BodyReader.ReadToEndAsync();
            
            TokenClaimsExtensionRequest data = JsonConvert.DeserializeObject<TokenClaimsExtensionRequest>(reqbody);
            _logger.LogInformation("Request: " + JsonConvert.SerializeObject(data));
         
            _logger.LogInformation("Requst Body data ClientServicePrincipal.Id: " + data?.data.AuthenticationContext?.ClientServicePrincipal?.Id);
            _logger.LogInformation("Requst Body data ClientServicePrincipal.APPID: " + data?.data.AuthenticationContext?.ClientServicePrincipal?.AppId);
            _logger.LogInformation("Requst Body data ResourceServicePrincipal.Id: " + data?.data.AuthenticationContext?.ResourceServicePrincipal?.Id);
            _logger.LogInformation("Requst Body data ResourceServicePrincipal.APPID: " + data?.data.AuthenticationContext?.ResourceServicePrincipal?.AppId);
            _logger.LogInformation("Requst Body data USER.Id: " + data?.data.AuthenticationContext?.User?.Id);

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
                        //


                        r.data.actions[0].claims.partnerName = "Belong health"; 
                        r.data.actions[0].claims.groupAccess.Add("490643");
                        r.data.actions[0].claims.lobAccess.Add("MC44");
                        r.data.actions[0].claims.tinAccess.Add("490643");
                        r.data.actions[0].claims.customRoles.Add("Reader");
                        r.data.actions[0].claims.customRoles.Add("Editor");

                        
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(r);
                   
                        _logger.LogInformation("response: " + JsonConvert.SerializeObject(r));
                        return response;
                    }
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Ex " + ex.Message);
                    HttpResponseData response = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await response.WriteAsJsonAsync(ex.Message);
                    return response;

                }

            }
        }
    }

  }
