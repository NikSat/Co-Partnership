using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    interface IPersonalAccountRepository
    {
        IQueryable<PersonalFinancialAccount> Account { get; }

        void UpdateAccount(PersonalFinancialAccount account);
    }
}
