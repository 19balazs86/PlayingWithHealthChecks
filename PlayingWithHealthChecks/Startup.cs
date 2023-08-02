using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

namespace PlayingWithHealthChecks;

public class Startup
{
    public IConfiguration Configuration { get; }

    private static readonly bool _isEnableHealthChecksUI = true;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //services.AddControllers();

        // Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
        services.AddDbContext<DataBaseContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL")));

        // Health checks are created as transient objects by default.
        // If you want another lifecycle, health checks can be added manually like this:
        services.AddSingleton<RandomHealthCheck>();

        // Add: Health checks.
        services
            .AddHealthChecks()
            // Install-Package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
            .AddDbContextCheck<DataBaseContext>()
            // Install-Package AspNetCore.HealthChecks.Redis
            .AddRedis(Configuration.GetConnectionString("Redis"))
            // Install-Package AspNetCore.HealthChecks.MySql
            .AddMySql(Configuration.GetConnectionString("MySQL"))
            .AddCheck<RandomHealthCheck>("random");

        // Install-Package AspNetCore.HealthChecks.UI
        if (_isEnableHealthChecksUI)
            services.AddHealthChecksUI();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        if (_isEnableHealthChecksUI) // http://localhost:5000/healthchecks-ui
            app.UseHealthChecksUI();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", createHealthCheckOptions());
            //.RequireAuthorization(new AuthorizeAttribute() { Roles = "admin", });
        });
    }

    // Create 2 types of HealthCheckOptions depending on the isEnableHealthChecksUI.
    private static HealthCheckOptions createHealthCheckOptions()
    {
        if (_isEnableHealthChecksUI)
            return new HealthCheckOptions { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse };

        return new HealthCheckOptions { ResponseWriter = responseWriter };
    }

    private static async Task responseWriter(HttpContext httpContext, HealthReport healthReport)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var reportResponse = new
        {
            status = healthReport.Status.ToString(),
            errors = healthReport.Entries.Select(e => new { e.Key, Value = e.Value.Status.ToString(), e.Value.Description })
        };

        string result = JsonSerializer.Serialize(reportResponse);

        await httpContext.Response.WriteAsync(result);
    }
}
