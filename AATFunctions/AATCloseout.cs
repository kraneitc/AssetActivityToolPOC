using System.Threading.Tasks;
using AATShared;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public static class AATCloseout
    {
        [FunctionName("SAPCloseoutRelay")]
        public static async Task SAPCloseoutRelay([QueueTrigger("sap-closeout-queue")] string myQueueItem, ILogger log)
        {
            var result = await QueueService.QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT Function");

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("ClickCloseoutRelay")]
        public static async Task ClickCloseoutRelay([QueueTrigger("sap-closeout-queue")] string myQueueItem, ILogger log)
        {
            var result = await QueueService.QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT Function");

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
