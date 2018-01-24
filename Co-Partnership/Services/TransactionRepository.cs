using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;

namespace Co_Partnership.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private Co_PartnershipContext db;

        public TransactionRepository(Co_PartnershipContext db)
        {
            this.db = db;
        }


        public IQueryable<Transaction> Transactions => db.Transaction;

        public Transaction DeleteTransaction(int transactionId)
        {
            Transaction transaction = db.Transaction.FirstOrDefault(p => p.Id == transactionId);

            if (transaction != null)
            {
                db.Transaction.Remove(transaction);
                db.SaveChanges();
            }

            return transaction;


        }


        public void SaveTransaction(Transaction transaction)
        {
            db.Transaction.Add(transaction);
            db.SaveChanges();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            db.Update(transaction);
            db.SaveChanges();
        }
    }
}
