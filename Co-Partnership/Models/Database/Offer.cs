using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Offer
    {
        public Offer()
        {
            OfferedItems = new HashSet<OfferedItems>();
        }

        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int? AdminId { get; set; }
        public bool? Prossesed { get; set; }
        public int? FoundId { get; set; }
        public decimal? Price { get; set; }

        public User Admin { get; set; }
        public Fund Found { get; set; }
        public User Member { get; set; }
        public ICollection<OfferedItems> OfferedItems { get; set; }
    }
}
