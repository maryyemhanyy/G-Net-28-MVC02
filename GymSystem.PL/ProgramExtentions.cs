using GymSystem.DAL.Data.DataSeeding;
using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL
{
    public static class ProgramExtentions
    {
        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var pendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();

            if(pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} pending migrations...");
                await dbcontext.Database.MigrateAsync();
            }
            
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot" , "Files");

            await GymDataSeeding.SeedAsync(dbcontext, seedFolderPath, logger);

            await IdentityDataSeeding.SeedAsync(roleManager, userManager, logger);
        }
    }
}
