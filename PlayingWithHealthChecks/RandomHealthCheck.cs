using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PlayingWithHealthChecks
{
  public class RandomHealthCheck : IHealthCheck
  {
    private readonly Random _random = new Random();

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
    {
      if (_random.NextDouble() <= 0.5)
        return Task.FromResult(HealthCheckResult.Healthy("Random check is OK."));

      return Task.FromResult(HealthCheckResult.Unhealthy("Random check is FAILED."));
    }
  }
}
