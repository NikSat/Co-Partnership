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


        public IQueryable<Transaction> Transactions
            => db.Transaction
            .Include(t => t.Owner)
            .Include(t => t.Recipient)
            .Include(t => t.TransactionItem)
            .ThenInclude(t => t.Item);

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


        public Transaction SaveTransaction(Transaction transaction)
        {
            db.Transaction.Add(transaction);
            db.SaveChanges();
            return transaction;
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
        public double? CountItems(int type)
        {
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
                   ItemType = TransactionItem.Item.UnitType,
                   ItemFullPrice = (decimal)TransactionItem.Quantinty.Value * TransactionItem.Item.UnitPrice.Value,
                   ItemAcceptance = TransactionItem.Acceptance
               };
            return ItemList;
        }

        // This function returns the cart or the incomplete order
        public Transaction GetIncompleteTransaction(int userId)
        {
            var incomplete = Transactions.FirstOrDefault(t => t.OwnerId == userId && t.Type == 0);
            return incomplete;
        }

        public IEnumerable<Object> GetPurchaseHistory(int userId)
        {
            var PurchaseList =
               from Transaction in Transactions
               where Transaction.OwnerId == userId && Transaction.IsProcessed == 1 && Transaction.Type == 1
               select new
               {
                   OrderId = Transaction.Id,
                   OrderDate = Transaction.Date,
                   OrderPrice = Transaction.Price,
               };
            return  PurchaseList;
        }


        // This function returns a summary of selected object
        public IEnumerable<Object> getSummaries(int id,DateTime start,DateTime end)
        {
            // First of all check the inputs serverside
            // Id must be one of these numbers
            if (id == 1 || id == 2 || id == 3 || id == 10)
            {
                // No date must be null
                if (start ==null || end==null)
                {
                    return null;
                }

                // End date later than startdate
                if (start>=end)
                {
                    return null;
                }

                // Get all the processed transactions between that time window
                List<Transaction> Summaries = new List<Transaction>();

                foreach (Transaction trans in Transactions)
                {
                    // Make sure processed date has a value
                    if (!trans.DateProcessed.HasValue)
                    {
                        continue;
                    }
                    // Then cast it
                    DateTime time = (DateTime)trans.DateProcessed;
      
                    if (start <= time && end >= time && trans.IsProcessed==1)
                        {
                        // Now select according to the input 10 means select all
                        if (id == 10)
                        {
                            if( trans.Type == 1 || trans.Type==2 || trans.Type == 3)
                            {
                                Summaries.Add(trans);
                            }
                        }
                        // If not 10 select accordingly
                        if (id ==trans.Type )
                        {
                            Summaries.Add(trans);
                        }



                    }
                }

                // Now make the new items
                var Selection =
                    Summaries.Select(a => new
                    {
                        TransId = a.Id,
                        TransDate = a.DateProcessed,
                        TransGoods = a.TransactionItem.Sum(x => x.Quantinty),
                        TransPrice = a.Price,
                        TransType= a.Type
                    });
                return Selection;


            }

            return null;

        }

        // This function gives a summary of new orders or offers
        public Object SummarizeNewTransactions(int type)
        {
            // Get the transactions of each type that are not processed
            IQueryable<Transaction> ThisType = Transactions.Where(a => a.Type == type && a.IsProcessed==0);

            // If no transactions of this type return empty
            if (!ThisType.Any())
            {
                return null;
            }
            int number = ThisType.Count();
            //count the money for each transaction
            decimal money = ThisType.Sum(a => (decimal)a.Price);
            // Check if there are transaction items in each transaction
            double totals=0;
            foreach (Transaction tra in ThisType)
            {
                if (!tra.TransactionItem.Any())
                {
                    continue;
                }
                foreach (TransactionItem traitem in tra.TransactionItem)
                {
                    totals += (double)traitem.Quantinty;
                }

            }
            
            return new
            {
                Number = number,
                TotalPrice = money,
                TotalItems = totals
            };

        }


    }
}
