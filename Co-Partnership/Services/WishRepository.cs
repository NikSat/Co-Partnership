using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;
using Microsoft.EntityFrameworkCore;


namespace Co_Partnership.Services
{
    public class WishRepository:IWishRepository
    {
        // The database through injection
        private Co_PartnershipContext db;

        // The Constructor
        public WishRepository (Co_PartnershipContext db)
        {
            this.db = db;
        }


        public IQueryable<WishList> Wishes => db.WishList
                                                    .Include(a => a.User)
                                                    .Include(b => b.Item);

        // Might not be needed, we will see

        public async Task<WishList> DeleteWishAsync(int wishId)
        {
            WishList wish = await db.WishList.FirstOrDefaultAsync(p => p.Id == wishId);

            if (wish != null)
            {
                db.WishList.Remove(wish);
                await db.SaveChangesAsync();
            }

            return wish;

        }

        // This function deletes the item from the wishlist depending on the id of the item
        public async Task<WishList> DeleteWishAsync(int itemId, int userId)
        {
            WishList wish = await db.WishList.FirstOrDefaultAsync(p => p.ItemId == itemId && p.UserId == userId);

            if (wish != null)
            {
                db.WishList.Remove(wish);
                await db.SaveChangesAsync();
            }

            return wish;

        }

        // This function deletes the item from the wishlist - accepts wishlist as item input
        public async Task DeleteWishAsync(WishList weee)
        {
            db.WishList.Remove(weee);
            await db.SaveChangesAsync();
        }



        //This function takes a wishlist and saves it
        public async Task SaveWishAsync(WishList wish)
        {
            db.WishList.Add(wish);
            await db.SaveChangesAsync();

        }


        // This function gets a summary of the wished items
        public IQueryable<Object> GetWishSummary (int userId)
        {
            var WishSummary =
                from Wished in Wishes
                where Wished.UserId == userId
                select new
                {
                    Id = Wished.ItemId,
                    Name = Wished.Item.Name,
                    Category = Wished.Item.Category,
                    IsLive = Wished.Item.IsLive                    
                };
            return WishSummary;
        }
    }
}
