using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Co_Partnership.Models
{
    public class SeeTransactionsViewModel
    {
        public List<Transaction> Orders { get; set; }
        public List<Transaction> Offers { get; set; }
        public List<Transaction> SalesShare { get; set; }
        public List<Transaction> DeclinedOrders { get; set; }
        public List<Transaction> DeclinedOffers { get; set; }
    }
}
