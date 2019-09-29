# Playing with Health Checks

This is a small .NET Core WebAPI application to try out the new health checks.

[Separate branch](https://github.com/19balazs86/PlayingWithHealthChecks/tree/netcoreapp2.2) with the .NET Core 2.2 version.

Resources: 
- [Microsoft official page](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0) has a tons of example and information about this topic.
- [HealthChecks packages](https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks) a list of packages and information about the HealthCheckUI.
- [Telerik blog](https://www.telerik.com/blogs/health-checks-in-aspnet-core): Health Checks in ASP.NET Core.
- [Elmah blog post](https://blog.elmah.io/asp-net-core-2-2-health-checks-explained) to explain the health checks.
- [Microsoft video](https://www.youtube.com/watch?v=_vw3hcnSA1Y&t=516) to introduce this new feature and give some tips to use liveness and readiness probes in Docker.
- Scott Hanselman: [How to set up ASP.NET Core 2.2 Health Checks with BeatPulse.](https://www.hanselman.com/blog/HowToSetUpASPNETCore22HealthChecksWithBeatPulsesAspNetCoreDiagnosticsHealthChecks.aspx)

To use HealthCheckUI is totally optional. Usually, you have a centralised system, where you can monitor all your services. Also do not need this in Docker. 

##### The packages I used:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    // Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
    services.AddDbContext<DataBaseContext>(options
        => options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL")));
    ...
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
    services.AddHealthChecksUI();
}
```

##### Setup some portable servers on Windows (no need installation).
- Redis: Description in my [PlayingWithDistributedLock](https://github.com/19balazs86/PlayingWithDistributedLock#setup-a-redis-server-locally-on-windows) repository.
- MySql: [Wiki page.](http://wiki.uniformserver.com/index.php/Mini_Servers:_MySQL_5.0.67_Portable#Support_files)
- PostgreSQL: [PostgreSQL portable page.](https://gareth.flowers/postgresql-portable)
