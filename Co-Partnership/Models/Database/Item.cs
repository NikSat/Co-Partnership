using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Item
    {
        public Item()
        {
            TransactionItem = new HashSet<TransactionItem>();
            WishList = new HashSet<WishList>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double? StockQuantity { get; set; }
        public bool? IsLive { get; set; }
        public string Image { get; set; }
        public string UnitType { get; set; }
        public decimal? UnitPrice { get; set; }

        public ICollection<TransactionItem> TransactionItem { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
