using API.Data.DataAccess;
using API.Data.Translators;
using API.Interfaces;
using API.Services;

namespace API
{
    public static class DependencyInjection
    {
        public static void AddDependencyServices(this IServiceCollection services)
        {
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IQueries, Queries>();
            services.AddScoped<ICommands, Commands>();
            services.AddScoped<IToDtoTranslator, ToDtoTranslator>();
        }
    }
}