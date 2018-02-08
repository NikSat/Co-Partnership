using Co_Partnership.Data;
using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public class PersonalAccountRepository :IPersonalAccountRepository
    {
        // Set the database needed to get the products
        private Co_PartnershipContext db;

        // The Constructor
        public PersonalAccountRepository(Co_PartnershipContext db)
        {
            this.db = db;
        }

        public IQueryable<PersonalFinancialAccount> Account => db.PersonalFinancialAccount;

        public void UpdateAccount(PersonalFinancialAccount account)
        {
            db.Update(account);
            db.SaveChanges();
        }

        public void AddAccount(PersonalFinancialAccount account)
        {
            db.PersonalFinancialAccount.Add(account);
            db.SaveChanges();
        }

        public PersonalFinancialAccount GetAccount(int userId)
        {
            return Account.FirstOrDefault(a => a.UserId == userId);
        }
    }
}
