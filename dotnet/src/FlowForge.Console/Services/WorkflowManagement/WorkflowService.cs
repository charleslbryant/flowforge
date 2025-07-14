using Microsoft.Extensions.Logging;
using FlowForge.Console.Infrastructure.Http;
using FlowForge.Console.Models.Workflows;

namespace FlowForge.Console.Services.WorkflowManagement;

public class WorkflowService : IWorkflowService
{
    private readonly ILogger<WorkflowService> _logger;
    private readonly IN8nHttpClient _n8nHttpClient;

    public WorkflowService(ILogger<WorkflowService> logger, IN8nHttpClient n8nHttpClient)
    {
        _logger = logger;
        _n8nHttpClient = n8nHttpClient;
    }

    public async Task<WorkflowListResult> GetWorkflowsAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Retrieving workflows from n8n");
            
            var workflows = await _n8nHttpClient.GetWorkflowsAsync(cancellationToken);
            var workflowList = workflows.ToList();
            
            _logger.LogInformation("Retrieved {Count} workflows", workflowList.Count);
            
            return new WorkflowListResult
            {
                Success = true,
                Workflows = workflowList,
                TotalCount = workflowList.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve workflows");
            
            return new WorkflowListResult
            {
                Success = false,
                ErrorMessage = $"Failed to retrieve workflows: {ex.Message}",
                Workflows = Array.Empty<WorkflowSummary>(),
                TotalCount = 0
            };
        }
    }

    public async Task<WorkflowDetailsResult> GetWorkflowAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Retrieving workflow {Id} from n8n", id);
            
            var workflowDetails = await _n8nHttpClient.GetWorkflowAsync(id, cancellationToken);
            
            _logger.LogInformation("Retrieved workflow {Id}", id);
            
            return new WorkflowDetailsResult
            {
                Success = true,
                WorkflowDetails = workflowDetails
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve workflow {Id}", id);
            
            return new WorkflowDetailsResult
            {
                Success = false,
                ErrorMessage = $"Failed to retrieve workflow: {ex.Message}"
            };
        }
    }
}