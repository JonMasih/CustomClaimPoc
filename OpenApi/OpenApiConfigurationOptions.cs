using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;

namespace CustomClaimsPOC3.OpenApi
{
    // This class is required so that the ProvisionAPIM deployment task has the required fields
    internal class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "2.0.0",
            Title = "MVP Custom Claims API",
            Description = "MVP Custom Claims API",
           
        };
    }
}
