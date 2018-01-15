using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Item
    {
        public Item()
        {
            OfferedItems = new HashSet<OfferedItems>();
            OrderItem = new HashSet<OrderItem>();
            WishList = new HashSet<WishList>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public bool? IsLive { get; set; }

        public ICollection<OfferedItems> OfferedItems { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
