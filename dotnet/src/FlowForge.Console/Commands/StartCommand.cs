using FlowForge.Console.Services.HealthChecking;
using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class StartCommand : AsyncCommand
{
    private readonly ILogger<StartCommand> _logger;
    private readonly IProcessManager _processManager;
    private readonly IHealthChecker _healthChecker;

    public StartCommand(ILogger<StartCommand> logger, IProcessManager processManager, IHealthChecker healthChecker)
    {
        _logger = logger;
        _processManager = processManager;
        _healthChecker = healthChecker;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.Write(
            new FigletText("Start n8n")
                .LeftJustified()
                .Color(Color.Green));

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green]🚀 Starting n8n...[/]");
        AnsiConsole.WriteLine();

        // Check if n8n is already running
        var isRunning = await _processManager.IsN8nRunningAsync(CancellationToken.None);
        
        if (isRunning)
        {
            AnsiConsole.MarkupLine("[yellow]⚠️  n8n is already running[/]");
            AnsiConsole.WriteLine();
            
            // Check health of already running n8n
            AnsiConsole.MarkupLine("[blue]Checking n8n health...[/]");
            var healthResult = await _healthChecker.CheckN8nHealthAsync(CancellationToken.None);
            
            if (healthResult.IsHealthy)
            {
                AnsiConsole.MarkupLine("[green]✅ n8n is running and healthy[/]");
                AnsiConsole.MarkupLine("[blue]🌐 Access n8n at: http://localhost:5678[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]❌ n8n is running but not responding properly[/]");
                AnsiConsole.MarkupLine($"[gray]   {healthResult.Message}[/]");
            }
            
            return 0;
        }

        // n8n is not running - start it
        AnsiConsole.MarkupLine("[blue]Starting n8n in background...[/]");
        
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

        // Wait for n8n to start up with a progress bar
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask("[green]Waiting for n8n to start...[/]");
                
                for (int i = 0; i < 50; i++) // 5 seconds total (50 * 100ms)
                {
                    await Task.Delay(100);
                    task.Increment(2); // 100% / 50 = 2% per iteration
                }
            });

        // Check if n8n started successfully
        AnsiConsole.MarkupLine("[blue]Checking n8n health...[/]");
        var healthCheck = await _healthChecker.CheckN8nHealthAsync(CancellationToken.None);
        
        if (healthCheck.IsHealthy)
        {
            AnsiConsole.MarkupLine("[green]✅ n8n started successfully![/]");
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