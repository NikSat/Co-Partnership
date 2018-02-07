using Co_Partnership.Models.Database;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class OfferViewModel
    {
        public Item Item { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
