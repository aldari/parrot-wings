using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PW.Core.Account.Domain;
using PW.DataAccess.ApplicationData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PW.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILoggerFactory logger)
        {
            // Create the Db if it doesn’t exist
            context.Database.Migrate();

            // Look for any users.
            /*if (context.Users.Any())
            {
                return;   // DB has been seeded
            }*/

            ILogger log = logger.CreateLogger("DbInitializer");

            //Create Roles (if they doesn't exist yet)
            if (!await roleManager.RoleExistsAsync(RolesList.AdministratorRole))
                await roleManager.CreateAsync(new IdentityRole(RolesList.AdministratorRole));
            if (!await roleManager.RoleExistsAsync(RolesList.RegisteredRole))
                await roleManager.CreateAsync(new IdentityRole(RolesList.RegisteredRole));
            if (!await roleManager.RoleExistsAsync(RolesList.SuperuserRole))
                await roleManager.CreateAsync(new IdentityRole(RolesList.SuperuserRole));

            // Create the "Admin" ApplicationUser account (if it doesn't exist already)
//            var userAdmin = new ApplicationUser()
//            {
//                UserName = "Admin",
//                Email = "admin@gm",
//            };

            // Insert "Admin" into the Database and also assign the "Administrator" role to him.
//            if (await userManager.FindByIdAsync(userAdmin.Id) == null)
//            {
//                await userManager.CreateAsync(userAdmin, "123456");
//                await userManager.AddToRoleAsync(userAdmin, RolesList.AdministratorRole);
//                // Remove Lockout and E-Mail confirmation.
//                userAdmin.EmailConfirmed = true;
//                userAdmin.LockoutEnabled = false;
//
//                await userManager.AddClaimAsync(userAdmin, new Claim("Role", "Administrator"));
//            }

            var fakeSystemAccountUserId = Guid.NewGuid();
            if (!context.Accounts.Any(x => x.Name == "System Account"))
            {
                await context.Accounts.AddAsync(new Account
                {
                    Id = AccountConst.SystemAccountGuid,
                    Name = "System Account",
                    UserId = fakeSystemAccountUserId
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
