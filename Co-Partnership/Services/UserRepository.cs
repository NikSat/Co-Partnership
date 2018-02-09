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
        private ApplicationDbContext identityDb;

        public UserRepository(Co_PartnershipContext db, ApplicationDbContext applicationDbContext)
        {
            this.db = db;
            identityDb = applicationDbContext;
        }

        public IQueryable<User> Users => db.User.Include(u => u.TransactionOwner); 

        public async Task CreateUserAsync(string extId,int userType, string fisrtName, string lastName, bool isActive)
        {
            User user = new User()
            {
                ExtId = extId,
                UserType = userType,
                FirstName = fisrtName,
                LastName = lastName,
                IsActive = isActive
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

        //Get user from identity table
        public int GetUserFromIdentity(string userId)
        {
            return Users.FirstOrDefault(u => u.ExtId == userId).Id;
        }

        public string GetEmail(string userId)
        {
            var user = identityDb.Users.FirstOrDefault(u => u.Id == userId);
            if(user != null)
            {
                return user.Email;
            }
            return null;
        }

        public string GetUsername(string userId)
        {
            var user = identityDb.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return user.UserName;
            }
            return null;
        }

        public string GetName(int userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if(user != null)
            {
                return user.FirstName + " " + user.LastName;
            }
            return null;
        }

        public decimal GetBalance(int userId)
        {
            var account = db.PersonalFinancialAccount.FirstOrDefault(a => a.UserId == userId);
            if(account != null)
            {
                return (decimal)account.Amount;
            }
            return 0m;
        }
    }
}
