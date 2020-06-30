using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace AATShared
{
    public class DeveloperService
    {
        private readonly HttpClient _client;

        public DeveloperService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetDeveloperToken(ServiceSettings settings)
        {
            var response = await _client.RequestAuthorizationCodeTokenAsync(settings.TokenRequest);
            return response.AccessToken;
        }
    }
}
