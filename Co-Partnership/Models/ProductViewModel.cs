using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Services;

namespace Co_Partnership.Models
{
    public class ProductViewModel
    {
        public List<LikeItem> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
