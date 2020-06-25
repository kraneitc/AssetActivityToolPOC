using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AATWebApp.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AATWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiManagerService _apiManagerService;

        public HomeController(ILogger<HomeController> logger, ApiManagerService client)
        {
            _logger = logger;
            _apiManagerService = client;
        }

        public async Task<IActionResult> Index()
        {

            var result = await _apiManagerService.Client.GetAsync("https://sapn-enterpriseapim-poc2-ae-api.azure-api.net/sap-closeout/closeout");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
