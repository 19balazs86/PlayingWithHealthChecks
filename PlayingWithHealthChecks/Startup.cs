using System.Linq;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace PlayingWithHealthChecks
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    private const bool _isEnableHealthChecksUI = false;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      // Health checks are created as transient objects by default.
      // If you want another lifecycle, health checks can be added manually like this:
      services.AddSingleton<RandomHealthCheck>();

      // Health checks set-up.
      services
        .AddHealthChecks()
        .AddRedis(Configuration.GetConnectionString("Redis"))
        .AddMySql(Configuration.GetConnectionString("MySQL"))
        .AddCheck<RandomHealthCheck>("random");

      if (_isEnableHealthChecksUI)
        services.AddHealthChecksUI();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      // Use health checks
      app.UseHealthChecks("/health", createHealthCheckOptions(_isEnableHealthChecksUI));

      if (_isEnableHealthChecksUI)
        app.UseHealthChecksUI(); // http://localhost:5000/healthchecks-ui

      app.UseMvc();
    }

    // Create 2 types of HealthCheckOptions depending on the isEnableHealthChecksUI.
    private static HealthCheckOptions createHealthCheckOptions(bool isEnableHealthChecksUI)
    {
      if (isEnableHealthChecksUI)
        return new HealthCheckOptions { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse };

      return new HealthCheckOptions
      {
        ResponseWriter = async (httpContext, healthReport) =>
        {
          httpContext.Response.ContentType = "application/json";

          string result = JsonConvert.SerializeObject(new
          {
            status = healthReport.Status.ToString(),
            errors = healthReport.Entries.Select(e =>
                new { e.Key, Value = e.Value.Status.ToString(), e.Value.Description })
          });

          await httpContext.Response.WriteAsync(result);
        }
      };
    }
  }
}
