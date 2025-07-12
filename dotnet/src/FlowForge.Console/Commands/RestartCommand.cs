using FlowForge.Console.Services.HealthChecking;
using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class RestartCommand : AsyncCommand
{
    private readonly ILogger<RestartCommand> _logger;
    private readonly IProcessManager _processManager;
    private readonly IHealthChecker _healthChecker;

    public RestartCommand(ILogger<RestartCommand> logger, IProcessManager processManager, IHealthChecker healthChecker)
    {
        _logger = logger;
        _processManager = processManager;
        _healthChecker = healthChecker;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.Write(
            new FigletText("Restart n8n")
                .LeftJustified()
                .Color(Color.Orange3));

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[orange3]🔄 Restarting n8n...[/]");
        AnsiConsole.WriteLine();

        // Check if n8n is running
        var isRunning = await _processManager.IsN8nRunningAsync(CancellationToken.None);
        
        if (isRunning)
        {
            // n8n is running - stop it first
            AnsiConsole.MarkupLine("[blue]Stopping n8n process...[/]");
            
            var stopResult = await _processManager.StopN8nAsync(CancellationToken.None);
            
            if (!stopResult)
            {
                AnsiConsole.MarkupLine("[red]❌ Failed to stop n8n process[/]");
                AnsiConsole.MarkupLine("[gray]💡 Try manually stopping with: pkill -f n8n[/]");
                return 1;
            }
            
            AnsiConsole.MarkupLine("[green]✅ n8n stopped successfully[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]⚠️  n8n is not currently running[/]");
        }

        // Start n8n
        AnsiConsole.MarkupLine("[blue]Starting n8n process...[/]");
        
        var startResult = await _processManager.StartN8nAsync(CancellationToken.None);
        
        if (!startResult.Success)
        {
            AnsiConsole.MarkupLine("[red]❌ Failed to start n8n process[/]");
            if (!string.IsNullOrEmpty(startResult.ErrorMessage))
            {
                AnsiConsole.MarkupLine($"[gray]   Error: {startResult.ErrorMessage}[/]");
            }
            AnsiConsole.MarkupLine("[gray]💡 Make sure n8n is installed: npm install -g n8n[/]");
            return 1;
        }

        AnsiConsole.MarkupLine($"[green]✅ n8n process started (PID: {startResult.ProcessId})[/]");
        AnsiConsole.WriteLine();

        // Wait for n8n to start up
        AnsiConsole.MarkupLine("[blue]Waiting for n8n to start...[/]");
        await Task.Delay(5000); // Wait 5 seconds for startup

        // Check if n8n started successfully
        AnsiConsole.MarkupLine("[blue]Checking n8n health...[/]");
        var healthCheck = await _healthChecker.CheckN8nHealthAsync(CancellationToken.None);
        
        if (healthCheck.IsHealthy)
        {
            AnsiConsole.MarkupLine("[green]✅ n8n restarted successfully![/]");
            AnsiConsole.MarkupLine("[blue]🌐 Access n8n at: http://localhost:5678[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[gray]💡 Logs are written to: ~/n8n.log[/]");
            return 0;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]❌ n8n process started but is not responding properly[/]");
            AnsiConsole.MarkupLine($"[gray]   {healthCheck.Message}[/]");
            
            if (healthCheck.Issues.Any())
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]Issues found:[/]");
                foreach (var issue in healthCheck.Issues)
                {
                    AnsiConsole.MarkupLine($"[red]  • {issue}[/]");
                }
            }
            
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[gray]💡 Check logs: tail -f ~/n8n.log[/]");
            return 1;
        }
    }
}