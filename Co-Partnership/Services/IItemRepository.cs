using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    // A form of storage for the items
    interface IItemRepository
    {

        IQueryable<Item> Items { get; }

        void UpdateProduct(Item item);

        void SaveProduct(Item item);

        Item DeleteProduct(int itemId);
    }

    
}
