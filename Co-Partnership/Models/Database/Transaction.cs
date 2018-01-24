using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Transaction
    {
        public Transaction()
        {
            Address = new HashSet<Address>();
            TransactionItem = new HashSet<TransactionItem>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? OwnerId { get; set; }
        public int? RecipientId { get; set; }
        public DateTime? DateProcessed { get; set; }
        public int? IsProcessed { get; set; }
        public decimal? Price { get; set; }
        public int? Type { get; set; }

        public User Owner { get; set; }
        public User Recipient { get; set; }
        public ICollection<Address> Address { get; set; }
        public ICollection<TransactionItem> TransactionItem { get; set; }
    }
}
