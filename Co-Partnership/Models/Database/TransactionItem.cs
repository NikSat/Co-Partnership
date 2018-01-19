using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class TransactionItem
    {
        public int Id { get; set; }
        public int? TransactionId { get; set; }
        public int? ItemId { get; set; }
        public double? Quantinty { get; set; }
        public bool? Acceptance { get; set; }

        public Item Item { get; set; }
        public Transaction Transaction { get; set; }
    }
}
