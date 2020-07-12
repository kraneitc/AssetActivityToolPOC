using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AATShared;

namespace AATWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiManagerService _apiManagerService;

        public HomeController(ILogger<HomeController> logger, ApiManagerService client)
        {
            _apiManagerService = client;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _apiManagerService.Client.GetAsync("https://sapn-enterpriseapim-poc2-ae-api.azure-api.net/sap-closeout/closeout");
            //var result = await _apiManagerService.Client.GetAsync("https://localhost:44344/closeout");
            return View(new
            {
                Result = result.IsSuccessStatusCode,
                Token = _apiManagerService.Client.DefaultRequestHeaders.Authorization.Parameter
            });
        }
    }
}
