using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FlowForge.Console.Models.Config;
using FlowForge.Console.Models.Workflows;

namespace FlowForge.Console.Infrastructure.Http;

/// <summary>
/// HTTP client for communicating with n8n instance
/// </summary>
public class N8nHttpClient : IN8nHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<N8nHttpClient> _logger;
    private readonly N8nConfiguration _config;

    public N8nHttpClient(HttpClient httpClient, ILogger<N8nHttpClient> logger, IOptions<N8nConfiguration> config)
    {
        _httpClient = httpClient;
        _logger = logger;
        _config = config.Value;
        
        // Configure HttpClient headers if API key is provided
        if (!string.IsNullOrEmpty(_config.ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("X-N8N-API-KEY", _config.ApiKey);
        }
    }

    public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(_config.HealthUrl, cancellationToken);
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
            var response = await _httpClient.GetAsync(_config.HealthUrl, cancellationToken);
            
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

    public async Task<IEnumerable<WorkflowSummary>> GetWorkflowsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var workflowsUrl = $"{_config.ApiBaseUrl}/workflows";
            var response = await _httpClient.GetAsync(workflowsUrl, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to retrieve workflows. Status: {StatusCode}", response.StatusCode);
                return Array.Empty<WorkflowSummary>();
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var apiResponse = JsonSerializer.Deserialize<N8nWorkflowListResponse>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return apiResponse?.Data?.Select(MapToWorkflowSummary) ?? Array.Empty<WorkflowSummary>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error retrieving workflows");
            return Array.Empty<WorkflowSummary>();
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Workflow retrieval request timed out");
            return Array.Empty<WorkflowSummary>();
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse workflow response JSON");
            return Array.Empty<WorkflowSummary>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving workflows");
            return Array.Empty<WorkflowSummary>();
        }
    }

    public async Task<WorkflowDetails> GetWorkflowAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var workflowUrl = $"{_config.ApiBaseUrl}/workflows/{id}";
            var response = await _httpClient.GetAsync(workflowUrl, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to retrieve workflow {Id}. Status code: {StatusCode}", id, response.StatusCode);
                throw new InvalidOperationException($"Workflow {id} not found");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var workflow = JsonSerializer.Deserialize<N8nWorkflow>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return MapToWorkflowDetails(workflow ?? new N8nWorkflow());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error retrieving workflow {Id}", id);
            throw;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Workflow {Id} retrieval request timed out", id);
            throw new InvalidOperationException($"Request timed out for workflow {id}");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse workflow {Id} response JSON", id);
            throw new InvalidOperationException($"Invalid response format for workflow {id}");
        }
    }

    private static WorkflowSummary MapToWorkflowSummary(N8nWorkflow workflow)
    {
        return new WorkflowSummary
        {
            Id = workflow.Id ?? string.Empty,
            Name = workflow.Name ?? string.Empty,
            Active = workflow.Active,
            CreatedAt = workflow.CreatedAt,
            UpdatedAt = workflow.UpdatedAt,
            Tags = workflow.Tags ?? Array.Empty<string>(),
            NodeCount = workflow.Nodes?.Count() ?? 0,
            Description = workflow.Description
        };
    }

    private static WorkflowDetails MapToWorkflowDetails(N8nWorkflow workflow)
    {
        return new WorkflowDetails
        {
            Id = workflow.Id ?? string.Empty,
            Name = workflow.Name ?? string.Empty,
            Active = workflow.Active,
            CreatedAt = workflow.CreatedAt,
            UpdatedAt = workflow.UpdatedAt,
            Tags = workflow.Tags ?? Array.Empty<string>(),
            NodeCount = workflow.Nodes?.Count() ?? 0,
            Description = workflow.Description,
            Nodes = workflow.Nodes?.Select(MapToNodeDefinition) ?? Array.Empty<NodeDefinition>()
        };
    }

    private static NodeDefinition MapToNodeDefinition(N8nNode node)
    {
        return new NodeDefinition
        {
            Id = node.Id ?? string.Empty,
            Name = node.Name ?? string.Empty,
            Type = node.Type ?? string.Empty
        };
    }
}