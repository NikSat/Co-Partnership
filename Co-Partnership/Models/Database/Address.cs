using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Co_Partnership.Models.Database
{
    public partial class Address
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TransactionId { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address1 { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Zip { get; set; }

        public Transaction Transaction { get; set; }
        public User User { get; set; }
    }
}
