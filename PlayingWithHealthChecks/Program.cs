using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

namespace PlayingWithHealthChecks;

public static class Program
{
    private static readonly bool _isEnableHealthChecksUI = false;

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder       = WebApplication.CreateBuilder(args);
        IServiceCollection    services      = builder.Services;
        ConfigurationManager  configuration = builder.Configuration;

        // Add services to the container
        {
            //services.AddDbContext<DataBaseContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

            // Health checks are created as transient objects by default.
            // If you want another lifecycle, health checks can be added manually like this:
            services.AddSingleton<RandomHealthCheck>();

            // Add: Health checks.
            services.AddHealthChecks()
                //// Install-Package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
                //.AddDbContextCheck<DataBaseContext>()
                //.AddDbContextCheck<DataBaseContext>("db-check-users", customTestQuery: (db, ct) => db.Users.AnyAsync(ct))
                //// Install-Package AspNetCore.HealthChecks.Redis
                //.AddRedis(configuration.GetConnectionString("Redis")!)
                //// Install-Package AspNetCore.HealthChecks.MySql
                //.AddMySql(configuration.GetConnectionString("MySQL")!)
                .AddCheck<RandomHealthCheck>("RandomHealthCheck");

            // Install-Package AspNetCore.HealthChecks.UI
            if (_isEnableHealthChecksUI)
            {
                services.AddHealthChecksUI();
            }
        }

        WebApplication app = builder.Build();

        // Configure the request pipeline
        {
            app.UseDeveloperExceptionPage();

            if (_isEnableHealthChecksUI)
            {
                app.UseHealthChecksUI(); // http://localhost:5000/healthchecks-ui
            }

            app.MapHealthChecks("/health", createHealthCheckOptions());
        }

        app.Run();
    }

    // Create 2 types of HealthCheckOptions depending on the isEnableHealthChecksUI.
    private static HealthCheckOptions createHealthCheckOptions()
    {
        return _isEnableHealthChecksUI
            ? new HealthCheckOptions { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }
            : new HealthCheckOptions { ResponseWriter = jsonResponseWriter };
    }

    // Without this custom writer, the default response is just a text of Healthy/Unhealthy
    private static async Task jsonResponseWriter(HttpContext httpContext, HealthReport healthReport)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var reportResponse = new
        {
            Status  = healthReport.Status.ToString(),
            Entries = healthReport.Entries.Select(e => new { Name = e.Key, Status = e.Value.Status.ToString(), e.Value.Description })
        };

        await JsonSerializer.SerializeAsync(httpContext.Response.Body, reportResponse);
    }
}
