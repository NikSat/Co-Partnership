using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;


namespace Co_Partnership.Services
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void UpdateMember(User user);

        void SaveMember(User user);

        Item DeleteMember(int userId);

    }
}
