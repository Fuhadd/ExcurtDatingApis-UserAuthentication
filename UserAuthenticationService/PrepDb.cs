using Microsoft.EntityFrameworkCore;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DatabaseContext>(), isProd);
            }

        }

        private static void SeedData(DatabaseContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply MIgrations");
                try
                {

                }catch (Exception ex)
                {
                    Console.WriteLine($"Could not run migrations: {ex.Message}");
                }
                context.Database.Migrate();
            }

        }
    }
}
