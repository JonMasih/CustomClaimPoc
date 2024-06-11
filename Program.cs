using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CustomClaimsPOC3.Models;
using CustomClaimsPOC3.Services;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using CustomClaimsPOC3.Middleware;
using Azure.Identity;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using System.Diagnostics.CodeAnalysis;


namespace CustomClaimsPOC3
{
    public class Program
    {
        [SuppressMessage("Usage", "AZFW0014:Missing expected registration of ASP.NET Core Integration services", Justification = "<Pending>")]
        public static async Task Main(string[] args)
        {
            IHost? host = null;

            host = new HostBuilder()
               .ConfigureFunctionsWorkerDefaults(worker =>
               {
                    worker.UseWhen<AuthenticationMiddleware>((context) => {
                        return !(context.FunctionDefinition.Name.ToLower().Contains("swagger") ||
                        context.FunctionDefinition.Name.ToLower().Equals("Function1")

                        );
                    });

                  //


               
                   // use that on any request
                   worker.UseNewtonsoftJson();
               })
                .ConfigureServices(services =>
                {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
                services.AddScoped<ITokenService, TokenService>();
                services.AddSingleton<ISecurityTokenValidator, JwtSecurityTokenHandler>();
                services.AddSingleton<IConfigurationManager<OpenIdConnectConfiguration>>(serviceProvider =>
                {
                    var configUrl = $"https://login.microsoftonline.com/684ff4ff-7382-4e5c-bfb7-376352c3ce5a/v2.0/.well-known/openid-configuration";
                    return new ConfigurationManager<OpenIdConnectConfiguration>(configUrl, new OpenIdConnectConfigurationRetriever());
                });
              })
             .ConfigureLogging(logging =>
             {
                 logging.Services.Configure<LoggerFilterOptions>(options =>
                 {
                     LoggerFilterRule defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName
                         == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
                     if (defaultRule is not null)
                     {
                         options.Rules.Remove(defaultRule);
                     }
                 });
             })
            .Build();



            //add logging for app insights 
            host.Run();
        }

    }
}