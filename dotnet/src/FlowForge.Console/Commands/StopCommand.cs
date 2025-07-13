using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Linq;

namespace FlowForge.Console.Commands;

public class StopCommand : AsyncCommand
{
    private readonly ILogger<StopCommand> _logger;
    private readonly IProcessManager _processManager;

    public StopCommand(ILogger<StopCommand> logger, IProcessManager processManager)
    {
        _logger = logger;
        _processManager = processManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.Write(
            new FigletText("Stop n8n")
                .LeftJustified()
                .Color(Color.Red));

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[red]🛑 Stopping n8n...[/]");
        AnsiConsole.WriteLine();

        var stopResult = await _processManager.StopN8nAsyncEnhanced(CancellationToken.None);
        
        if (stopResult.Success)
        {
            AnsiConsole.MarkupLine($"[green]✅ {stopResult.Message}[/]");
            return 0;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]❌ {stopResult.Message}[/]");
            
            if (!string.IsNullOrWhiteSpace(stopResult.ErrorDetails))
            {
                AnsiConsole.MarkupLine($"[gray]Details: {stopResult.ErrorDetails}[/]");
            }
            
            if (stopResult.SuggestedActions.Any())
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]💡 Suggested actions:[/]");
                foreach (var action in stopResult.SuggestedActions)
                {
                    AnsiConsole.MarkupLine($"[gray]• {action}[/]");
                }
            }
            
            return 1;
        }
    }
}