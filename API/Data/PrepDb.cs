using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class PrepDb
    {
        public static async Task PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                await SeedData(serviceScope, isProd);
            }
        }

        private static async Task SeedData(IServiceScope serviceScope, bool isProd)
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
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
                try
                {
                    Console.WriteLine("--> Seeding Users Data");
                    var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
                    var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
                    // var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    // var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppUser>>();
                    foreach (var user in users)
                    {
                        using var hmac = new HMACSHA512();

                        user.UserName = user.UserName.ToLower();
                        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                        user.PasswordSalt = hmac.Key;
                        user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
                        user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);
                        // await userManager.CreateAsync(user, "Pa$$w0rd");
                        // await userManager.AddToRoleAsync(user, "Member");
                    }
                    context.Users.AddRange(users);
                    context.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}