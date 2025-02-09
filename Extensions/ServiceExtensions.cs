using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using LTBACKEND.Utils;
using LTBACKEND.Data;
namespace LTBACKEND.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigDbContext(this IServiceCollection services, IConfigurationManager configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                    services.AddScoped<ApplicationDbContextInitialize>();
            services.AddScoped<DbConnection>(sp =>
            {
                var context = sp.GetRequiredService<ApplicationDbContext>();
                var connection = context.Database.GetDbConnection();
                return connection; // Dapper sẽ dùng connection từ EF Core
            });

            services.AddScoped<SQLHelper>();
            services.AddControllers();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LTBACKEND API", Version = "v1" });
            });
        }

        public static IServiceCollection AddAuth(this IServiceCollection services)
        {

            services.AddAuthentication();
            services.AddAuthorization();
            return services;
        }
    }
}
