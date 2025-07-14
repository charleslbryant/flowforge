using FlowForge.Console.Models.Workflows;

namespace FlowForge.Console.Infrastructure.Http;

/// <summary>
/// Infrastructure service for HTTP communication with n8n
/// </summary>
public interface IN8nHttpClient
{
    Task<bool> CheckHealthAsync(CancellationToken cancellationToken);
    Task<string> GetHealthDetailsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<WorkflowSummary>> GetWorkflowsAsync(CancellationToken cancellationToken);
    Task<WorkflowDetails> GetWorkflowAsync(string id, CancellationToken cancellationToken);
}