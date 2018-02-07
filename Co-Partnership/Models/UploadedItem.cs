using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class UploadedItem: Models.Database.Item
    {
        [Required]
        public new string Name { get; set; }
        [Required]
        public new string Category { get; set; }
        [Required]
        public new string Description { get; set; }
        [Required]
        public new double? StockQuantity { get; set; }
        [Required]
        public new bool? IsLive { get; set; }

        [Required]
        public new IFormFile Image { get; set; }
        [Required]
        public new string UnitType { get; set; }
        [Required]
        public new decimal? UnitPrice { get; set; }
    }
}
