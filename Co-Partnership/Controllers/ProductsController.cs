using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Co_Partnership.Data;
using Co_Partnership.Models.Database;
using Co_Partnership.Models;

namespace Co_Partnership.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Co_PartnershipContext _context;
        public int PageSize = 8;
        public ProductsController(Co_PartnershipContext context)
        {
            _context = context;
        }

        // GET: All Products that are live
        public async Task<ViewResult> Index(string category = null, int productPage = 1)
        {

            return View(new ProductViewModel
            {
                Products= await _context.Item.Where(p => (p.IsLive ?? false)&&(category == null || p.Category == category))
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize)
                .AsNoTracking()
                .ToListAsync(),//Usage of NoTracking() is recommended when your query is meant for read operations. In these scenarios, you get back your entities but they are not tracked by your context.This ensures minimal memory usage and optimal performance
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)_context.Item.Where(p => (p.IsLive ?? false) && (category == null || p.Category == category)).Count() / (double)PageSize)
                },

                CurrentCategory = category
            });
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

