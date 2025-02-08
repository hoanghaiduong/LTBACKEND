using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LTBACKEND.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void ConfigHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "sql",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "database" });
        }
    }
}
