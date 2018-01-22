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

        public static async Task EnsurePopulated(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

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

        //public static void CreateRoles(IServiceProvider serviceProvider)
        //{

        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    Task<IdentityResult> roleResult;
            

        //    //Check that there is an Administrator role and create if not
        //    Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Admin");
        //    hasAdminRole.Wait();

        //    if (!hasAdminRole.Result)
        //    {
        //        roleResult = roleManager.CreateAsync(new IdentityRole("Admin"));
        //        roleResult.Wait();
        //    }

        //    //Check if the admin user exists and create it if not
        //    //Add to the Administrator role

        //    Task<ApplicationUser> userA = userManager.FindByEmailAsync(UserAdmin);
        //    userA.Wait();

        //    if (userA.Result == null)
        //    {
        //        ApplicationUser admin = new ApplicationUser()
        //        {
        //            Email = UserAdmin,
        //            UserName = UserAdmin,
        //            EmailConfirmed = true
        //        };

        //        Task<IdentityResult> newUser = userManager.CreateAsync(admin, PasswordAdmin);
        //        newUser.Wait();

        //        if (newUser.Result.Succeeded)
        //        {
        //            Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(admin, "Admin");
        //            newUserRole.Wait();
        //        }
        //    }


        //}
    }
}

