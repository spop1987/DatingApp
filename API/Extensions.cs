using System.Text;
using API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public static class Extensions
    {
        public static ErrorActivity GetActivity(this HttpRequest request)
        {
            var path = request?.Path.ToString().ToUpper();
            var method = request?.Method.ToString().ToUpper();

            if(method == nameof(HttpMethod.Post).ToUpper())
            {
                if(path.ToUpper().Contains("SEARCH"))
                    return ErrorActivity.Search;
                return ErrorActivity.Create;
            }
            if(method == nameof(HttpMethod.Get).ToUpper())
                return ErrorActivity.Get;

            if(method == nameof(HttpMethod.Put).ToUpper())
                return ErrorActivity.Update;
            
            return ErrorActivity.Unknown;
        }

        public static void AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dob.Year;
            if(dob > today.AddYears(-age)) age--;

            return age;
        }
    }
}