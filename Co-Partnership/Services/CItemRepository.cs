using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;

namespace Co_Partnership.Services
{
    public class CItemRepository : IItemRepository
    {
        // Set the database needed to get the products
        private Co_PartnershipContext db;

        // The Constructor
        public CItemRepository (Co_PartnershipContext db)
        {
            this.db = db;
        }

        // Get a queryable for items get all, apply queries at the controller level
        public IQueryable<Item> Items => db.Item;

       
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
    }
}
