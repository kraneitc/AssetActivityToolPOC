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
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Rest;
using Microsoft.Azure.Storage;
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
            await QueueMessageAsync("pocaatsa", "aatqueue", "message");

            return $"Message inserted";
        }

        private static async Task QueueMessageAsync(string accountName, string queueName, string message)
        {
            var token = new AzureServiceTokenProvider().GetAccessTokenAsync($"https://storage.azure.com/").Result;
            var creds = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(new Microsoft.WindowsAzure.Storage.Auth.TokenCredential(token));

            var queueEndpoint = $"https://{accountName}.queue.core.windows.net/";
            //var queueClient = new QueueClient(new Uri(queueEndpoint), new DefaultAzureCredential(true));
            var queueClient = new CloudQueueClient(new Uri(queueEndpoint), creds);

            var queue = queueClient.GetQueueReference(queueName);


            await queue.CreateIfNotExistsAsync();
            await queue.AddMessageAsync(new CloudQueueMessage(message));
        }
    }
}
