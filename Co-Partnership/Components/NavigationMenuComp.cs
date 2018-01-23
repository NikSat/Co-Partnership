using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Co_Partnership.Components
{
    public class NavigationMenuComp : ViewComponent
    {
        private readonly Co_PartnershipContext _context;

        public NavigationMenuComp(Co_PartnershipContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.CurrentCategory = RouteData.Values["category"];
            return View( _context.Item
                        .Select(x => x.Category)
                        .Distinct()
                        .OrderBy(x => x));
        }
    }
}


