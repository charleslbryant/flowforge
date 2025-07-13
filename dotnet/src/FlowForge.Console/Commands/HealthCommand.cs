using FlowForge.Console.Services.HealthChecking;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class HealthCommand : AsyncCommand
{
    private readonly ILogger<HealthCommand> _logger;
    private readonly IHealthChecker _healthChecker;

    public HealthCommand(ILogger<HealthCommand> logger, IHealthChecker healthChecker)
    {
        _logger = logger;
        _healthChecker = healthChecker;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.Write(
            new FigletText("Health Check")
                .LeftJustified()
                .Color(Color.Green));

        AnsiConsole.WriteLine();

        var result = await _healthChecker.CheckN8nHealthAsync(CancellationToken.None);
        
        if (result.IsHealthy)
        {
            AnsiConsole.MarkupLine("[green]✅ n8n is healthy[/]");
            AnsiConsole.MarkupLine($"[gray]{result.Message}[/]");
            return 0;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]❌ n8n is not healthy[/]");
            AnsiConsole.MarkupLine($"[gray]{result.Message}[/]");
            
            if (result.Issues.Any())
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]Issues found:[/]");
                foreach (var issue in result.Issues)
                {
                    AnsiConsole.MarkupLine($"[red]  • {issue}[/]");
                }
            }
            
            return 1;
        }
    }
}