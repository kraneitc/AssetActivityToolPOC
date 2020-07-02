using System.Collections.Generic;
using System.Linq;
using AATWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AATWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AATTestController : ControllerBase
    {
        private readonly AATDbContext _context;

        public AATTestController(AATDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _context.Product.Take(5).ToList();
        }
    }
}
