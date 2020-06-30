using System;
using System.Collections.Generic;
using System.Text;
using IdentityModel.Client;

namespace AATShared
{

    public class ServiceSettings
    {
        public AuthorizationCodeTokenRequest TokenRequest { get; set; }
        public string ApiKey { get; set; }
        public string DevCertThumbprint { get; set; }
    }
}
