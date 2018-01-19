using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class CompanyFinancialAccount
    {
        public int Id { get; set; }
        public decimal? CoOpShare { get; set; }
        public decimal? MemberShare { get; set; }
        public DateTime? Date { get; set; }
    }
}
