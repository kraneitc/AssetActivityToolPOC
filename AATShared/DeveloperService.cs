using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace AATShared
{
    public class DeveloperService
    {

        public async Task<string> GetDeveloperToken(ServiceSettings settings)
        {

            X509Certificate2 devCert;
            using (var certStore = new X509Store(StoreLocation.LocalMachine))
            {
                certStore.Open(OpenFlags.ReadOnly);
                devCert = certStore.Certificates.Find(X509FindType.FindByThumbprint, settings.DevCertThumbprint, true)[0];
            }

            var app = ConfidentialClientApplicationBuilder.Create(settings.ClientId)
                .WithCertificate(devCert)
                .WithAuthority(new Uri(settings.Authority))
                .Build();

            var response = await app.AcquireTokenForClient(new []{settings.Scope} )
                .ExecuteAsync();

            return response.AccessToken;
        }
    }
}
