using FlowForge.Console.Infrastructure.Process;
using Microsoft.Extensions.Logging;

namespace FlowForge.Console.Services.ProcessManagement;

/// <summary>
/// Business logic service for n8n process management
/// </summary>
public class ProcessManager : IProcessManager
{
    private readonly ILogger<ProcessManager> _logger;
    private readonly IProcessExecutor _processExecutor;
    private readonly string _n8nLogPath;

    public ProcessManager(ILogger<ProcessManager> logger, IProcessExecutor processExecutor)
    {
        _logger = logger;
        _processExecutor = processExecutor;
        _n8nLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "n8n.log");
    }

    public async Task<bool> IsN8nRunningAsync(CancellationToken cancellationToken)
    {
        return await _processExecutor.IsProcessRunningAsync("n8n", cancellationToken);
    }

    public async Task<ProcessStartResult> StartN8nAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting n8n process...");

            var process = await _processExecutor.StartBackgroundProcessAsync("n8n", "", _n8nLogPath);

            return new ProcessStartResult 
            { 
                Success = true, 
                ProcessId = process.Id 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start n8n process");
            return new ProcessStartResult 
            { 
                Success = false, 
                ErrorMessage = ex.Message 
            };
        }
    }

    public async Task<bool> StopN8nAsync(CancellationToken cancellationToken)
    {
        return await _processExecutor.KillProcessAsync("n8n", cancellationToken);
    }
    
    public async Task<ProcessOperationResult> StopN8nAsyncEnhanced(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Attempting to stop n8n process...");
            
            // First check if process is running
            var isRunning = await IsN8nRunningAsync(cancellationToken);
            if (!isRunning)
            {
                return ProcessOperationResult.CreateSuccess(
                    ProcessOperationType.Stop,
                    "n8n process is not running");
            }
            
            // Attempt to kill the process
            var killed = await _processExecutor.KillProcessAsync("n8n", cancellationToken);
            
            if (killed)
            {
                _logger.LogInformation("n8n process stopped successfully");
                return ProcessOperationResult.CreateSuccess(
                    ProcessOperationType.Stop,
                    "n8n process stopped successfully");
            }
            else
            {
                _logger.LogWarning("Failed to stop n8n process - may require elevated permissions");
                return ProcessOperationResult.CreateFailure(
                    ProcessOperationType.Stop,
                    "Failed to stop n8n process",
                    "The process could not be terminated. This may be due to insufficient permissions.",
                    "Try running with elevated permissions (sudo on Linux/macOS)",
                    "Use 'pkill -f n8n' or 'taskkill /F /IM n8n.exe' manually",
                    "Check if the process is system-protected or locked");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while stopping n8n process");
            return ProcessOperationResult.CreateFailure(
                ProcessOperationType.Stop,
                "Unexpected error while stopping n8n process",
                ex.Message,
                "Check system logs for more details",
                "Ensure the process management tools are available (pkill/taskkill)",
                "Try stopping the process manually");
        }
    }
}