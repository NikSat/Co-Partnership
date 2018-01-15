using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Payment
    {
        public Payment()
        {
            PersonalFund = new HashSet<PersonalFund>();
        }

        public int Id { get; set; }
        public int? PfId { get; set; }
        public int? FundId { get; set; }
        public decimal? Amount { get; set; }

        public Fund Fund { get; set; }
        public ICollection<PersonalFund> PersonalFund { get; set; }
    }
}
