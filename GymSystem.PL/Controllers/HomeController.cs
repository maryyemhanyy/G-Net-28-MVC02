using GymSystem.BLL.Services.Interfaces;
using GymSystem.PL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystem.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAnalyticsService _analyticsService;

        public HomeController(ILogger<HomeController> logger , IAnalyticsService analyticsService)
        {
            _logger = logger;
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var dashboard = await _analyticsService.GetAnalyticsDataAsync(ct);

            return View(dashboard);
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
