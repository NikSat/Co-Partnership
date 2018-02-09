using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Co_Partnership.Models.Database
{
    public partial class Item
    {
        public Item()
        {
            TransactionItem = new HashSet<TransactionItem>();
            WishList = new HashSet<WishList>();
        }

        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(15)]
        [Required]
        public string Name { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(15)]
        [Required]
        public string Category { get; set; }
        [StringLength(255)]
        [Required]
        public string Description { get; set; }

        [Range(1, 500)]
        [Required]
        public double? StockQuantity { get; set; }
        [Required]
        public bool? IsLive { get; set; }
        
        public string Image { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Required]
        public string UnitType { get; set; }

        [Range(0.2, 800)]
        [DisplayFormat(DataFormatString = "{0:n} €")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal? UnitPrice { get; set; }

        public ICollection<TransactionItem> TransactionItem { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
