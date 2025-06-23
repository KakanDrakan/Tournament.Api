using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var db = serviceProvider.GetRequiredService<TournamentApiContext>();

            await db.Database.MigrateAsync();
            if (await db.Tournament.AnyAsync())
            {
                return; // Database already seeded
            }

            try
            {
                SeedData.SeedTournamentData(db);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while seeding the database.", ex);
            }
        }
    }
}
