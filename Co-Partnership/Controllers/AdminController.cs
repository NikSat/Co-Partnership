using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{

    public class AdminController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAddressRepository _addressRepository;

        public AdminController(
            IItemRepository itemRepository, 
            IHostingEnvironment hostingEnvironment, 
            ITransactionRepository transactionRepository,
            IAddressRepository addressRepository)
        {
            _itemRepository = itemRepository;
            _hostingEnvironment = hostingEnvironment;
            _transactionRepository = transactionRepository;
            _addressRepository = addressRepository;
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
                model.Addresses.Add(_addressRepository
                    .AddressRepo
                    .FirstOrDefault(a => a.TransactionId == order.Id));
            }
            return View(model);
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
    }
}