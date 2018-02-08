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

        Task UpdateUserAsync(User user);

        Task SaveUserAsync(User user);

        Task<User> DeleteUserAsync(int userId);

        Task CreateUserAsync(string extId, int userType, string fisrtName, string lastName);

        Task<User> RetrieveByExternalAsync(string extId);

        int GetUserFromIdentity(string userId);


        string GetEmail(string userId);

        string GetUsername(string userId);

        string GetName(int userId);

        decimal GetBalance(int userId);
    }
}
