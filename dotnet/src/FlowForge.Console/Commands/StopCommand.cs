using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

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

        // Check if n8n is running
        var isRunning = await _processManager.IsN8nRunningAsync(CancellationToken.None);
        
        if (!isRunning)
        {
            AnsiConsole.MarkupLine("[yellow]⚠️  n8n is not currently running[/]");
            return 0;
        }

        // n8n is running - stop it
        AnsiConsole.MarkupLine("[blue]Stopping n8n process...[/]");
        
        var stopResult = await _processManager.StopN8nAsync(CancellationToken.None);
        
        if (stopResult)
        {
            AnsiConsole.MarkupLine("[green]✅ n8n stopped successfully[/]");
            return 0;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]❌ Failed to stop n8n process[/]");
            AnsiConsole.MarkupLine("[gray]💡 Try manually stopping with: pkill -f n8n[/]");
            return 1;
        }
    }
}