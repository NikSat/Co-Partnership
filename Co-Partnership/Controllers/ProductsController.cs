using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Co_Partnership.Data;
using Co_Partnership.Models.Database;
using Co_Partnership.Models;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Identity;

namespace Co_Partnership.Controllers
{
    public class ProductsController : Controller
    {
        private IItemRepository itemRepository;

        private UserManager<ApplicationUser> manager;

        private IWishRepository wishRepository;

        private IUserRepository userep;


        public int PageSize = 8;


        // Constructor 
        public ProductsController(UserManager<ApplicationUser> mngr, IWishRepository wRpstr, IUserRepository us, IItemRepository itrep)
        {
            // Get the repositories through depedency injection
            manager = mngr;
            wishRepository = wRpstr;
            userep = us;
            itemRepository = itrep;
            
        }



        // This function gets the list of liked items for the specific user
        public async Task<List<LikeItem>> MakeLikeList()
        {
            // The list of integers
            List<int?> list = new List<int?>();
            // The list of likeitems
            List<LikeItem> LikeItemList = new List<LikeItem>();
            // Fill the list with list items nobody likes
            foreach (Item it in itemRepository.Items)
            {
                LikeItem Like = new LikeItem(it);
                LikeItemList.Add(Like);
            }


            // Now check if there is a user 

            var name = HttpContext.User.Identity.Name;
            

            if (name != null)
            {
                 var currentuser = await manager.FindByNameAsync(name).ConfigureAwait(false);

                // Get the id from this database
                var use = await userep.RetrieveByExternalAsync(currentuser.Id);
                list= await wishRepository.Wishes.Where(a => a.UserId == use.Id).Select(selector: b => b.ItemId).ToListAsync();

                // From this list got to the like items and change the items to liked
                // First check if list is empty ie the user has no favored any items if
                if (!list.Any())
                {
                    return LikeItemList;

                }
                else
                {
                    foreach (int iid in list)
                    {
                        foreach (LikeItem litem in LikeItemList)
                        {
                            if (litem.BaseItem.Id==iid)
                            {
                                litem.IsLiked = true;
                            }
                        }
                    }
                    return LikeItemList;
                }

            }
            else
            {
                return LikeItemList;
            }
            
        }





        //[HttpPost]
        //public void Index(string searchString, string sortOrder)
        //{
        //    Index(searchString, sortOrder);
        //}

        // GET: All Products that are live
        public async Task<ViewResult> Index(string searchString, string sortOrder = null,string category = null, int productPage = 1)
        {
            List<LikeItem> likeItem = await MakeLikeList();
           // ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
           // ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            //ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? sortOrder : "";
            //ViewData["PriceSortDesc"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            var sortPar =likeItem.Where(p => (p.BaseItem.IsLive ?? false) && (category == null || p.BaseItem.Category == category));
            if (!String.IsNullOrEmpty(searchString))
            {
                sortPar = sortPar.Where(s => s.BaseItem.Name.Contains(searchString)
               
                );//pros evaluation to sygkekrimeno
            }

            switch (sortOrder)
            {
                //case "name_desc":
                //    sortPar = sortPar.OrderByDescending(s => s.Name);
                //    break;
                //case "Price":
                //    sortPar = sortPar.OrderBy(s => ((decimal?)s.UnitPrice));//den exei ylopoih8ei to price swsta akoma
                //    ViewBag.CurrentSorting = sortOrder;
                //    break;
                //case "price_desc":
                //    sortPar = sortPar.OrderByDescending(s => ((decimal?)s.UnitPrice));//den exei ylopoih8ei to price swsta akoma
                //    ViewBag.CurrentSorting = sortOrder;
                //    break;
                //case "Date":
                //            sortPar = sortPar.OrderBy(s => s.);//den exei ylopoih8ei to date akoma
                //    break;
                //case "date_desc":
                //            sortPar = sortPar.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    sortPar = sortPar.OrderBy(s => s.BaseItem.Name);//auto prepei na to vgalv an apofsisv na mhn exw allo sorting
                    ViewBag.CurrentSorting = null;
                    break;
            }


            return View(new ProductViewModel
            {
                Products = sortPar
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize)
                .ToList(),
                //.AsNoTracking()
                //Usage of NoTracking() is recommended when your query is meant for read operations. In these scenarios, you get back your entities but they are not tracked by your context.This ensures minimal memory usage and optimal performance
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)likeItem.Where(p => (p.BaseItem.IsLive ?? false) && (category == null || p.BaseItem.Category == category)).Count() / (double)PageSize)
                },
                CurrentCategory = category
            });
        }

        // GET: One Product that is live
        public async Task<IActionResult> Product(int? id)
        {
            // Checks if the item exists
            if (id == null)
            {
                return NotFound();
            }

            var item = await itemRepository.Items
                                           .SingleOrDefaultAsync(m => (m.IsLive ?? false) && m.Id == id);

            if (item == null)
            {
                return NotFound();
            }


            // Now check if there is a user 

            var name = HttpContext.User.Identity.Name;
            if (name != null)
            {
                var currentuser = await manager.FindByNameAsync(name).ConfigureAwait(false);

                // Get the id from this database and check if this object is in the wishlist
                var use = await userep.RetrieveByExternalAsync(currentuser.Id);
                bool isLiked = await wishRepository.Wishes.AnyAsync(a => a.UserId == use.Id && a.ItemId == id);
                if (isLiked)
                {
                    return View(new LikeItem(item,true));
                }
                else
                {
                   return View(new LikeItem(item, false));
                }

            }
            else
            {
                return View(new LikeItem(item, false));
            }
            
        }

    }
}

