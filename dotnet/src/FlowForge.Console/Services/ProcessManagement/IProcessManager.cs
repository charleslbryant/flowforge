namespace FlowForge.Console.Services.ProcessManagement;

public interface IProcessManager
{
    Task<bool> IsN8nRunningAsync(CancellationToken cancellationToken);
    Task<ProcessStartResult> StartN8nAsync(CancellationToken cancellationToken);
    Task<bool> StopN8nAsync(CancellationToken cancellationToken);
}

public class ProcessStartResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public int? ProcessId { get; set; }
}