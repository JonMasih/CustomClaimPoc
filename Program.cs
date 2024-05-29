using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.ApplicationInsights.WorkerService;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
   // .ConfigureFunctionsWorkerDefaults(worker =>  worker.UseNewtonsoftJson())
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

//add logging for app insights 
host.Run();
