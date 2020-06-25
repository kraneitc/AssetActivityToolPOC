using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public static class SAPCloseoutRelay
    {
        [FunctionName("SAPCloseoutRelay")]
        public static void Run([QueueTrigger("sap-closeout-queue", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
