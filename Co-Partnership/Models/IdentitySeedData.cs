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
        private const string UserName = "Admin";
        private const string Password = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<IdentityUser> userManager = app
                .ApplicationServices
                .GetRequiredService<UserManager<IdentityUser>>();

            RoleManager<IdentityRole> roleManager = app
                .ApplicationServices
                .GetRequiredService<RoleManager<IdentityRole>>();

            IdentityUser user = await userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                user = new IdentityUser("admin");
                await userManager.CreateAsync(user, Password);
            }

            IdentityRole superAdminRole = await roleManager.FindByIdAsync("SuperAdmin");
            IdentityRole simpleMemberRole = await roleManager.FindByIdAsync("SimpleMemberRole");

            if (superAdminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
            if (simpleMemberRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("simpleMemberRole"));
                var simpleUser = await userManager.FindByNameAsync("User1");
                await userManager.AddToRoleAsync(simpleUser, "simpleMemberRole");
            }
        }
    }
}
