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
}