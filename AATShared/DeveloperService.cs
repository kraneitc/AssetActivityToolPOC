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
    public class DeveloperService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public DeveloperService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<string> GetDeveloperToken()
        {

            var response = await _client.RequestAuthorizationCodeTokenAsync(
                new AuthorizationCodeTokenRequest
                {
                    Address = _config["oauth:endpoint"],
                    ClientId = _config["oauth:client_id"],
                    ClientSecret = _config["oauth:client_secret"],
                    Parameters = new Dictionary<string, string>
                    {
                        { "resource", _config["oauth:resource"] }
                    }
                });

            return response.AccessToken;
        }
    }
}
