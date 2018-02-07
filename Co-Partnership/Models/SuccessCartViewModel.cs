using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class SuccessCartViewModel
    {
        public decimal PriceNoVAT { get; set; }
        public List<TransactionItem> TransactionItems { get; set; }
    }
}
