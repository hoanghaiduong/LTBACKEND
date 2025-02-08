using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
namespace LTBACKEND.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigDbContext(this IServiceCollection services, IConfigurationManager configuration)
        {
         
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LTBACKEND API", Version = "v1" });
            });
        }

        public static IServiceCollection AddAuth(this IServiceCollection services) {

            services.AddAuthentication();
            services.AddAuthorization();
            return services;
        }
    }
}
