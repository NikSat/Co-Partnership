using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Order
    {
        public Order()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? ClientId { get; set; }
        public int? FoundId { get; set; }
        public bool? Completed { get; set; }
        public decimal? Price { get; set; }
        public int? AddressId { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsShipped { get; set; }

        public Address Address { get; set; }
        public User Client { get; set; }
        public Fund Found { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
    }
}
