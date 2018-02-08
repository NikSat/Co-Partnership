using System;
using System.Collections.Generic;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Mvc;

namespace Co_Partnership.Controllers
{
    [Produces("application/json")]
    public class ProductsApiController : Controller
    {
        private IItemRepository itemsRepository;
        public ProductsApiController(IItemRepository _itemRepository)
        {
            itemsRepository = _itemRepository;
        }

        [Route("Admin/api/Products")]
        [HttpGet]
        public IEnumerable<Object> Get()
        {
            return itemsRepository.Items;
        }
    }
}