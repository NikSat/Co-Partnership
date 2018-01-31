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
using Microsoft.AspNetCore.Http;

namespace Co_Partnership.Controllers
{
    [Produces("application/json")]
    [Route("api/Cart")]
    public class CartAPIController : Controller
    {
        private IItemRepository _itemRepository;
        private Cart cart;

        public CartAPIController(IItemRepository itemRepository, Cart cartService)
        {
            _itemRepository = itemRepository;
            cart = cartService;
        }

        // GET: api/CartAPI
        [HttpGet]
        public IEnumerable<TransactionItem> Get()
        {
            return cart.CartItems;
        }

        [HttpPost]
        public IActionResult Post(int cartItemId, int quantinty)
        {
            if (quantinty==null || quantinty <= 0)
            {
                quantinty = 1;
            }
            if(cartItemId==null || cartItemId < 0)
            {
                return null;
            }           

            var cartItem = cart.UpdateQuantity(cartItemId, quantinty);
            if(cartItem != null)
            {
                return Ok();
            }
            return null;
        }

    }
}