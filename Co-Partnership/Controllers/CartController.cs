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
        private IAddressRepository _addressRepository;

        public CartController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IItemRepository itemRepository,
            ITransactionRepository transactionRepository,
            ITransactionItemRepository transactionItems,
            Cart cartService,
            IAddressRepository addressRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _transactionRepository = transactionRepository;
            _transactionItems = transactionItems;
            _cart = cartService;
            _addressRepository = addressRepository;
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
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(Address address)
        {
            if (!ModelState.IsValid)
            {
                return View(address);
            }

            var userId = await GetUserId();
            address.UserId = userId;
            var incomplete = _transactionRepository.GetIncompleteTransaction(userId);
            var transId = incomplete.Id;
            address.TransactionId = transId;

            _addressRepository.SaveAddress(address); //save address to db

            var transactionItems = incomplete.TransactionItem.ToList();
            // afairesh twn proiontwn apo to stock
            foreach (var cItem in _cart.CartItems)
            {
                var item = _itemRepository.Items.FirstOrDefault(i => i.Id == cItem.ItemId);
                var stock = item.StockQuantity;
                if (cItem.Quantinty >= stock)
                {
                    item.StockQuantity = 0;
                    item.IsLive = false;
                }
                else
                {
                    item.StockQuantity -= cItem.Quantinty;   
                }
                _itemRepository.UpdateItem(item); // save items to db
            }

            incomplete.Type = 1; // change incomplete transaction to complete
            _transactionRepository.UpdateTransaction(incomplete); // update transaction


            _cart.Clear(); // clear cart

            
            
            return RedirectToAction("Success", "Cart", new { transactionId = transId });
        }

        public IActionResult Success(int transactionId)
        {
            var transaction = _transactionRepository.Transactions.SingleOrDefault(tr => tr.Id == transactionId);
            var transactionItems = transaction.TransactionItem.ToList();
            var price = (decimal)transaction.Price;
            var successCartView = new SuccessCartViewModel()
            {
                PriceNoVAT = price,
                TransactionItems = transactionItems
            };
            return View(successCartView);
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

            return RedirectToAction(nameof(HomeController.Index), "Home");
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