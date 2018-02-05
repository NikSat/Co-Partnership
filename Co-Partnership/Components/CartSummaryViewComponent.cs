using Co_Partnership.Models;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Components
{
    [ViewComponent(Name = "CartSummary")]
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;

        public CartSummaryViewComponent(Cart c)
        {
            cart = c;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartSummary = new CartSummaryViewModel()
            {
                NumberOfItems = cart.NumberOfItems(),
                Price = cart.ComputeTotalValue()*1.23m
            };
            return View(cartSummary);
        }

    }
}
