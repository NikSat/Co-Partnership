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
    [Route("/Products/Index/api/Wishlist")]
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


        // This function gets the user Id
        public async Task<int> GetUserId()
        {
            var currentuser = await manager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);

            var users = userep.Users;
            return users.SingleOrDefault(a => a.ExtId == currentuser.Id).Id;
        } 



        // This function gets all the objects from the wishlist for this user
        // GET: api/ProductsApi
        [HttpGet]
        public async Task<IEnumerable<Object>> Get()
        {

            var user = await manager.FindByNameAsync(HttpContext.User.Identity.Name);

            int BId = await GetUserId();

            return wishRepository.Wishes.Where(a => a.UserId == BId);
        }


        // This function checks if the item is already in the wishlist and returns it, gets as input the item id and cross checks with the user
        // GET: api/ProductsApi/5
        [HttpGet("{id}")]
        public async Task<Object> Get(int itemid)
        {
            int userid = await GetUserId();
            return wishRepository.Wishes.FirstOrDefault(a => a.ItemId==itemid  && a.UserId == userid);
        }
        

        // This one creates a new wishlist item from a selection 
        // POST: api/ProductsApi
        [HttpPost]
        public async void Post([FromBody]WishList wish)
        {
            int userid = await GetUserId();
            // Check if this wished item already exist in the wishlist
            var checkwish = wishRepository.Wishes.FirstOrDefault(a=> a.ItemId==wish.ItemId && a.UserId==userid);
            if (checkwish == null)
            {
                // If it does not exist add it to the list 
                wish.UserId = userid;
                wishRepository.SaveWish(wish);
            }
            else
            {
                // If it exists delete it
                wishRepository.DeleteWish(wish.Id, userid);
            }


        }

        // This one deletes the list item based on the item id and current user
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            int userid = await GetUserId();
            wishRepository.DeleteWish(id,userid);

        }
    }
}
