using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;
using Microsoft.EntityFrameworkCore;

namespace Co_Partnership.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private Co_PartnershipContext db;

        public TransactionRepository(Co_PartnershipContext db)
        {
            this.db = db;
        }


        public IQueryable<Transaction> Transactions => db.Transaction
                                                               .Include(a => a.Owner)
                                                               .Include(b => b.TransactionItem)
                                                                    .ThenInclude(TransactionItem =>TransactionItem.Item);

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


        // This function only returns the needed elements of a transaction ie id , owner name, date and price 
        public IEnumerable<Object> ListTransactions(int type)
        {
            var ListOrder =
                from Transaction in Transactions
                where Transaction.Type == type && Transaction.IsProcessed == 0
                select new
                {
                    OrderId = Transaction.Id,
                    SenderName = Transaction.Owner.FirstName + " " + Transaction.Owner.LastName,
                    OrderDate = Transaction.Date,
                    OrderPrice = Transaction.Price,
                };
            return ListOrder;
        }



        // This function counts the new transactions by type
        public int NewTransactionCount(int type)
        {
            var ListTrans =
                from transaction in Transactions
                where transaction.Type == type && transaction.IsProcessed == 0
                select transaction;
            return ListTrans.Count();

        }




        // This function returns the total items in all unprocessed transactions of the same type
        public double? CountItems (int type)
        {
            /// To implement later
            double? count = 0;
            var ListTrans =
                from Transaction in Transactions
                where Transaction.Type == type && Transaction.IsProcessed == 0
                select Transaction;
            foreach (Transaction trans in ListTrans)
            {
                foreach (TransactionItem transitem in trans.TransactionItem)
                {
                    count += transitem.Quantinty;
                }
            }

            return count;

        }




        // This function returns the items of each order or offer
        public IEnumerable<Object> ListItems(int orderId)
        {
            // Get the transaction
            Transaction transaction = Transactions.FirstOrDefault(a => a.Id == orderId);
            if (transaction == null)
            {
                return null;
            }


            // From this transaction get the items, quantity, price, total price etc 
            var ItemList =
               from TransactionItem in transaction.TransactionItem
               select new
               {
                   ItemName = TransactionItem.Item.Name,
                   ItemCategory = TransactionItem.Item.Category,
                   ItemQuantity = TransactionItem.Quantinty.Value,
                   ItemPrice = TransactionItem.Item.UnitPrice.Value,
                   ItemType= TransactionItem.Item.UnitType,
                   ItemFullPrice = (decimal)TransactionItem.Quantinty.Value * TransactionItem.Item.UnitPrice.Value,
                   ItemAcceptance=TransactionItem.Acceptance
               };
            return ItemList;
        }


    }
}
