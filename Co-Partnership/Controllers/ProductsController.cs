using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Co_Partnership.Data;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Co_PartnershipContext _context;

        public ProductsController(Co_PartnershipContext context)
        {
            _context = context;
        }

        // GET: All Products that are live
        public async Task<ViewResult> Index()
        {
            return View(await _context.Item.Where(p => (p.IsLive ?? false)).ToListAsync());
        }

        // GET: Category Products that are live
        public async Task<IActionResult> Category(string category)
        {
            //
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var catProducts = await _context.Item
                .Where(p => (p.IsLive ?? false) && p.Category == category)
                .ToListAsync();

            //
            if (catProducts == null || catProducts.Count == 0)
            {
                return RedirectToAction("Index");
            }

            return View("Index", catProducts);
        }

        // GET: One Product that is live
        public async Task<IActionResult> Product(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .SingleOrDefaultAsync(m => (m.IsLive ?? false) && m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

    }
}

