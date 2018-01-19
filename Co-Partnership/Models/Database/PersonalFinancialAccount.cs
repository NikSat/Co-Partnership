using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class PersonalFinancialAccount
    {
        public int UserId { get; set; }
        public decimal? Amount { get; set; }

        public User User { get; set; }
    }
}
