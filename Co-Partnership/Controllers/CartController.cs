using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    
    public class CartController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private IUserRepository _userRepository;
        private IItemRepository _itemRepository;
        private ITransactionRepository _transactionRepository;
        private ITransactionItemRepository _transactionItems;
        private Cart _cart;

        public CartController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IItemRepository itemRepository,
            ITransactionRepository transactionRepository,
            ITransactionItemRepository transactionItems,
            Cart cartService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _transactionRepository = transactionRepository;
            _transactionItems = transactionItems;
            _cart = cartService;
        }

        [Route("Cart")]
        public ViewResult Index()
        {
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(_cart);
        }

        public IActionResult AddToCart(int itemId, string returnUrl)
        {
            Item item = _itemRepository.Items.SingleOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                _cart.AddItem(item, 1);
            }
            return Redirect(returnUrl);
        }

        
        public async Task<IActionResult> CheckOut()
        {
            var user = HttpContext.User.Identity.Name;
            if (user == null)
            {
                return RedirectToAction("AuthorizeIndex");
            }
            var id = await GetUserId();
            
            _transactionItems.SaveCartToDB(id);
            return View();
        }

        public async Task<IActionResult> SaveLogout()
        {
            var user = HttpContext.User.Identity.Name;
            if (user == null)
            {
                return RedirectToAction("AuthorizeIndex");
            }
            var id = await GetUserId();

            _transactionItems.SaveCartToDB(id);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index");
            //return RedirectToRoute("{controller=Account}/{action=Logout}");
        }

        [Authorize]
        public IActionResult AuthorizeIndex()
        {
            return RedirectToAction("Index");
        }

        public async Task<int> GetUserId()
        {
            var currentuser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);

            return _userRepository.GetUserFromIdentity(currentuser.Id);
        }


    }
}