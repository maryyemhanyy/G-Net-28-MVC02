using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
        {

            try
            {
                bool HasUsers = userManager.Users.Any();

                bool hasRoles = roleManager.Roles.Any();

                if (hasRoles && HasUsers) return;

                if (!hasRoles)
                {
                    var Roles = new List<IdentityRole>
                    {
                        new IdentityRole { Name = "SuperAdmin" },
                        new IdentityRole { Name = "Admin" }
                    };

                    foreach (var role in Roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            var result = await roleManager.CreateAsync(new IdentityRole { Name = role });
                            if (!result.Succeeded)
                            {
                                logger.LogError("Failed to create role: {Role}. Errors: {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
                            }
                        }
                    }
                }

                if (!HasUsers)
                {
                    var superAdminUser = new ApplicationUser
                    {
                        FirstName = "Hossam",
                        LastName = "Ahmed",
                        UserName = "hossamahmed",
                        Email = "hossamahmed@gmail.com",
                        PhoneNumber = "01000000000"
                    };
                    await userManager.CreateAsync(superAdminUser, "P@ssw0rd");
                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");

                    var adminUser = new ApplicationUser
                    {
                        FirstName = "Hala",
                        LastName = "Ahmed",
                        UserName = "halaahmed",
                        Email = "halaahmed@gmail.com",
                        PhoneNumber = "01000000000"
                    };

                    var result = await userManager.CreateAsync(adminUser, "P@ssw0rd");
                    if (!result.Succeeded)
                    {
                        logger.LogError("Failed to create user: {User}. Errors: {Errors}", adminUser.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
                        return;
                    }
                    logger.LogInformation($"seeded admin {adminUser.Email}");
                }
                return;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during identity data seeding.");
                throw;

            }
        }
    }
}
    

