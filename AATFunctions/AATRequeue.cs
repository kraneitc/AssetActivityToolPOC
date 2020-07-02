using System.Threading.Tasks;
using AATShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace AATFunctions
{
    public class AATRequeue
    {
        private readonly ApiManagerService _apiManagerService;

        public AATRequeue(ApiManagerService apiManagerService)
        {
            _apiManagerService = apiManagerService;
        }

        [FunctionName("SAPRequeue")]
        public async Task<IActionResult> SAPRequeue(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var responseMessage = await _apiManagerService.Client.GetAsync("https://sapn-enterpriseapim-poc2-ae-api.azure-api.net/sap-closeout/closeout");

            var result = await QueueService.QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT Function");

            return new OkObjectResult(responseMessage.IsSuccessStatusCode);
        }


        [FunctionName("ClickRequeue")]
        public async Task<IActionResult> ClickRequeue(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var responseMessage = await _apiManagerService.Client.GetAsync("https://sapn-enterpriseapim-poc2-ae-api.azure-api.net/sap-closeout/closeout");

            var result = await QueueService.QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT Function");

            return new OkObjectResult(result.Value.InsertionTime);
        }
    }
}
