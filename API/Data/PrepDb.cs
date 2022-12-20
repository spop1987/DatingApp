using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DataContext>(), isProd);
            }
        }

        private static void SeedData(DataContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if(!context.Users.Any()){
                Console.WriteLine("--> Seeding Users Data");
                context.Users.AddRange(
                    new AppUser{
                        UserName = "Sergio Pastor Ontiveros Perez"
                    },
                    new AppUser{
                        UserName = "Diovana Daugs Borges Fortes"
                    }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}