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
    [Route("/api/Wishlist")]
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



        // This function gets all the objects from the wishlist for this user
        // GET: api/ProductsApi
        [HttpGet]
        public async Task<IEnumerable<Object>> Get()
        {


            int BId = await GetUserId();

            return wishRepository.Wishes.Where(a => a.UserId == BId);
        }


        // This function checks if the item is already in the wishlist and returns it, gets as input the item id and cross checks with the user
        // GET: api/ProductsApi/5
        [HttpGet("{id}")]
        public async Task<Object> Get(int itemid)
        {
            int userid = await GetUserId();
            return await wishRepository.Wishes.FirstOrDefaultAsync(a => a.ItemId==itemid  && a.UserId == userid);
        }
        

        // This one creates a new wishlist item from a selection 
        // POST: api/ProductsApi
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

        // This one deletes the list item based on the item id and current user
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            int userid = await GetUserId();
            await wishRepository.DeleteWishAsync(id,userid);

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
