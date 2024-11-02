using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTrack.Data;
using MoneyTrack.Models;
using System.Diagnostics;

namespace MoneyTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MoneyTrackContext _context;

        public HomeController(ILogger<HomeController> logger, MoneyTrackContext context)
        {
            _logger = logger;
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.ProjectTable.Take(6).ToListAsync(); // Fetch first 6 projects
            return View(projects);
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
       
        public IActionResult Contact()
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
