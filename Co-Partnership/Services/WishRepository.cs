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

        public WishList DeleteWish(int wishId)
        {
            WishList wish = db.WishList.FirstOrDefault(p => p.Id == wishId);

            if (wish != null)
            {
                db.WishList.Remove(wish);
                db.SaveChanges();
            }

            return wish;

        }


        public WishList DeleteWish(int itemId, int userId)
        {
            WishList wish = db.WishList.FirstOrDefault(p => p.ItemId == itemId && p.UserId == userId);

            if (wish != null)
            {
                db.WishList.Remove(wish);
                db.SaveChanges();
            }

            return wish;

        }

        public void SaveWish(WishList wish)
        {
            db.WishList.Add(wish);
            db.SaveChanges();

        }
    }
}
