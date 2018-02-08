using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class ClientsViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int NumberOfOrders { get; set; }
        public decimal? SumOfPayments { get; set; }
        public int? UserType { get; set; }
    }
}
