using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace FlowForge.Console.Infrastructure.Process;

/// <summary>
/// Cross-platform process execution infrastructure
/// </summary>
public class ProcessExecutor : IProcessExecutor
{
    private readonly ILogger<ProcessExecutor> _logger;

    public ProcessExecutor(ILogger<ProcessExecutor> logger)
    {
        _logger = logger;
    }

    public async Task<ProcessResult> ExecuteAsync(string fileName, string arguments, CancellationToken cancellationToken)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new System.Diagnostics.Process { StartInfo = processInfo };
        
        try
        {
            process.Start();
            await process.WaitForExitAsync(cancellationToken);

            var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
            var error = await process.StandardError.ReadToEndAsync(cancellationToken);

            return new ProcessResult
            {
                ExitCode = process.ExitCode,
                Output = output,
                Error = error
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute process {FileName} {Arguments}", fileName, arguments);
            return new ProcessResult
            {
                ExitCode = -1,
                Error = ex.Message
            };
        }
    }

    public async Task<ProcessResult> ExecuteWithOutputAsync(string fileName, string arguments, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(fileName, arguments, cancellationToken);
    }

    public async Task<System.Diagnostics.Process> StartBackgroundProcessAsync(string fileName, string arguments, string? logFilePath = null)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = !string.IsNullOrEmpty(logFilePath),
            RedirectStandardError = !string.IsNullOrEmpty(logFilePath)
        };

        var process = new System.Diagnostics.Process { StartInfo = processInfo };

        if (!string.IsNullOrEmpty(logFilePath))
        {
            // Set up logging to file
            process.OutputDataReceived += (sender, e) => LogToFile(logFilePath, e.Data);
            process.ErrorDataReceived += (sender, e) => LogToFile(logFilePath, e.Data);
        }

        try
        {
            var started = process.Start();
            
            if (!started)
            {
                throw new InvalidOperationException($"Failed to start process {fileName}");
            }

            if (!string.IsNullOrEmpty(logFilePath))
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            _logger.LogInformation("Started background process {FileName} with PID {ProcessId}", fileName, process.Id);
            return process;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start background process {FileName}", fileName);
            process.Dispose();
            throw;
        }
    }

    public async Task<bool> IsProcessRunningAsync(string processName, CancellationToken cancellationToken)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var result = await ExecuteAsync("tasklist", $"/FI \"IMAGENAME eq {processName}.exe\"", cancellationToken);
                return result.Success && result.Output.Contains($"{processName}.exe");
            }
            else
            {
                var result = await ExecuteAsync("pgrep", $"-f {processName}", cancellationToken);
                return result.Success;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check if process {ProcessName} is running", processName);
            return false;
        }
    }

    public async Task<bool> KillProcessAsync(string processName, CancellationToken cancellationToken)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var result = await ExecuteAsync("taskkill", $"/F /IM {processName}.exe", cancellationToken);
                return result.Success;
            }
            else
            {
                var result = await ExecuteAsync("pkill", $"-f {processName}", cancellationToken);
                return result.Success;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to kill process {ProcessName}", processName);
            return false;
        }
    }

    private void LogToFile(string logFilePath, string? data)
    {
        if (string.IsNullOrEmpty(data)) return;

        try
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {data}{Environment.NewLine}";
            File.AppendAllText(logFilePath, logEntry);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to write to log file {LogFilePath}", logFilePath);
        }
    }
}