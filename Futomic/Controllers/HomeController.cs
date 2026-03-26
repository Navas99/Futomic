using Futomic.Data;
using Futomic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Futomic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Traemos los 3 primeros campos de la base de datos
            var featuredFields = _context.Fields.Take(3).ToList();

            return View(featuredFields);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
