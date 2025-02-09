
using System;
using System.Data.Common;
using System.Threading.Tasks;
using LTBACKEND.Data;
using LTBACKEND.Extensions;
using LTBACKEND.Repositories;
using LTBACKEND.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LTBACKEND;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigDbContext(builder.Configuration);
        builder.Services.ConfigureSwagger();
        builder.Services.ConfigHealthChecks();


        var app = builder.Build();
        await app.AddInitialDatabaseAsync();
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
