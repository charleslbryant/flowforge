using FlowForge.Console.Services.HealthChecking;
using FlowForge.Console.Services.SystemChecking;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class DoctorCommand : AsyncCommand
{
    private readonly ILogger<DoctorCommand> _logger;
    private readonly ISystemChecker _systemChecker;
    private readonly IHealthChecker _healthChecker;

    public DoctorCommand(ILogger<DoctorCommand> logger, ISystemChecker systemChecker, IHealthChecker healthChecker)
    {
        _logger = logger;
        _systemChecker = systemChecker;
        _healthChecker = healthChecker;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.Write(
            new FigletText("System Doctor")
                .LeftJustified()
                .Color(Color.Blue));

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[blue]🩺 FlowForge System Check[/]");
        AnsiConsole.MarkupLine("[gray]========================[/]");
        AnsiConsole.WriteLine();

        var overallHealthy = true;

        // Check system requirements
        var systemResult = await _systemChecker.CheckSystemRequirementsAsync(CancellationToken.None);
        
        AnsiConsole.MarkupLine("[yellow]📋 Dependencies Check[/]");
        foreach (var check in systemResult.Checks)
        {
            if (check.IsInstalled)
            {
                var version = !string.IsNullOrEmpty(check.Version) ? $" ({check.Version})" : "";
                AnsiConsole.MarkupLine($"[green]✅ {check.Name}{version}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]❌ {check.Name} - {check.Issue}[/]");
                if (!string.IsNullOrEmpty(check.InstallCommand))
                {
                    AnsiConsole.MarkupLine($"[gray]   Install with: {check.InstallCommand}[/]");
                }
                overallHealthy = false;
            }
        }

        AnsiConsole.WriteLine();

        // Check n8n health
        AnsiConsole.MarkupLine("[yellow]🌐 n8n Health Check[/]");
        var healthResult = await _healthChecker.CheckN8nHealthAsync(CancellationToken.None);
        
        if (healthResult.IsHealthy)
        {
            AnsiConsole.MarkupLine("[green]✅ n8n is running and responding[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]❌ n8n health check failed: {healthResult.Message}[/]");
            foreach (var issue in healthResult.Issues)
            {
                AnsiConsole.MarkupLine($"[gray]   • {issue}[/]");
            }
            overallHealthy = false;
        }

        AnsiConsole.WriteLine();

        // Final verdict
        if (overallHealthy)
        {
            AnsiConsole.MarkupLine("[green]🎯 System is ready! You can now create workflows.[/]");
            return 0;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]❌ System not ready. Please fix the issues above.[/]");
            AnsiConsole.MarkupLine("[gray]💡 Try running 'forge-dotnet health' after fixing dependencies[/]");
            return 1;
        }
    }
}