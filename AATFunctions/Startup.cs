
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

            builder.Services.AddOptions<ServiceSettings>()
                .Configure<IConfiguration>((settings, configuration) => {
                configuration.GetSection("ServiceSettings").Bind(settings);
            });

            builder.Services.AddTransient<DeveloperService>();
            builder.Services.AddHttpClient<ApiManagerService>();
        }
    }
}
