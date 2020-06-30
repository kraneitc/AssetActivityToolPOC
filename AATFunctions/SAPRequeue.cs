using System;
using System.IO;
using System.Threading.Tasks;
using AATShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace AATFunctions
{
    public class SAPRequeue
    {
        private readonly ApiManagerService _apiManagerService;

        public SAPRequeue(ApiManagerService apiManagerService)
        {
            _apiManagerService = apiManagerService;
        }

        [FunctionName("SAPRequeue")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var responseMessage = await _apiManagerService.Client.GetAsync("https://sapn-enterpriseapim-poc2-ae-api.azure-api.net/sap-closeout/closeout");

            return new OkObjectResult(responseMessage.IsSuccessStatusCode);
        }
    }
}
