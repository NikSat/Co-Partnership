using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;


namespace Co_Partnership.Services
{
    interface IFinanceRepository
    {
        IQueryable<Transaction> Transactions { get; }

        void UpdateTransaction(Transaction transaction);

        void SaveTransaction(Transaction transaction);

        Item DeleteTransaction(int transactionId);

    }
}
