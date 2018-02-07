using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public interface IAddressRepository
    {
        IQueryable<Address> AddressRepo { get; }

        void SaveAddress(Address address);
    }
}
