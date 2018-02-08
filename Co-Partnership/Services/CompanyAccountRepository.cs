using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;

namespace Co_Partnership.Services
{
    public class CompanyAccountRepository : ICompAccountRepository
    {
        // Set the database needed to get the products
        private Co_PartnershipContext db;

        // The Constructor
        public CompanyAccountRepository(Co_PartnershipContext db)
        {
            this.db = db;
        }

        public CompanyFinancialAccount Account => db.CompanyFinancialAccount.FirstOrDefault(a => a.Id == 1);

        public void UpdateAccount(CompanyFinancialAccount account)
        {
            db.Update(account);
            db.SaveChanges();
        }

        public decimal GetCoopShare()
        {
            return (decimal)Account.CoOpShare;
        }

        public void UpdateCoopBalance(decimal coopbalance)
        {
            Account.CoOpShare = coopbalance;
            Account.Date = DateTime.Today;
            db.Update(Account);
            db.SaveChanges();
        }
    }
}
