using System;
using System.Threading.Tasks;
using AATShared;
using Azure;
using Azure.Identity;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AATFunctions
{
    public static class SAPCloseoutRelay
    {
        [FunctionName("SAPCloseoutRelay")]
        public static async Task Run([QueueTrigger("sap-closeout-queue", Connection = "")]string myQueueItem, ILogger log)
        {
            var result = await QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT API");

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        public static async Task<Response<SendReceipt>> QueueMessageAsync(string accountName, string queueName, string message)
        {
            var queueEndpoint = $"https://{accountName}.queue.core.windows.net/{queueName}";
            var queueClient = new QueueClient(new Uri(queueEndpoint), new DefaultAzureCredential());

            await queueClient.CreateIfNotExistsAsync();
            return await queueClient.SendMessageAsync(message);
        }
    }
}
