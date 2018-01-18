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
        private CoPartnershipContext db;
        private ApplicationDbContext idedb;

        // The Constructor
        public ACAdministatorRepository (CoPartnershipContext db, ApplicationDbContext idedb)
        {
            this.db = db;
            this.idedb = idedb;
        }



        public IQueryable<Fund> Funds => db.Fund;

        public IQueryable<Order> Orders => db.Order;

        public IQueryable<Offer> Offers => db.Offer;

        public IQueryable<Payment> Payments => db.Payment;

        public IQueryable<Item> Items => db.Item;

        public IQueryable<User> Users => db.User; //.Join(idedb.Users, p => p.Id , q=>q. )

        public IQueryable<Message> Messages => db.Message;



        // This part deals with the items

        #region Items
        public Item DeleteItem(int itemId)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void SaveItem(Item item)
        {
            throw new NotImplementedException();
        }
        #endregion


        // This part deals with the orders
        #region Offers
        public void UpdateOffer(Offer offer)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
        #endregion


        // This part deals with payments
        #region Payments 
        public void MakePayment(Payment payment)
        {
            throw new NotImplementedException();
        }

        public void MakeTransfer(Fund fund)
        {
            throw new NotImplementedException();
        }
        #endregion


        // Members panel
        #region Member
        public void UpdateMember(User user)
        {
            throw new NotImplementedException();
        }

        public Item DeleteMember(int userId)
        {
            throw new NotImplementedException();
        }

        public void SaveMember(User user)
        {
            throw new NotImplementedException();
        }
        #endregion


        // Message panel
        #region Member
        public void SaveMessage(Message message)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
