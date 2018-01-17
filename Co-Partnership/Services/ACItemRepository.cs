using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;

namespace Co_Partnership.Services
{
    public class ACItemRepository : IItemRepository
    {
        private CoPartnershipContext db;

        public ACItemRepository (CoPartnershipContext db)
        {
            this.db = db;
        }

        public IQueryable<Item> Items => db.Item;

        public Item DeleteProduct(int itemId)
        {
            Item ite = db.Item.FirstOrDefault(p => p.Id == itemId);

            if (ite != null)
            {
                db.Item.Remove(ite);
                db.SaveChanges();
            }

            return ite;

        }

        public void SaveProduct(Item item)
        {
            db.Update(item);
            db.SaveChanges();
        }

        public void UpdateProduct(Item item)
        {
            db.Item.Add(item);
            db.SaveChanges();
        }
    }
}
