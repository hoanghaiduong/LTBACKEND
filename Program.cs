
using System;
using LTBACKEND.Extensions;
using LTBACKEND.Repositories;
using LTBACKEND.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LTBACKEND;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigDbContext(builder.Configuration);
        builder.Services.ConfigureSwagger();
        builder.Services.ConfigHealthChecks();
        builder.Services.AddSingleton<DatabaseConfig>();
        builder.Services.AddScoped<SQLHelper>();
        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");

        app.Run();
    }
}
