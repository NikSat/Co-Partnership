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
        public AdminController(IItemRepository itemRepository, IHostingEnvironment hostingEnvironment, ITransactionRepository transactionRepository)
        {
            _itemRepository = itemRepository;
            _hostingEnvironment = hostingEnvironment;
            _transactionRepository = transactionRepository;
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
    }
}