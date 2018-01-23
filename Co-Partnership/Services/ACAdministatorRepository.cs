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

        public IQueryable<Transaction> Transactions => db.Transaction;


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



        // Users both members and clients
        #region Users
        public User DeleteUser(int userId)
        {
            User user = db.User.FirstOrDefault(p => p.Id == userId);

            if (user != null)
            {
                db.User.Remove(user);
                db.SaveChanges();
            }

            return user;
        }


        public void SaveUser(User user)
        {
            db.User.Add(user);
            db.SaveChanges();
        }


        public void UpdateUser(User user)
        {
            db.Update(user);
            db.SaveChanges();
        }
        #endregion


        #region Finances
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
        #endregion

        #region Messages
        public void SaveMessage(Message message)
        {
            db.Message.Add(message);
            db.SaveChanges();
        }
        #endregion

    }
}
