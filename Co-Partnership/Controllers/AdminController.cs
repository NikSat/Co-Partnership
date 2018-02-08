using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICompAccountRepository _compAccount;
        private readonly IPersonalAccountRepository _personalAccountRepo;
        private readonly IMessageInterface _messageInterface;

        public AdminController(
            IItemRepository itemRepository,
            IHostingEnvironment hostingEnvironment,
            ITransactionRepository transactionRepository,
            IAddressRepository addressRepository,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            ICompAccountRepository compAccount,
            IPersonalAccountRepository personal,
            IMessageInterface messageInterface)
        {
            _itemRepository = itemRepository;
            _hostingEnvironment = hostingEnvironment;
            _transactionRepository = transactionRepository;
            _addressRepository = addressRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _compAccount = compAccount;
            _personalAccountRepo = personal;
            _messageInterface = messageInterface;
        }

        public IActionResult Finance()
        {
            ViewBag.CurrentChoice = this.ControllerContext.RouteData.Values["action"].ToString();
            return View("Finance");
        }

        public IActionResult Products()
        {
            ViewBag.CurrentChoice = this.ControllerContext.RouteData.Values["action"].ToString();
            return View();
        }

        public IActionResult MessageBoard()
        {
            ViewBag.CurrentChoice = this.ControllerContext.RouteData.Values["action"].ToString();
            return View();
        }

        public IActionResult Sales()
        {
            ViewBag.CurrentChoice = this.ControllerContext.RouteData.Values["action"].ToString();
            return View();
        }


        public IActionResult CreateProduct()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Item newItem, IFormFile uploadedImage)
        {



            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var filename = ContentDispositionHeaderValue
                      .Parse(uploadedImage.ContentDisposition)
                      .FileName
                      .Trim('"');
                var filepath = _hostingEnvironment.WebRootPath + $@"\images\{filename}";
                newItem.Image = $"/images/{filename}";
                if (ModelState.IsValid)
                {
                    _itemRepository.SaveItem(newItem);



                    using (System.IO.FileStream fs = System.IO.File.Create(filepath))
                    {
                        uploadedImage.CopyTo(fs);
                        fs.Flush();
                    }

                    return RedirectToAction("Products");
                }
            }
            ViewBag.ErrorMessage = "image upload is required.";
            return View(newItem);


        }


        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            _itemRepository.DeleteItem(id);
            return View("Products");
        }

        public IActionResult EditProduct(int id)
        {
            var item = _itemRepository.GetItem(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(int? id, Item item)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _itemRepository.UpdateItem(item);

                return RedirectToAction("Products");
            }
            return View(item);
        }

        public IActionResult SeeTransactions()
        {
            ViewBag.CurrentChoice = ControllerContext.RouteData.Values["action"].ToString();
            var model = new SeeTransactionsViewModel()
            {
                Orders = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 1 && t.Type == 1).ToList(),
                DeclinedOrders = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 1 && t.Type == -1).ToList(),
                SalesShare = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 1 && t.Type == 3).ToList(),
                Offers = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 1 && t.Type == 2).ToList(),
                DeclinedOffers = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 1 && t.Type == -2).ToList()
            };
            
            return View(model);
        }

        public IActionResult Requests()
        {
            ViewBag.CurrentChoice = ControllerContext.RouteData.Values["action"].ToString();
            var model = new RequestsViewModel()
            {
                PendingOffers = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 0 && t.Type == 2).ToList(),
                PendingOrders = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 0 && t.Type == 1).ToList(),
                Addresses = new List<Address>()
            };

            foreach (var order in model.PendingOrders)
            {
                var address = _addressRepository.AddressRepo
                    .FirstOrDefault(a => a.TransactionId == order.Id);
                if (address != null)
                {
                    model.Addresses.Add(address);
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Requests(RequestsViewModel model)
        {
            var newModel = new RequestsViewModel()
            {
                PendingOffers = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 0 && t.Type == 2).ToList(),
                PendingOrders = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 0 && t.Type == 1).ToList(),
                Addresses = new List<Address>()
            };

            foreach (var order in newModel.PendingOrders)
            {
                var address = _addressRepository.AddressRepo
                    .FirstOrDefault(a => a.TransactionId == order.Id);
                if (address != null)
                {
                    newModel.Addresses.Add(address);
                }
            }

            newModel.ErrorMessage = model.ErrorMessage;
            return View(newModel);
        }

        public async Task<IActionResult> AcceptOffer(RequestsViewModel model)
        {
            var transactionId = model.TransactionId;
            var offer = _transactionRepository.Transactions.FirstOrDefault(t => t.Id == transactionId);
            var adminId = await GetUserId();

            var coBalance = _compAccount.GetCoopShare();

            if (coBalance < offer.Price) //check if company has enough money
            {
                model.ErrorMessage = "Co-Partenership balance is not enought to buy the offer.";
                model.PendingOffers = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 0 && t.Type == 2).ToList();
                model.PendingOrders = _transactionRepository.Transactions
                    .Where(t => t.IsProcessed == 0 && t.Type == 1).ToList();
                model.Addresses = new List<Address>();
                foreach (var order in model.PendingOrders)
                {
                    var address = _addressRepository.AddressRepo
                        .FirstOrDefault(a => a.TransactionId == order.Id);
                    if (address != null)
                    {
                        model.Addresses.Add(address);
                    }
                }
                return View("Requests", model);
            }

            //take money from company
            coBalance -= (decimal)offer.Price;
            _compAccount.UpdateCoopBalance(coBalance);

            //give money to member
            var memberAccount = _personalAccountRepo.GetAccount((int)offer.OwnerId);
            if (memberAccount == null)
            {
                memberAccount = new PersonalFinancialAccount()
                {
                    UserId = (int)offer.OwnerId,
                    Amount = offer.Price
                };
                _personalAccountRepo.AddAccount(memberAccount);
            }
            else
            {
                memberAccount.Amount += offer.Price;
                _personalAccountRepo.UpdateAccount(memberAccount);
            }

            //offer accepted by current user
            offer.IsProcessed = 1;
            offer.DateProcessed = DateTime.Now;
            offer.RecipientId = adminId;
            _transactionRepository.UpdateTransaction(offer);

            //get offered products & add them to db
            var itemId = offer.TransactionItem.First().ItemId;
            var quantity = offer.TransactionItem.First().Quantinty;

            var item = _itemRepository.GetItem((int)itemId);
            item.StockQuantity += quantity;
            _itemRepository.UpdateItem(item);

            //send notification to member
            var message = new Message()
            {
                SenderId = adminId,
                ReceiverId = offer.OwnerId,
                DateSent = DateTime.Now,
                Title = "Offer accepted",
                Message1 = $"Your offer for {quantity} {item.UnitType} {item.Name} has been accepted and {Math.Round((decimal)offer.Price, 2)}€ has been deposited to your member account.\n Thank you and have a great day,\n Co-Partenership"
            };
            _messageInterface.SaveMessage(message);

            return RedirectToAction(nameof(Requests));
        }

        public async Task<IActionResult> DeclineOffer(RequestsViewModel model)
        {
            var transactionId = model.TransactionId;
            var offer = _transactionRepository.Transactions.FirstOrDefault(t => t.Id == transactionId);
            var adminId = await GetUserId();

            //offer declined(type = -2) processed by admin
            offer.IsProcessed = 1;
            offer.DateProcessed = DateTime.Now;
            offer.RecipientId = adminId;
            offer.Type = -2;
            offer.Price = 0m; //just for safety
            _transactionRepository.UpdateTransaction(offer);

            //send notification to member
            var item = offer.TransactionItem.First().Item;
            var quantity = offer.TransactionItem.First().Quantinty;
            var message = new Message()
            {
                SenderId = adminId,
                ReceiverId = offer.OwnerId,
                DateSent = DateTime.Now,
                Title = "Offer declined",
                Message1 = $"We are sorry to inform you that your offer for {quantity} {item.UnitType} {item.Name} has been declined.\n Thanks for your attention, consideration, and time,\n Co-Partenership"
            };
            _messageInterface.SaveMessage(message);

            return RedirectToAction(nameof(Requests));
        }

        public async Task<IActionResult> AcceptOrder(RequestsViewModel model)
        {
            var transactionId = model.TransactionId;
            var order = _transactionRepository.Transactions.FirstOrDefault(t => t.Id == transactionId);
            var adminId = await GetUserId();

            //order shipped by admin
            order.DateProcessed = DateTime.Now;
            order.IsProcessed = 1;
            order.RecipientId = adminId;
            _transactionRepository.UpdateTransaction(order);

            // get clients money and balance it to member & coopshare            
            var companyAccount = _compAccount.Account;
            var price = order.Price / 2m;
            companyAccount.CoOpShare += price;
            companyAccount.MemberShare += price;
            companyAccount.Date = DateTime.Now;
            _compAccount.UpdateAccount(companyAccount);

            //send notification to user
            var items = string.Join(',', order.TransactionItem
                .Select(ti => ti.Item.Name)
                .ToArray());
            var message = new Message()
            {
                SenderId = adminId,
                ReceiverId = order.OwnerId,
                DateSent = DateTime.Now,
                Title = "Your Order is being shipped",
                Message1 = $"Your order for {items} is being shipped. We hope to see you again soon.\n Have a great day,\n Co-Partenership"
            };
            _messageInterface.SaveMessage(message);

            return RedirectToAction(nameof(Requests));
        }

        public async Task<IActionResult> DeclineOrder(RequestsViewModel model)
        {
            var transactionId = model.TransactionId;
            var order = _transactionRepository.Transactions.FirstOrDefault(t => t.Id == transactionId);
            var adminId = await GetUserId();

            //order declined (type = -1) by admin
            order.IsProcessed = 1;
            order.DateProcessed = DateTime.Now;
            order.RecipientId = adminId;
            order.Type = -1;
            order.Price = 0;
            _transactionRepository.UpdateTransaction(order);

            //return products to repo
            foreach (var transactionItem in order.TransactionItem)
            {
                var itemId = transactionItem.ItemId;
                var quantity = transactionItem.Quantinty;

                var item = _itemRepository.GetItem((int)itemId);
                item.StockQuantity += quantity;
                _itemRepository.UpdateItem(item);
            }

            //send notification to client
            var items = string.Join(',', order.TransactionItem
                .Select(ti => ti.Item.Name)
                .ToArray());
            var message = new Message()
            {
                SenderId = adminId,
                ReceiverId = order.OwnerId,
                DateSent = DateTime.Now,
                Title = "Your Order is cancel",
                Message1 = $"We are sorry to inform you that your order for {items} cannot be completed right now. Feel free to contact us for more information. We hope to see you again soon.\n Have a great day,\n Co-Partenership"
            };
            _messageInterface.SaveMessage(message);

            return RedirectToAction(nameof(Requests));
        }

        public async Task<int> GetUserId()
        {
            var currentuser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);

            return _userRepository.GetUserFromIdentity(currentuser.Id);
        }

    }
}