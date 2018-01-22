using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class IdentitySeedData
    {
        private const string UserAdmin = "Admin";
        private const string PasswordAdmin = "Admin1";

        private const string UserMember = "Member";
        private const string PasswordMember = "Member1";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<IdentityUser> userManager = app
                .ApplicationServices
                .GetRequiredService<UserManager<IdentityUser>>();

            RoleManager<IdentityRole> roleManager = app
                .ApplicationServices
                .GetRequiredService<RoleManager<IdentityRole>>();

            IdentityUser userA = await userManager.FindByNameAsync(UserAdmin);
            IdentityUser userM = await userManager.FindByNameAsync(UserMember);


            if (userA == null)
            {
                userA = new IdentityUser("admin");
                await userManager.CreateAsync(userA, PasswordAdmin);
            }

            if (userM == null)
            {
                userM = new IdentityUser("member");
                await userManager.CreateAsync(userM, PasswordMember);
            }

            IdentityRole adminRole = await roleManager.FindByIdAsync("Admin");
            IdentityRole memberRole = await roleManager.FindByIdAsync("Member");
            IdentityRole simpleUserRole = await roleManager.FindByIdAsync("SimpleUser");

            if (adminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await userManager.AddToRoleAsync(userA, "Admin");
            }
            if (memberRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Member"));
                await userManager.AddToRoleAsync(userM, "Member");
            }
            if (simpleUserRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("SimpleUser"));
                var simpleUser = await userManager.FindByNameAsync("User1");
                await userManager.AddToRoleAsync(simpleUser, "SimpleUser");
            }
        }
    }
}
