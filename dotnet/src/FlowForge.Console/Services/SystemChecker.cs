using FlowForge.Console.Infrastructure.Process;
using Microsoft.Extensions.Logging;

namespace FlowForge.Console.Services;

/// <summary>
/// Business logic service for system requirements checking
/// </summary>
public class SystemChecker : ISystemChecker
{
    private readonly ILogger<SystemChecker> _logger;
    private readonly IProcessExecutor _processExecutor;
    
    private readonly string[] _requiredCommands = { "node", "npm", "jq", "curl", "nc" };
    private readonly string[] _requiredTools = { "n8n", "claude" };

    public SystemChecker(ILogger<SystemChecker> logger, IProcessExecutor processExecutor)
    {
        _logger = logger;
        _processExecutor = processExecutor;
    }

    public async Task<SystemCheckResult> CheckSystemRequirementsAsync(CancellationToken cancellationToken)
    {
        var result = new SystemCheckResult();
        var allChecks = new List<SystemCheck>();

        // Check required commands
        foreach (var command in _requiredCommands)
        {
            var check = await CheckCommandAsync(command, cancellationToken);
            allChecks.Add(check);
            
            if (!check.IsInstalled)
            {
                result.MissingDependencies.Add(command);
            }
        }

        // Check required tools
        foreach (var tool in _requiredTools)
        {
            var check = await CheckToolAsync(tool, cancellationToken);
            allChecks.Add(check);
            
            if (!check.IsInstalled)
            {
                result.MissingDependencies.Add(tool);
            }
        }

        result.Checks = allChecks;
        result.IsHealthy = result.MissingDependencies.Count == 0;

        return result;
    }

    private async Task<SystemCheck> CheckCommandAsync(string command, CancellationToken cancellationToken)
    {
        try
        {
            var whichResult = await _processExecutor.ExecuteAsync("which", command, cancellationToken);

            if (whichResult.Success)
            {
                var version = await GetVersionAsync(command, cancellationToken);
                return new SystemCheck
                {
                    Name = command,
                    IsInstalled = true,
                    Version = version
                };
            }
            else
            {
                return new SystemCheck
                {
                    Name = command,
                    IsInstalled = false,
                    Issue = "Command not found",
                    InstallCommand = GetInstallCommand(command)
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check command {Command}", command);
            return new SystemCheck
            {
                Name = command,
                IsInstalled = false,
                Issue = $"Check failed: {ex.Message}",
                InstallCommand = GetInstallCommand(command)
            };
        }
    }

    private async Task<SystemCheck> CheckToolAsync(string tool, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _processExecutor.ExecuteAsync(tool, "--version", cancellationToken);

            if (result.Success)
            {
                return new SystemCheck
                {
                    Name = tool,
                    IsInstalled = true,
                    Version = ExtractVersion(result.Output)
                };
            }
            else
            {
                return new SystemCheck
                {
                    Name = tool,
                    IsInstalled = false,
                    Issue = "Command not found",
                    InstallCommand = GetToolInstallCommand(tool)
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check tool {Tool}", tool);
            return new SystemCheck
            {
                Name = tool,
                IsInstalled = false,
                Issue = $"Check failed: {ex.Message}",
                InstallCommand = GetToolInstallCommand(tool)
            };
        }
    }

    private async Task<string?> GetVersionAsync(string command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _processExecutor.ExecuteAsync(command, "--version", cancellationToken);
            
            if (result.Success)
            {
                return ExtractVersion(result.Output);
            }
        }
        catch
        {
            // Ignore version check failures
        }

        return null;
    }

    private static string ExtractVersion(string output)
    {
        // Simple version extraction - gets first line that looks like a version
        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return lines.FirstOrDefault()?.Trim() ?? "unknown";
    }

    private static string GetInstallCommand(string command)
    {
        return command switch
        {
            "node" => "sudo apt install nodejs",
            "npm" => "sudo apt install npm", 
            "jq" => "sudo apt install jq",
            "curl" => "sudo apt install curl",
            "nc" => "sudo apt install netcat-openbsd",
            _ => $"Install {command}"
        };
    }

    private static string GetToolInstallCommand(string tool)
    {
        return tool switch
        {
            "n8n" => "npm install -g n8n",
            "claude" => "npm install -g @anthropic-ai/claude-code",
            _ => $"Install {tool}"
        };
    }
}