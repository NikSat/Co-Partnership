using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    public class CartController : Controller
    {
        private IItemRepository _itemRepository;
        private Cart cart;
        public CartController(IItemRepository itemRepository, Cart cartService)
        {
            _itemRepository = itemRepository;
            cart = cartService;
        }

        public ViewResult Index()
        {
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(cart);
        }

        public IActionResult AddToCart(int itemId, string returnUrl)
        {
            Item item = _itemRepository.Items.SingleOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                cart.AddItem(item, 1);
            }
            //TempData["returnUrl"] = returnUrl;
            return Redirect(returnUrl);
        }
    }
}