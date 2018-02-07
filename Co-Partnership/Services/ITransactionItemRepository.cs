using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    public interface ITransactionItemRepository
    {
        IQueryable<TransactionItem> TIRepository { get; }

        void UpdateItem(TransactionItem item);

        void AddOrUpdate(TransactionItem item);

        TransactionItem DeleteItem(TransactionItem item);

        List<TransactionItem> GetTransactionItems(int transactionId);

        TransactionItem GetItem(int transactionId, int? itemId);

        void CreateDbCart(int userid);

        void SaveCartToDB(int userid);

        void LoginMergeCart(int userid);
    }
}
