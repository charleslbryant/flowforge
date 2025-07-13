using FlowForge.Console.Models.Workflows;

namespace FlowForge.Console.Services.WorkflowManagement;

public interface IWorkflowService
{
    Task<WorkflowListResult> GetWorkflowsAsync(CancellationToken cancellationToken);
}

public class WorkflowListResult
{
    public bool Success { get; set; }
    public IEnumerable<WorkflowSummary> Workflows { get; set; } = Array.Empty<WorkflowSummary>();
    public string? ErrorMessage { get; set; }
    public int TotalCount { get; set; }
}