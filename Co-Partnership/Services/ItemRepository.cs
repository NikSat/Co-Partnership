using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;
using Co_Partnership.Models;
namespace Co_Partnership.Services
{
    public class ItemRepository : IItemRepository
    {
        // Set the database needed to get the products
        private Co_PartnershipContext db;

        // The Constructor
        public ItemRepository (Co_PartnershipContext db)
        {
            this.db = db;
        }

        // Get a queryable for items get all, apply queries at the controller level
        public IQueryable<Item> Items => db.Item;
        public IQueryable<Transaction> Transactions => db.Transaction;
        public IQueryable<TransactionItem> TransactionItems => db.TransactionItem;

        public IQueryable<TopProductsModel> GetTop()
        {
            var topProducts = (from trI in TransactionItems
                              from tr in Transactions
                               from it in Items
                               where tr.IsProcessed==1
                               where trI.Id == tr.Id
                               where it.Id == trI.ItemId
                               group tr by it into productGroups
                              select new TopProductsModel
                              {
                                  Product =productGroups.Key,
                                  NumberOfOrders = productGroups.Count()
                              }).OrderByDescending(x => x.NumberOfOrders).Distinct().Take(4);
            return topProducts;
            //).OrderByDescending(x => x.numberOfOrders).Distinct().Take(10);
            //    return Transactions.OrderByDescending(x => x.).Take(3)
        }

        public Item DeleteItem(int itemId)
        {
            Item ite = db.Item.FirstOrDefault(p => p.Id == itemId);

            if (ite != null)
            {
                db.Item.Remove(ite);
                db.SaveChanges();
            }

            return ite;

        }

        public void SaveItem(Item item)
        {
            db.Item.Add(item);
            db.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            db.Update(item);
            db.SaveChanges();
        }

        public List<string> GetCategories()
        {
            return Items
                .Select(i => i.Category)
                .Distinct()
                .ToList();
        }

        public Item GetItem(int id)
        {
            return Items.FirstOrDefault(i => i.Id == id);
        }
    }
}
