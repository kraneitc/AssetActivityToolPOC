using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AATShared
{
    public class ApiManagerService
    {
        public HttpClient Client { get; }

        public ApiManagerService(HttpClient client, DeveloperService devService, IHostEnvironment env, IConfiguration config)
        {

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config["api:key"]);

            // If DEV environment, get DEV OAuth token,
            // else get Managed Identity token
            var token = env.IsDevelopment() ?
                devService.GetDeveloperToken().Result :
                new AzureServiceTokenProvider().GetAccessTokenAsync(config["oauth:resource"]).Result;

            var auth = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = auth;

            Client = client;
        }
    }
}
