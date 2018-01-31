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

        //[HttpPost]
        //public void Index(string searchString, string sortOrder)
        //{
        //    Index(searchString, sortOrder);
        //}

        // GET: All Products that are live
        public async Task<ViewResult> Index(string searchString, string sortOrder = null,string category = null, int productPage = 1)
        {
           // ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
           // ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            //ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? sortOrder : "";
            //ViewData["PriceSortDesc"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            var sortPar = _context.Item.Where(p => (p.IsLive ?? false) && (category == null || p.Category == category));
            if (!String.IsNullOrEmpty(searchString))
            {
                sortPar = sortPar.Where(s => s.Name.Contains(searchString)
               
                );//pros evaluation to sygkekrimeno
            }

            switch (sortOrder)
            {
                //case "name_desc":
                //    sortPar = sortPar.OrderByDescending(s => s.Name);
                //    break;
                //case "Price":
                //    sortPar = sortPar.OrderBy(s => ((decimal?)s.UnitPrice));//den exei ylopoih8ei to price swsta akoma
                //    ViewBag.CurrentSorting = sortOrder;
                //    break;
                //case "price_desc":
                //    sortPar = sortPar.OrderByDescending(s => ((decimal?)s.UnitPrice));//den exei ylopoih8ei to price swsta akoma
                //    ViewBag.CurrentSorting = sortOrder;
                //    break;
                //case "Date":
                //            sortPar = sortPar.OrderBy(s => s.);//den exei ylopoih8ei to date akoma
                //    break;
                //case "date_desc":
                //            sortPar = sortPar.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    sortPar = sortPar.OrderBy(s => s.Name);//auto prepei na to vgalv an apofsisv na mhn exw allo sorting
                    ViewBag.CurrentSorting = null;
                    break;
            }
            

            return View(new ProductViewModel
            {
                Products = await sortPar
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

