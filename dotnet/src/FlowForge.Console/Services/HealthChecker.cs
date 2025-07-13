using FlowForge.Console.Infrastructure.Http;
using Microsoft.Extensions.Logging;

namespace FlowForge.Console.Services;

/// <summary>
/// Business logic service for health checking
/// </summary>
public class HealthChecker : IHealthChecker
{
    private readonly ILogger<HealthChecker> _logger;
    private readonly IN8nHttpClient _n8nHttpClient;

    public HealthChecker(ILogger<HealthChecker> logger, IN8nHttpClient n8nHttpClient)
    {
        _logger = logger;
        _n8nHttpClient = n8nHttpClient;
    }

    public async Task<HealthResult> CheckN8nHealthAsync(CancellationToken cancellationToken)
    {
        var result = new HealthResult();
        
        try
        {
            var isHealthy = await _n8nHttpClient.CheckHealthAsync(cancellationToken);
            var details = await _n8nHttpClient.GetHealthDetailsAsync(cancellationToken);
            
            result.IsHealthy = isHealthy;
            result.Message = details;
            
            if (!isHealthy)
            {
                result.Issues.Add(details);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during health check");
            result.IsHealthy = false;
            result.Message = "Health check failed due to unexpected error";
            result.Issues.Add($"Unexpected error: {ex.Message}");
        }
        
        return result;
    }
}