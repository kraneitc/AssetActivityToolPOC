using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AATShared;

namespace AATWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueTestController : ControllerBase
    {
        private readonly QueueService _queueService;

        public QueueTestController(QueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var result = await _queueService.QueueMessageAsync("pocaatsa", "aatqueue", "Message from AAT API");
            return $"Message inserted at {result.Value.InsertionTime}";
        }
    }
}
