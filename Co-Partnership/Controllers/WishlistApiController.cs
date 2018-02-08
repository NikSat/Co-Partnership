using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


using Co_Partnership.Models;
using Co_Partnership.Services;
using Co_Partnership.Models.Database;



namespace Co_Partnership.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class WishlistApiController : Controller
    {

        // We use the usermanager information for the user
        private UserManager<ApplicationUser> manager;

        private IWishRepository wishRepository;

        private IUserRepository userep;

        // Constructor for the controller
        public WishlistApiController(UserManager<ApplicationUser> mngr, IWishRepository wRpstr, IUserRepository us)
        {
            manager = mngr;
            wishRepository = wRpstr;
            userep = us;
        }


        // This function gets the user Id of the current user
        public async Task<int> GetUserId()
        {
            var currentuser = await manager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);


            var use= await userep.RetrieveByExternalAsync(currentuser.Id);
            return use.Id;
        } 






        // This one creates a new wishlist item from a selection 
        // POST: api/ProductsApi
        [Route("/api/Wishlist/Toggle")]
        [HttpPost]
        public async Task Post([FromBody]WishList wish)
        {
            int userid = await GetUserId();
            // Check if this wished item already exist in the wishlist
            var checkwish = await wishRepository.Wishes.FirstOrDefaultAsync(a=> a.ItemId==wish.ItemId && a.UserId==userid);
            if (checkwish == null)
            {
                // If it does not exist add it to the list 
                wish.UserId = userid;
                await wishRepository.SaveWishAsync(wish);
            }
            else
            {
                // If it exists delete it
                await wishRepository.DeleteWishAsync(checkwish);
            }


        }




        // This function gives a summary of the wishlist items
        [Route("/api/Wishlist/Summary")]
        [HttpPost]
        public async Task<IQueryable<object>> GiveSummary()
        {
            int userid = await GetUserId();
            return wishRepository.GetWishSummary(userid);
        }



    }
}
