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
        private UserManager<ApplicationUser> _userManager;
        private IUserRepository _userRepository;
        private IItemRepository _itemRepository;
        private ITransactionRepository _transactionRepository;
        private ITransactionItemRepository _transactionItems;
        private Cart _cart;

        public CartController(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IItemRepository itemRepository,
            ITransactionRepository transactionRepository,
            ITransactionItemRepository transactionItems,
            Cart cartService)
        {
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
            await SaveCartAsync();
            return View();
        }

        [Authorize]
        public IActionResult AuthorizeIndex()
        {
            return RedirectToAction("Index");
        }

        public async Task SaveCartAsync()
        {            
            var userid = await GetUserId();
            var incomplete = _transactionRepository.Transactions.FirstOrDefault(t => t.OwnerId == userid && t.Type == 0); // incomplete transaction = cart saved 
            //if there isn't an incomplete transaction create an new one
            if (incomplete == null)
            {
                var incTransaction = new Transaction()
                {
                    OwnerId = userid,
                    Date = DateTime.Now,
                    Type = 0,
                    IsProcessed = 0
                };
                _transactionRepository.SaveTransaction(incTransaction); // save new transaction to db
                incomplete = _transactionRepository.Transactions.FirstOrDefault(t => t.OwnerId == userid && t.Type == 0); // get incomplete trasnaction
            }
            var transactionId = incomplete.Id;         

            // save cartItems to TransactionItems table
            // if they already exist update them
            foreach(var item in _cart.CartItems)
            {
                //item.Id = null;
                item.TransactionId = transactionId;
                item.Acceptance = true;
                var itemExists = _transactionItems.TIRepository.FirstOrDefault(ti => ti.Id == transactionId && ti.ItemId == item.ItemId);
                if(itemExists != null)
                {
                    _transactionItems.UpdateItem(item);
                }
                else
                {
                    _transactionItems.SaveItem(item);
                }
            }

            // get items for incomplete transaction
            List<TransactionItem> items = _transactionItems.TIRepository.Where(ti => ti.TransactionId == transactionId).ToList();
            // calculate price
            var price = items.Select(i => (decimal)i.Quantinty * i.Item.UnitPrice).Sum();// without VAT

            //save price into transaction
            incomplete.Price = price;
            _transactionRepository.UpdateTransaction(incomplete);
        }

        public async Task<int> GetUserId()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return _userRepository.Users.FirstOrDefault(a => a.ExtId == user.Id).Id;
        }
    }
}