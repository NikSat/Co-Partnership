using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Co_Partnership.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class MemberController : Controller
    {
        private ITransactionRepository _transactionRepository;
        private UserManager<ApplicationUser> _userManager;
        private IUserRepository _userRepository;
        private IItemRepository _itemRepository;
        private ITransactionItemRepository _transactionItems;

        public MemberController(
            ITransactionRepository transactionRepository,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IItemRepository itemRepository,
            ITransactionItemRepository transactionItem)
        {
            _transactionRepository = transactionRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _transactionItems = transactionItem;
        }

        [HttpGet]
        [Route("Member/Index")]
        public async Task<IActionResult> Index() // See memberAccount (balance & transactions)
        {
            var userId = await GetUserId();
            var transactions = _transactionRepository.Transactions
                .Where(t => (t.IsProcessed == 1 && (t.OwnerId == userId || t.RecipientId == userId)))
                    //(t.OwnerId == userId && t.IsProcessed == 1 && t.Type == 2) ||  //accepted offers
                    //(t.RecipientId == userId && t.IsProcessed == 1 && t.Type == 3)) // sales share
                .OrderByDescending(t => t.DateProcessed)
                .ToList();

            var model = new MemberAccountViewModel()
            {
                Transactions = transactions,
                Username = _userRepository.GetName(userId),
                Balance = (decimal)_userRepository.GetBalance(userId),
                CurrentUserId = userId,               
            };
            return View(model);
        }

        public IActionResult OfferItems()
        {
            var model = new OfferItemsViewModel()
            {
                Items = _itemRepository.Items.OrderBy(i => i.Category).ToList(),
                SelectItems = new List<SelectListItem>()
            };
            foreach (var item in model.Items)
            {
                model.SelectItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult OfferItems(OfferItemsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction(nameof(Offer), new { id = model.ItemId });
        }

        public IActionResult Offer(int id)
        {
            var model = new OfferViewModel()
            {
                Item = _itemRepository.GetItem(id),
                Quantity = 0
            };            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Offer(OfferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = await GetUserId();
            var item = _itemRepository.GetItem(model.ItemId);
            decimal price = (decimal)item.UnitPrice / 3m ;
            var offer = new Transaction()
            {
                Date = DateTime.Now,
                OwnerId = userId,
                IsProcessed = 0,
                Price = Math.Round(price, 2) * (decimal)model.Quantity,
                Type = 2
            };
            var transaction =_transactionRepository.SaveTransaction(offer);

            var trItem = new TransactionItem()
            {
                TransactionId = transaction.Id,
                ItemId = item.Id,
                Quantinty = model.Quantity
            };
            _transactionItems.AddOrUpdate(trItem);

            return View("OfferSubmitted");
        }

        public IActionResult MembersTable()
        {
            var users = _userRepository.Users
                .Where(u => u.UserType == 2 || u.UserType == 3)
                .OrderByDescending(u => u.UserType);
            var model = new List<MembersTableViewModel>();
            foreach(var user in users)
            {
                model.Add(new MembersTableViewModel()
                {
                    Name = _userRepository.GetName(user.Id),
                    Type = (user.UserType == 3) ? "administrator" : "member",
                    Email = _userRepository.GetEmail(user.ExtId)
                });
            }
            return View(model);
        }

        public async Task<int> GetUserId()
        {
            var currentuser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);

            return _userRepository.GetUserFromIdentity(currentuser.Id);
        }
    }
}