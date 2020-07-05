using System.Threading.Tasks;
using AATShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace AATFunctions
{
    public class TestFunction
    {
        private readonly ApiManagerService _apiManagerService;
        private readonly QueueService _queueService;

        public TestFunction(ApiManagerService apiManagerService, QueueService queueService)
        {
            _apiManagerService = apiManagerService;
            _queueService = queueService;
        }

        [FunctionName("APIMTest")]
        public async Task<IActionResult> APIMTest(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var responseMessage = await _apiManagerService.Client.GetAsync(
                "https://sapn-enterpriseapim-poc2-ae-api.azure-api.net/sap-closeout/closeout");

            return new OkObjectResult(await responseMessage.Content.ReadAsStringAsync());
            //return new OkObjectResult(responseMessage.IsSuccessStatusCode);
        }


        [FunctionName("QueueTest")]
        public async Task<IActionResult> QueueTest(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var result = await _queueService.QueueMessageAsync(
                "pocaatsa", "aatqueue", "Message from AAT Function");

            return new OkObjectResult(result.Value.InsertionTime);
        }
    }
}