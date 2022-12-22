
namespace API.Helpers
{
    public enum ErrorActivity
    {
        Get = 1,
        Search = 2,
        Create = 3,
        Update = 4,
        Delete = 5,
        Authenticate = 6,
        Unknown = 7
    }
    public static class ExceptionFilterExtension
    {
        public static void ConfigureExceptionFilter(this IServiceCollection services, Func<HttpRequest, ErrorActivity> getActivityFunc = null)
        {
            services.AddScoped(s => new ExceptionFilterConfig(getActivityFunc));
            services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(config => config.Filters.Add(typeof(ExceptionFilter)));
        }
        public static ErrorActivity GetActivityDefault(this HttpRequest request)
        {
            var method = request?.Method.ToString().ToUpper();

            return method switch
            {
                "GET" => ErrorActivity.Get,
                "POST" => ErrorActivity.Create,
                "PUT" => ErrorActivity.Update,
                "PATCH" => ErrorActivity.Update,
                "DELETE" => ErrorActivity.Delete,
                _ => ErrorActivity.Unknown
            };
        }
    }
}