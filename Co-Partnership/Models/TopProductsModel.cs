using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class TopProductsModel
   {
       public Item Product{ get; set; }
        public int NumberOfOrders { get; set; }
    }
}
