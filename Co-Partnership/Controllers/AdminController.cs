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

        public AdminController(
            IItemRepository itemRepository, 
            IHostingEnvironment hostingEnvironment, 
            ITransactionRepository transactionRepository,
            IAddressRepository addressRepository,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            ICompAccountRepository compAccount)
        {
            _itemRepository = itemRepository;
            _hostingEnvironment = hostingEnvironment;
            _transactionRepository = transactionRepository;
            _addressRepository = addressRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _compAccount = compAccount;
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

            if(coBalance < offer.Price)
            {
                model.ErrorMessage = "Co-Partenership blanace is not enought to buy the offer.";
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



            //transaction accepted by current user
            offer.IsProcessed = 1;
            offer.RecipientId = adminId;
            _transactionRepository.UpdateTransaction(offer);




            return RedirectToAction(nameof(Requests));
        }

        public async Task<int> GetUserId()
        {
            var currentuser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);

            return _userRepository.GetUserFromIdentity(currentuser.Id);
        }

    }
}