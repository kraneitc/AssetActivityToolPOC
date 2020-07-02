using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public static class AATRetryAlert
    {
        [FunctionName("SAPRetryAlert")]
        public static void SAPRetryAlert([QueueTrigger("sap-closeout-queue-poison")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("ClickRetryAlert")]
        public static void ClickRetryAlert([QueueTrigger("sap-closeout-queue-poison")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
