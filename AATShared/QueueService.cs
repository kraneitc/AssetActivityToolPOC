using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AATShared
{
    public class QueueService
    {

        public static async Task<Response<SendReceipt>> QueueMessageAsync(string accountName, string queueName, string message)
        {
            var queueEndpoint = $"https://{accountName}.queue.core.windows.net/{queueName}";
            var queueClient = new QueueClient(new Uri(queueEndpoint), new DefaultAzureCredential());

            await queueClient.CreateIfNotExistsAsync();
            return await queueClient.SendMessageAsync(message);
        }
    }
}
