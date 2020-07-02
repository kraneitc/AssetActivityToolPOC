using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AATShared
{
    class QueueService
    {

        private static async Task<Response<SendReceipt>> QueueMessageAsync(string accountName, string queueName, string message)
        {
            var queueEndpoint = $"https://{accountName}.queue.core.windows.net/{queueName}";
            var queueClient = new QueueClient(new Uri(queueEndpoint), new DefaultAzureCredential());

            await queueClient.CreateIfNotExistsAsync();
            return await queueClient.SendMessageAsync(message);
        }
    }
}
