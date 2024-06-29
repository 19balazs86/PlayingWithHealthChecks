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
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        var services      = builder.Services;
        var configuration = builder.Configuration;

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
                //// Install-Package AspNetCore.HealthChecks.Redis
                //.AddRedis(configuration.GetConnectionString("Redis")!)
                //// Install-Package AspNetCore.HealthChecks.MySql
                //.AddMySql(configuration.GetConnectionString("MySQL")!)
                .AddCheck<RandomHealthCheck>("RandomHealthCheck");

            // Install-Package AspNetCore.HealthChecks.UI
            if (_isEnableHealthChecksUI)
                services.AddHealthChecksUI();
        }

        WebApplication app = builder.Build();

        // Configure the request pipeline
        {
            app.UseDeveloperExceptionPage();

            if (_isEnableHealthChecksUI) // http://localhost:5000/healthchecks-ui
                app.UseHealthChecksUI();

            app.MapHealthChecks("/health", createHealthCheckOptions());
        }

        app.Run();
    }

    // Create 2 types of HealthCheckOptions depending on the isEnableHealthChecksUI.
    private static HealthCheckOptions createHealthCheckOptions()
    {
        if (_isEnableHealthChecksUI)
            return new HealthCheckOptions { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse };

        return new HealthCheckOptions { ResponseWriter = responseWriter };
    }

    // Without this custom writer, the default response is just a text of Healthy/Unhealthy
    private static async Task responseWriter(HttpContext httpContext, HealthReport healthReport)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var reportResponse = new
        {
            Status = healthReport.Status.ToString(),
            Errors = healthReport.Entries.Select(e => new { Name = e.Key, Value = e.Value.Status.ToString(), e.Value.Description })
        };

        await JsonSerializer.SerializeAsync(httpContext.Response.Body, reportResponse);
    }
}
