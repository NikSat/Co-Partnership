using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Data;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    public class ACAdministatorRepository : IAdministratorRepository
    {


        // In order to view everyone also names etc we need to access both databases
        private Co_PartnershipContext db;
        private ApplicationDbContext idedb;

        // The Constructor receives the databases
        public ACAdministatorRepository(Co_PartnershipContext db, ApplicationDbContext idedb)
        {
            this.db = db;
            this.idedb = idedb;
        }


        public IQueryable<Item> Items => db.Item;

        public IQueryable<User> Users => db.User; //.Join(idedb.Users, p => p.Id , q=>q. )

        public IQueryable<Message> Messages => db.Message;

        public IQueryable<Transaction> Transactions => throw new NotImplementedException();


        // This part deals with the items
        #region Items
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
        #endregion




        #region Members
        public Item DeleteMember(int userId)
        {
            throw new NotImplementedException();
        }


        public void SaveMember(User user)
        {
            throw new NotImplementedException();
        }


        public void UpdateMember(User user)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Finances
        public Item DeleteTransaction(int transactionId)
        {
            throw new NotImplementedException();
        }


        public void SaveTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Messages
        public void SaveMessage(Message message)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
