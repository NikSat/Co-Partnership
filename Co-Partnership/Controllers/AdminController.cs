using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Finance()
        {
            return View();
        }
    }
}