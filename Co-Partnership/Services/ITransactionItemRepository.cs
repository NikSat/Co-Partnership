using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    interface ITransactionItemRepository
    {
        IQueryable<TransactionItem> TIRepository { get; }

        void UpdateItem(TransactionItem item);

        void SaveItem(TransactionItem item);

        TransactionItem DeleteItem(int itemId);

    }
}
