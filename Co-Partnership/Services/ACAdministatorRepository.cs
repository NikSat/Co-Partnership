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

        // In order to view everyone we need to access both databases
        private CoPartnershipContext db;
        private ApplicationDbContext idedb;


        public IQueryable<Fund> Funds => throw new NotImplementedException();

        public IQueryable<Order> Orders => throw new NotImplementedException();

        public IQueryable<Offer> Offers => throw new NotImplementedException();

        public IQueryable<Payment> Payments => throw new NotImplementedException();

        public IQueryable<Item> Items => throw new NotImplementedException();

        public Item DeleteItem(int itemId)
        {
            throw new NotImplementedException();
        }

        public void MakePayment(Payment payment)
        {
            throw new NotImplementedException();
        }

        public void MakeTransfer(Fund fund)
        {
            throw new NotImplementedException();
        }

        public void SaveItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void UpdateOffer(Offer offer)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
