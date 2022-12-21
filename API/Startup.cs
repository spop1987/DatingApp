using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDependencyServices();
            if(_env.IsProduction()){
                Console.WriteLine("--> Using SqlServer DB");
                ConfigureDb(services);
            }else{
                ConfigureDb(services);
                Console.WriteLine("--> Using InMem Db");
                services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("InMen"));
            }
            services.AddCors();
        }

        protected virtual void ConfigureDb(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DatingAppDb"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "BooksApi v1"));
            }
            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}