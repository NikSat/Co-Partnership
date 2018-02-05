using Co_Partnership.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class CartSummaryViewModel 
    {
        public int NumberOfItems { get; set; }
        public decimal Price { get; set; }
    }
}
