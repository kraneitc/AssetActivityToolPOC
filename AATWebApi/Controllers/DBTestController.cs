using System.Collections.Generic;
using System.Linq;
using AATWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AATWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DBTestController : ControllerBase
    {
        private readonly AATDbContext _context;

        public DBTestController(AATDbContext context)
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
