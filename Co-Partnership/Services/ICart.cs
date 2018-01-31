using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public interface ICart
    {
        TransactionItem AddItem(int itemId, int quantity);
        
        void RemoveItem(Item item);
        
        decimal ComputeTotalValue();

        decimal? ComputeItemValue(TransactionItem cartItem);

        void Clear();
    }
}
