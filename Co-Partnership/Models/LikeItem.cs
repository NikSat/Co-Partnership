using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Models
{
    public class LikeItem
    {
        public Item BaseItem { get; set; }

        public bool IsLiked { get; set; }

        public LikeItem(Item item)
        {
            BaseItem = item;
            IsLiked = false;

        }

    }
}
