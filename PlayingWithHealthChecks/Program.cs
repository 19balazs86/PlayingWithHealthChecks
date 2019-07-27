using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PlayingWithHealthChecks
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webHostBuilder =>
          webHostBuilder
            .UseStartup<Startup>()
            .ConfigureLogging((hostingContext, logging) =>
            {
              logging.ClearProviders();
              logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
              logging.AddConsole();
            }));
    }
  }
}
