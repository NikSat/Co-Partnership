using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Fund
    {
        public Fund()
        {
            Offer = new HashSet<Offer>();
            Order = new HashSet<Order>();
            Payment = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public decimal? CoopShare { get; set; }
        public decimal? MemberShare { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }

        public ICollection<Offer> Offer { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<Payment> Payment { get; set; }
    }
}
