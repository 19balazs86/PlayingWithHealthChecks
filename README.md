# Playing with Health Checks

This is a small WebAPI application to try out the health checks feature.

[Separate branch](https://github.com/19balazs86/PlayingWithHealthChecks/tree/netcoreapp2.2) with the .NET Core 2.2 version.

Resources: 
- [Health checks in ASP.NET](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks) 📚 - Lots of examples
- [Health checks for monitoring your applications](https://www.milanjovanovic.tech/blog/health-checks-in-asp-net-core) 📓*Milan*
- [Collection of packages](https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks) 👤 - A list of packages and lots of information, HealthCheckUI
- [Prepare you application to be monitored by Kubernetes](https://dev.to/gkarwchan/prepare-net-core-microservice-to-be-monitored-by-kubernetes-4pgn) 📓*DEV.to / Ghassan Karwchan*

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
    services.AddHealthChecksUI(); // Optional to use
}
```

##### Portable servers on Windows
- Redis: Description in my [PlayingWithDistributedLock](https://github.com/19balazs86/PlayingWithDistributedLock#setup-a-redis-server-locally-on-windows) repository.
- MySql: [Wiki page.](http://wiki.uniformserver.com/index.php/Mini_Servers:_MySQL_5.0.67_Portable#Support_files)
- Github: garethflowers / postgresql-portable / [releases](https://github.com/garethflowers/postgresql-portable/releases) *(database, user, password: postgres)*
