using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;
using Microsoft.EntityFrameworkCore;

namespace Co_Partnership.Services
{
    public class UserRepository : IUserRepository
    {
        private Co_PartnershipContext db;

        public UserRepository(Co_PartnershipContext db)
        {
            this.db = db;
        }

        public IQueryable<User> Users => db.User; 

        public async Task CreateUserAsync(string extId,int userType, string fisrtName, string lastName)
        {
            User user = new User()
            {
                ExtId=extId,
                UserType=userType,
                FirstName=fisrtName,
                LastName=lastName
            };
            await SaveUserAsync(user);
        }

        public async Task<User> DeleteUserAsync(int userId)
        {
            User user = db.User.FirstOrDefault(p => p.Id == userId);

            if (user != null)
            {
                db.User.Remove(user);
                await db.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> RetrieveByExternalAsync(string extId)
        {
            return await Users.FirstOrDefaultAsync(a => a.ExtId == extId);

        }

        public async Task SaveUserAsync(User user)
        {
            await db.User.AddAsync(user);
            await db.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            db.Update(user);
            await db.SaveChangesAsync();
        }
    }
}
