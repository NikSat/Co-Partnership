using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public interface ICart
    {
        void AddItem(Item item, int quantity);
        
        void RemoveItem(Item item);
        
        decimal ComputeTotalValue();

        decimal? ComputeItemValue(TransactionItem cartItem);

        void Clear();
    }
}
