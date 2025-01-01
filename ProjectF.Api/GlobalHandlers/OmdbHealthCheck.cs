using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectF.OmdbClient.Models.Requests;

namespace ProjectF.Api.GlobalHandlers;

public class OmdbHealthCheck(OmdbClient.Services.OmdbClient omdbClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            await omdbClient.SearchAsync(new SearchQueryModel { Term = "test" }, cancellationToken);
            
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(exception: exception);
        }
    }
}