using Microsoft.EntityFrameworkCore;

namespace API.Entities.TypeConfig
{
    public static class Extension
    {
        public static void ConfigurationAllTypeConfig(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserTypeConfig());
        }
    }
}