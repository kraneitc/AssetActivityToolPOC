using System.Collections.Generic;
using System.Linq;
using AATWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Azure;
using Azure.Storage.Queues;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Services.AppAuthentication;
using TokenCredential = Azure.Core.TokenCredential;

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

        private static async Task<Response<SendReceipt>> QueueMessageAsync(string accountName, string queueName, string message)
        {
            var queueEndpoint = $"https://{accountName}.queue.core.windows.net/{queueName}";
            var queueClient = new QueueClient(new Uri(queueEndpoint), new DefaultAzureCredential());

            await queueClient.CreateIfNotExistsAsync();
            return await queueClient.SendMessageAsync(message);
        }
    }
}
