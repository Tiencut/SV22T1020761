using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace SV22T1020761.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading home page");
                return View();
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading privacy page");
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
