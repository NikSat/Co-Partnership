using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    internal interface IAdministratorRepository : IFinanceRepository, IUserRepository, IItemRepository, IMessageInterface
    {


    }
}
