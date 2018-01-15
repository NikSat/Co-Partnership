using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class WishList
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ItemId { get; set; }

        public Item Item { get; set; }
        public User User { get; set; }
    }
}
