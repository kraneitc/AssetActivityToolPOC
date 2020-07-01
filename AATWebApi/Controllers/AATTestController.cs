using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AATWebApi.Models;
using AATWebApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AATWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AATTestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AATTestController> _logger;

        public AATTestController(ILogger<AATTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            using var ctx = new AATDbContext();
            return ctx.Product.ToList();
        }
    }
}
