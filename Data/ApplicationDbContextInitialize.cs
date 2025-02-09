using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LTBACKEND.Entities;
using LTBACKEND.Utils.Constants;
using Microsoft.EntityFrameworkCore;

namespace LTBACKEND.Data
{
    public static class InitializeDataContext
    {
        public static async Task AddInitialDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialize>();
            await initialiser.InitializeAsync();
            await initialiser.SeedAsync();

        }
    }
    public class ApplicationDbContextInitialize
    {
        private readonly ILogger<ApplicationDbContextInitialize> _logger;
        private readonly ApplicationDbContext _context;

        public ApplicationDbContextInitialize(ILogger<ApplicationDbContextInitialize> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Initializing database failed");
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Initializing database failed");
            }
        }

        private async Task TrySeedAsync()
        {
            // Default roles
            var administratorRole = new Role { Name = Roles.Administrator };
            var employeeRole = new Role { Name = Roles.Employee };

            if (!_context.Roles.Any())
            {
                _context.Roles.Add(administratorRole);
                _context.Roles.Add(employeeRole);
                await _context.SaveChangesAsync();
            }

        }
    }
}