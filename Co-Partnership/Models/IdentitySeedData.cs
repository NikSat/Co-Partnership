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
        private const string UserAdmin = "admin@cooperation.com";
        private const string PasswordAdmin = "SecretAdmin1!";

        private const string UserMember = "member@cooperation.com";
        private const string PasswordMember = "SecretMember1!";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            IServiceScopeFactory scopeFactory =
            app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {

                RoleManager<IdentityRole> roleManager = 
                    scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                UserManager<ApplicationUser> userManager =
                    scope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();


                //Check that there is an Admin role and create if not
                var hasAdminRole = await roleManager.RoleExistsAsync("Admin");

                if (!hasAdminRole)
                {
                    var roleresult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                //Check that there is a Member role and create if not
                var hasMemberRole = await roleManager.RoleExistsAsync("Member");

                if (!hasMemberRole)
                {
                    await roleManager.CreateAsync(new IdentityRole("Member"));
                }

                //Check that there is a SimpleUser role and create if not
                var hasSimpleUserRole = await roleManager.RoleExistsAsync("SimpleUser");

                if (!hasSimpleUserRole)
                {
                    await roleManager.CreateAsync(new IdentityRole("SimpleUser"));
                }

                //Check if the admin user exists and create it if not
                //Add to the Admin role
                var userA = await userManager.FindByEmailAsync(UserAdmin);

                if (userA == null)
                {
                    userA = new ApplicationUser()
                    {
                        Email = UserAdmin,
                        UserName = UserAdmin,
                        EmailConfirmed = true
                    };

                    var newUser = await userManager.CreateAsync(userA, PasswordAdmin);

                    if (newUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userA, "Admin");
                    }
                }

                //Check if the member user exists and create it if not
                //Add to the Member role
                var userM = await userManager.FindByEmailAsync(UserMember);

                if (userM == null)
                {
                    userM = new ApplicationUser()
                    {
                        Email = UserMember,
                        UserName = UserMember,
                        EmailConfirmed = true
                    };

                    var user = await userManager.CreateAsync(userM, PasswordMember);
                    if (user.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userM, "Member");
                    }
                }
            }
        }
    }
}

