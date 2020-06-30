
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
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

            var svcObject = Type.GetType("ServiceSettings");
            var options = ActivatorUtilities.CreateInstance(builder.Services.BuildServiceProvider(), svcObject) as ServiceSettings;

            if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
            {
                X509Certificate2 devCert;
                using (var certStore = new X509Store(StoreLocation.LocalMachine))
                {
                    certStore.Open(OpenFlags.ReadOnly);
                    devCert = certStore.Certificates.Find(X509FindType.FindByThumbprint, options.DevCertThumbprint, true)[0];
                }

                builder.Services.AddHttpClient<DeveloperService>()
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        var handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(devCert);
                        return handler;
                    });
            }



            builder.Services.AddHttpClient<ApiManagerService>();
        }
    }
}
