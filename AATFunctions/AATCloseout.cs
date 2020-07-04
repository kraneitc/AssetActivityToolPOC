using System.Collections.Generic;
using System.Threading.Tasks;
using AATShared;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public class AATCloseout
    {
        private readonly QueueService _queueService;

        public AATCloseout(QueueService queueService)
        {
            _queueService = queueService;
        }

        [FunctionName("SAPCloseoutRelay")]
        public async Task SAPCloseoutRelay([QueueTrigger("sap-closeout-queue")] string myQueueItem, ILogger log)
        {
            var result = await _queueService.QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT Function");

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("ClickCloseoutRelay")]
        public async Task ClickCloseoutRelay([QueueTrigger("sap-closeout-queue")] string myQueueItem, ILogger log)
        {
            var result = await _queueService.QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT Function");

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
