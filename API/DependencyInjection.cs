using API.Data.DataAccess;
using API.Services;

namespace API
{
    public static class DependencyInjection
    {
        public static void AddDependencyServices(this IServiceCollection services)
        {
            services.AddTransient<IUserServices, UserServices>();
            services.AddScoped<IQueries, Queries>();
        }
    }
}