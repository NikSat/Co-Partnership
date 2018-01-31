using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    public interface IWishRepository
    {
        // As always the querable list of wishes
        IQueryable<WishList> Wishes { get; }

        // Two functions: to add and to remove items from this list
        void SaveWish(WishList wish);

        WishList DeleteWish(int wishId);

        WishList DeleteWish(int itemId, int userId);


    }
}
