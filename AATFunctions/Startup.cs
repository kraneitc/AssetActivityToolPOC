
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AATShared;
using IdentityModel.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: FunctionsStartup(typeof(AATFunctions.Startup))]

namespace AATFunctions
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ServiceSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("ServiceSettings").Bind(settings);
            });

            builder.Services.AddHttpClient<DeveloperService>();
            builder.Services.AddHttpClient<ApiManagerService>();
        }
    }
}
