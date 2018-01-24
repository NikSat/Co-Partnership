using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public interface IAdministratorRepository : ITransactionRepository, IUserRepository, IItemRepository, IMessageInterface
    {


    }
}
