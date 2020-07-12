using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AATShared
{
    public class ApiManagerService
    {
        public HttpClient Client { get; }

        public ApiManagerService(HttpClient client, IOptions<ServiceSettings> settings, ILogger<ApiManagerService> logger)
        {
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", settings.Value.ApiKey);
            var token =  new AzureServiceTokenProvider().GetAccessTokenAsync(settings.Value.Resource);
            
            var auth = new AuthenticationHeaderValue("bearer", token.Result);
            client.DefaultRequestHeaders.Authorization = auth;

            Client = client;
        }
    }
}
