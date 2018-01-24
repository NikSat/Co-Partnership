using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;

namespace Co_Partnership.Services
{
    public class UserRepository : IUserRepository
    {
        private Co_PartnershipContext db;

        public UserRepository(Co_PartnershipContext db)
        {
            this.db = db;
        }


        public IQueryable<User> Users => db.User; //.Join(idedb.Users, p => p.Id , q=>q. )

        public User DeleteUser(int userId)
        {
            User user = db.User.FirstOrDefault(p => p.Id == userId);

            if (user != null)
            {
                db.User.Remove(user);
                db.SaveChanges();
            }

            return user;
        }


        public void SaveUser(User user)
        {
            db.User.Add(user);
            db.SaveChanges();
        }


        public void UpdateUser(User user)
        {
            db.Update(user);
            db.SaveChanges();
        }
    }
}
