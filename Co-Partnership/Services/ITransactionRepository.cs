using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;


namespace Co_Partnership.Services
{
    public interface ITransactionRepository
    {
        IQueryable<Transaction> Transactions { get; }

        void UpdateTransaction(Transaction transaction);

        void SaveTransaction(Transaction transaction);

        Transaction DeleteTransaction(int transactionId);

        IEnumerable<Object> ListTransactions(int type);

        IEnumerable<Object> ListItems(int orderId);

    }
}
