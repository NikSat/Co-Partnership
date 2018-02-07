using Co_Partnership.Data;
using Co_Partnership.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public class AddressRepository : IAddressRepository
    {
        private Co_PartnershipContext db;
        public AddressRepository(Co_PartnershipContext dbContext)
        {
            db = dbContext;
        }

        public IQueryable<Address> AddressRepo =>
            db.Address
            .Include(t => t.Transaction)
            .Include(u => u.User);

        public void SaveAddress(Address address)
        {
            db.Address.Add(address);
            db.SaveChanges();
        }

    }
}
