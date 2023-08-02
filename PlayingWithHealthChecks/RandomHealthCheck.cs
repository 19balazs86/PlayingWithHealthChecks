using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PlayingWithHealthChecks;

public sealed class RandomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        if (Random.Shared.NextDouble() <= 0.5)
            return Task.FromResult(HealthCheckResult.Healthy("Random check is OK."));

        return Task.FromResult(HealthCheckResult.Unhealthy("Random check is FAILED."));
    }
}
