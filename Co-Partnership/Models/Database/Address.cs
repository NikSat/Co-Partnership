using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Address
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TransactionId { get; set; }
        public string Address1 { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }

        public Transaction Transaction { get; set; }
        public User User { get; set; }
    }
}
