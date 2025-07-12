using Microsoft.Extensions.Logging;

namespace FlowForge.Console.Infrastructure.Http;

/// <summary>
/// HTTP client for communicating with n8n instance
/// </summary>
public class N8nHttpClient : IN8nHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<N8nHttpClient> _logger;
    private readonly string _healthUrl = "http://localhost:5678/healthz";

    public N8nHttpClient(HttpClient httpClient, ILogger<N8nHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(_healthUrl, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Health check failed for n8n");
            return false;
        }
    }

    public async Task<string> GetHealthDetailsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(_healthUrl, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                return "n8n is running and responding";
            }
            
            return $"n8n responded with status: {response.StatusCode}";
        }
        catch (HttpRequestException ex)
        {
            return $"Connection failed: {ex.Message}";
        }
        catch (TaskCanceledException)
        {
            return "Health check request timed out";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during health check");
            return $"Unexpected error: {ex.Message}";
        }
    }
}