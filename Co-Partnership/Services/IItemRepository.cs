using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    // A form of storage for the items to be used by the adminstrator
    public interface IItemRepository
    {

        IQueryable<Item> Items { get; }

        void UpdateItem(Item item);

        void SaveItem(Item item);

        Item DeleteItem(int itemId);

        Item GetItem(int itemId);

        IQueryable<TopProductsModel> GetTop();
    }

    
}
