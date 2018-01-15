using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class PersonalFund
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public int? MemberId { get; set; }
        public int? PaymentId { get; set; }

        public User Member { get; set; }
        public Payment Payment { get; set; }
    }
}
