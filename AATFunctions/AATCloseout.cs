using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public class AATCloseout
    {

        [FunctionName("SAPCloseoutRelay")]
        public static void SAPCloseoutRelay([QueueTrigger("sap-closeout-queue")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("ClickCloseoutRelay")]
        public static void ClickCloseoutRelay([QueueTrigger("sap-closeout-queue")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
