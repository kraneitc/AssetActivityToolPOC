using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Identity;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AATShared
{
    public class ApiManagerService
    {
        public HttpClient Client { get; }

        public ApiManagerService(HttpClient client, DeveloperService devService, IOptions<ServiceSettings> settings, IHostEnvironment env, ILogger<ApiManagerService> logger)
        {
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", settings.Value.ApiKey);

            // If DEV environment, get DEV OAuth token. Otherwise get Managed Identity token
            var isDev = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
            var token = isDev ?
                devService.GetDeveloperToken(settings.Value).Result :
                new AzureServiceTokenProvider().GetAccessTokenAsync(settings.Value.Resource).Result;

            var auth = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = auth;

            Client = client;
        }
    }
}
