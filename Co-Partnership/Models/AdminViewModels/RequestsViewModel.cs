using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class RequestsViewModel
    {
        public List<Transaction> PendingOrders { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Transaction> PendingOffers { get; set; }
        public int TransactionId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
