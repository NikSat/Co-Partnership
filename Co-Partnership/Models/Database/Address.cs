using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Address
    {
        public Address()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }

        public User User { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
