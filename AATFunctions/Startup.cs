
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using AATShared;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AATFunctions.Startup))]

namespace AATFunctions
{
    internal class Startup : FunctionsStartup
    {
        
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            // Bind settings to ServiceSettings object
            builder.Services.AddOptions<ServiceSettings>()
                .Configure<IConfiguration>((settings, configuration) => {
                configuration.GetSection("ServiceSettings").Bind(settings);
            });

            // Adds service for developers to authenticate to API Manager using 
            // their SAPN-issued PC client certificate. Export the public key certificate
            // and upload it to the AAT Developer Service Principal.
            builder.Services.AddTransient<DeveloperService>();

            // Creates a HttpClient service to interact with the API Manager
            builder.Services.AddHttpClient<ApiManagerService>();
        }
    }
}
