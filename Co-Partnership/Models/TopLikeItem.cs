using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class TopLikeItem
    {
        public TopProductsModel Product { get; set; }
        public bool IsLiked { get; set; }

        public TopLikeItem(TopProductsModel item)
        {
            Product = item;
            IsLiked = false;

        }

        public TopLikeItem(TopProductsModel item, bool isLiked)
        {
            Product = item;
            IsLiked = isLiked;
        }

    }
}
