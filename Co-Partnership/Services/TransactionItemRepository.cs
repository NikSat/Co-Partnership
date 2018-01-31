using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;

namespace Co_Partnership.Services
{
    public class TransactionItemRepository : ITransactionItemRepository
    {
        // Set the database needed to get the products
        private Co_PartnershipContext db;

        // The Constructor
        public TransactionItemRepository(Co_PartnershipContext db)
        {
            this.db = db;
        }

        // Get a queryable for items get all, apply queries at the controller level
        public IQueryable<TransactionItem> TIRepository => db.TransactionItem;

        public TransactionItem DeleteItem(int itemId)
        {
            TransactionItem ite = db.TransactionItem.FirstOrDefault(p => p.Id == itemId);

            if (ite != null)
            {
                db.TransactionItem.Remove(ite);
                db.SaveChanges();
            }

            return ite;

        }

        public void SaveItem(TransactionItem item)
        {
            db.TransactionItem.Add(item);
            db.SaveChanges();
        }

        public void UpdateItem(TransactionItem item)
        {
            db.Update(item);
            db.SaveChanges();
        }
    }
}
