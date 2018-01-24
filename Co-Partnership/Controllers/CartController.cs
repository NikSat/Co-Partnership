using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    public class CartController : Controller
    {
        private readonly Co_PartnershipContext _context;
        private Cart cart;
        public CartController(Co_PartnershipContext context, Cart cartService)
        {
            _context = context;
            cart = cartService;
        }

        public ViewResult Index()
        {
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(cart);
        }

        //public RedirectToRouteResult AddToCart(int itemId, string returnUrl)
        //{
        //    Item item = _context.Item.SingleOrDefault(i => i.Id == itemId);

        //    if (item != null)
        //    {
        //        cart.AddItem(item, 1);
        //    }
        //    TempData["returnUrl"] = returnUrl;
        //    return RedirectToRoute("cart");
        //}
    }
}