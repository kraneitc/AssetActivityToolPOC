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

            builder.Services.AddSingleton<QueueService>();

            // Creates a HttpClient service to interact with the API Manager
            builder.Services.AddHttpClient<ApiManagerService>();
        }
    }
}
