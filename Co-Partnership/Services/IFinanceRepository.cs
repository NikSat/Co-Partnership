using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;


namespace Co_Partnership.Services
{
    interface IFinanceRepository
    {
        IQueryable<Fund> Funds { get; }
        IQueryable<Order> Orders { get; }
        IQueryable<Offer> Offers { get; }
        IQueryable<Payment> Payments { get; }

        void UpdateOrder(Order order);

        void UpdateOffer(Offer offer);

        void MakePayment(Payment payment);

        void MakeTransfer(Fund fund);
        
    }
}
