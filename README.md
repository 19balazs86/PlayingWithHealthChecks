# Playing with Health Checks

This is a small .NET Core WebAPI application to try out the new health checks.

Resources: 
- [Microsoft official page](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2 "Microsoft official page") has a tuns of example and information about this topic.
- [HealthChecks packages](https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks "HealthChecks packages"), I also used for redis and MySql. Information about the HealthCheckUI.
- [Elmah blog post](https://blog.elmah.io/asp-net-core-2-2-health-checks-explained "Elmah blog post") to explain the health checks.
- Short [Microsoft video](https://www.youtube.com/watch?v=_vw3hcnSA1Y&t=516 "Microsoft video") to introduce this new feature and give some hint to use it in Docker.

To use HealthCheckUI is totally optional. Usually, you have a centralised system, where you can monitor all your services. Also do not need this in Docker. 

##### Setup a portable redis server on Windows (no need installation).
Description is in my [PlayingWithDistributedLock](https://github.com/19balazs86/PlayingWithDistributedLock#setup-a-redis-server-locally-on-windows "PlayingWithDistributedLock") repository.

##### Setup a portable mini MySql server on Windows (no need installation).
Description is on [this wiki page.](http://wiki.uniformserver.com/index.php/Mini_Servers:_MySQL_5.0.67_Portable#Support_files "this wiki page.")