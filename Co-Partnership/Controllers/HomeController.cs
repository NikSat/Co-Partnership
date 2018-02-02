using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Co_Partnership.Models;
using Co_Partnership.Data;
using Microsoft.EntityFrameworkCore;
using Co_Partnership.Services;

namespace Co_Partnership.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemRepository _context;
        public HomeController(IItemRepository context)
        {
            _context = context;
        }
        public async Task<ViewResult>  Index()
        {
            return View(await _context.GetTop().Where(p => (p.Product.IsLive ?? false)).ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
