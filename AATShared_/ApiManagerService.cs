using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AATShared
{
    public class ApiManagerService
    {
        public HttpClient Client { get; }

        public ApiManagerService(HttpClient client, DeveloperService devService, IOptions<ServiceSettings> settings, IHostEnvironment env)
        {
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", settings.Value.ApiKey);

            // If DEV environment, get DEV OAuth token,
            // else get Managed Identity token
            var token = env.IsDevelopment() ?
                devService.GetDeveloperToken(settings.Value).Result :
                new AzureServiceTokenProvider().GetAccessTokenAsync(settings.Value.TokenRequest.Parameters["resource"]).Result;

            var auth = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = auth;

            Client = client;
        }
    }
}
