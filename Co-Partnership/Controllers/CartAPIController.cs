using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    [Produces("application/json")]
    [Route("api/Cart")]
    public class CartAPIController : Controller
    {
        private readonly Co_PartnershipContext _context;
        private Cart cart;

        public CartAPIController(Co_PartnershipContext context, Cart cartService)
        {
            _context = context;
            cart = cartService;
        }

        // GET: api/CartAPI
        [HttpGet]
        public IEnumerable<TransactionItem> Get()
        {
            return cart.CartItems;
        }

        [HttpPost]
        public TransactionItem Post(int itemId, int quantinty)
        {
            if (quantinty==null || quantinty <= 0)
            {
                quantinty = 1;
            }

            return cart.AddItem(itemId, quantinty);
        }

    }
}