using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    public interface ICompAccountRepository
    {

        CompanyFinancialAccount Account { get; }

        void UpdateAccount(CompanyFinancialAccount account);

        decimal GetCoopShare();
    }
}
