namespace FlowForge.Console.Services.ProcessManagement;

/// <summary>
/// Detailed result of a process operation with user-friendly feedback
/// </summary>
public class ProcessOperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorDetails { get; set; }
    public List<string> SuggestedActions { get; set; } = new();
    public ProcessOperationType OperationType { get; set; }
    public int? ProcessId { get; set; }
    
    public static ProcessOperationResult CreateSuccess(ProcessOperationType operationType, string message, int? processId = null)
    {
        return new ProcessOperationResult
        {
            Success = true,
            Message = message,
            OperationType = operationType,
            ProcessId = processId
        };
    }
    
    public static ProcessOperationResult CreateFailure(ProcessOperationType operationType, string message, string? errorDetails = null, params string[] suggestedActions)
    {
        return new ProcessOperationResult
        {
            Success = false,
            Message = message,
            ErrorDetails = errorDetails,
            OperationType = operationType,
            SuggestedActions = suggestedActions.ToList()
        };
    }
}

public enum ProcessOperationType
{
    Start,
    Stop,
    Restart,
    StatusCheck
}