using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Co_Partnership.Models;
using Microsoft.EntityFrameworkCore;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Co_Partnership.Controllers
{

    public class HomeController : Controller
    {
        private readonly IItemRepository _itemRepo;
        private readonly UserManager<ApplicationUser> manager;
        private IUserRepository userep;
        private IWishRepository wishRepository;

        public HomeController(
            IItemRepository itemRepo, 
            UserManager<ApplicationUser> umngr, 
            IUserRepository userRepository, 
            IWishRepository wRpstr)
        {
            _itemRepo = itemRepo;
            manager = umngr;
            userep = userRepository;
            wishRepository = wRpstr;
        }

        public async Task<ViewResult>  Index()
        {
            // First, get the top items
            var Topitems = await _itemRepo.GetTop().Where(p => (p.Product.IsLive ?? false)).ToListAsync();
            
            // The list of likeitems
            List<TopLikeItem> LikeItemList = new List<TopLikeItem>();
            // Fill the list with list items nobody likes
            foreach (TopProductsModel it in Topitems)
            {
                TopLikeItem Like = new TopLikeItem(it);
                LikeItemList.Add(Like);
            }
            
            // Check if there is a user logged in
            var name = HttpContext.User.Identity.Name;
            if (name != null)
            {
                var currentuser = await manager.FindByNameAsync(name).ConfigureAwait(false);
                
                // Get the id from this database
                var use = await userep.RetrieveByExternalAsync(currentuser.Id);

                // The list of integers
                List<int?> list = new List<int?>();

                list = await wishRepository.Wishes.Where(a => a.UserId == use.Id).Select(selector: b => b.ItemId).ToListAsync();

                // From this list got to the like items and change the items to liked
                // First check if list is empty ie the user has no favored any items if
                if (!list.Any())
                {
                    return View(LikeItemList);

                }
                else
                {
                    foreach (int iid in list)
                    {
                        foreach (TopLikeItem litem in LikeItemList)
                        {
                            if (litem.Product.Product.Id == iid)
                            {
                                litem.IsLiked = true;
                            }
                        }
                    }
                    return View(LikeItemList);
                }
            }
            else
            {
                return View(LikeItemList);
            }           
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Welcome to our Co-Partnership";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Personal()
        {
            return View();
        }

        [Authorize]
        public IActionResult Messages()
        {
            return View();
        }
    }
}
