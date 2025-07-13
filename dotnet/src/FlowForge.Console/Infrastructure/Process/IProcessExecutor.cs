namespace FlowForge.Console.Infrastructure.Process;

/// <summary>
/// Infrastructure service for executing system processes
/// </summary>
public interface IProcessExecutor
{
    Task<ProcessResult> ExecuteAsync(string fileName, string arguments, CancellationToken cancellationToken);
    Task<ProcessResult> ExecuteWithOutputAsync(string fileName, string arguments, CancellationToken cancellationToken);
    Task<System.Diagnostics.Process> StartBackgroundProcessAsync(string fileName, string arguments, string? logFilePath = null);
    Task<bool> IsProcessRunningAsync(string processName, CancellationToken cancellationToken);
    Task<bool> KillProcessAsync(string processName, CancellationToken cancellationToken);
}

public class ProcessResult
{
    public int ExitCode { get; set; }
    public string Output { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public bool Success => ExitCode == 0;
}