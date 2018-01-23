using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;


namespace Co_Partnership.Services
{
    interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void UpdateUser(User user);

        void SaveUser(User user);

        User DeleteUser(int userId);

    }
}
