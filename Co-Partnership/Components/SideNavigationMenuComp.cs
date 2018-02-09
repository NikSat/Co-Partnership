using Co_Partnership.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Components
{
    public class SideNavigationMenuComp: ViewComponent
    {
        private readonly Co_PartnershipContext _context;

        public SideNavigationMenuComp(Co_PartnershipContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.CurrentCategory = RouteData.Values["category"];
            return View("Side", _context.Item
                        .Where(x => x.IsLive == true)
                        .Select(x => x.Category)
                        .Distinct()
                        .OrderBy(x => x));
        }

    }
}
