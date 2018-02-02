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
        Task SaveWishAsync(WishList wish);

        Task<WishList> DeleteWishAsync(int wishId);

        Task<WishList> DeleteWishAsync(int itemId, int userId);

        Task DeleteWishAsync(WishList weee);

    }
}
