using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AATShared;
using Microsoft.AspNetCore.Authorization;

namespace AATWebApi.Controllers
{
    [Authorize(Roles = "Contribute")]
    [ApiController]
    [Route("[controller]")]
    public class CloseoutController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            return $"Authorization Header: {HttpContext.Request.Headers["Authorization"]}";
        }
    }
}
