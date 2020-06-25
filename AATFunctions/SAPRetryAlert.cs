using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public static class SAPRetryAlert
    {
        [FunctionName("SAPRetryAlert")]
        public static void Run([QueueTrigger("sap-closeout-queue-poison", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
