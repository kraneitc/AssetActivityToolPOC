using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public static class ClickCloseoutRelay
    {
        [FunctionName("ClickCloseoutRelay")]
        public static void Run([QueueTrigger("click-closeout-queue", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
