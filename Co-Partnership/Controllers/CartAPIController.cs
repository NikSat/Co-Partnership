using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Co_Partnership.Controllers
{
    [Produces("application/json")]

    public class CartAPIController : Controller
    {
        private IItemRepository _itemRepository;
        private Cart cart;
        private ITransactionItemRepository _transactionItems;
        private UserManager<ApplicationUser> _userManager;
        private IUserRepository _userRepository;

        public CartAPIController(
            IItemRepository itemRepository, 
            Cart cartService, 
            ITransactionItemRepository transactionItems,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            cart = cartService;
            _transactionItems = transactionItems;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        // GET: api/CartAPI
        [Route("api/Cart")]
        [HttpGet]
        public IEnumerable<TransactionItem> Get()
        {
            return cart.CartItems;
        }

        [Route("api/Cart")]
        [HttpPost]
        public IActionResult Post([FromBody] TransactionItem item)//[FromBody]int ItemId, [FromBody]int quantinty)
        {
            var quantinty = (int)item.Quantinty;
            var itemId = (int)item.ItemId;
            if (quantinty <= 0)
            {
                quantinty = 1;
            }
            if (itemId < 0)
            {
                return null;
            }

            var cartItem = cart.UpdateQuantity(itemId, quantinty);
            if (cartItem != null)
            {
                return Ok();
            }
            return null;
        }

        [Route("api/Cart")]
        [HttpPut]
        public IActionResult Put([FromBody] int itemId)
        {
            if (itemId <= 0)
            {
                return null;
            }
            cart.RemoveItem(itemId);
            return Ok();
        }

        [Route("api/SaveCart")]
        [HttpPost]
        public async Task<IActionResult> SaveCart()
        {
            var user = HttpContext.User.Identity.Name;
            if (user == null)
            {
                return Ok();
            }

            var currentuser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);

            var id = _userRepository.GetUserFromIdentity(currentuser.Id);           

            _transactionItems.SaveCartToDB(id);
            return Ok();
        }
    }
}