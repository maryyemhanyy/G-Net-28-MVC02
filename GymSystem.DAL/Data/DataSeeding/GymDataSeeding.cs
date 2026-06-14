using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext gymDbContext , string seedFilePath , ILogger logger , CancellationToken ct = default)
        {
            try
            {
                if(!await gymDbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("Plans.json", seedFilePath);

                    if(plans.Count > 0)
                    {
                        await gymDbContext.Plans.AddRangeAsync(plans, ct);
                        logger.LogInformation("{Count} Plans seeded successfully." , plans.Count);
                    }
                }

                if(gymDbContext.ChangeTracker.HasChanges()) await gymDbContext.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        private static List<T>LoadDataFromJsonFile<T>(string fileName , string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);

            if(!File.Exists(filePath)) throw new FileNotFoundException($"File Not Found: {filePath}");

            var data = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? new List<T>();


        }
    }
}
