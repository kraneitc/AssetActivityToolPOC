using System;
using AATWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AATShared;
using Azure;
using Azure.Identity;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AATWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly AATDbContext _context;

        public QueueController(AATDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var result = await QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT API");
            return $"Message inserted at {result.Value.InsertionTime}";
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
