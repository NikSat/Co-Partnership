using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class OfferedItems
    {
        public int Id { get; set; }
        public int? OfferId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public bool? Acceptance { get; set; }

        public Item Item { get; set; }
        public Offer Offer { get; set; }
    }
}
