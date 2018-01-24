using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    [Produces("application/json")]
    [Route("api/CartAPI")]
    public class CartAPIController : Controller
    {
        private readonly Co_PartnershipContext _context;
        private Cart cart;
        public CartAPIController()
        {

        }
    }
}