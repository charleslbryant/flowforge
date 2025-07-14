using System.Text.Json;
using FlowForge.Console.Services.WorkflowManagement;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class ListWorkflowsCommand : AsyncCommand<ListWorkflowsSettings>
{
    private readonly ILogger<ListWorkflowsCommand> _logger;
    private readonly IWorkflowService _workflowService;

    public ListWorkflowsCommand(ILogger<ListWorkflowsCommand> logger, IWorkflowService workflowService)
    {
        _logger = logger;
        _workflowService = workflowService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ListWorkflowsSettings settings)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var result = await _workflowService.GetWorkflowsAsync(cancellationTokenSource.Token);
        
        if (!result.Success)
        {
            var errorMessage = result.ErrorMessage?.StartsWith("Failed to retrieve workflows:") == true
                ? result.ErrorMessage
                : $"Failed to retrieve workflows: {result.ErrorMessage}";
            
            if (settings.Json)
            {
                var errorJson = JsonSerializer.Serialize(new { error = errorMessage, success = false });
                System.Console.WriteLine(errorJson);
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]❌ {errorMessage}[/]");
            }
            return 1;
        }

        if (settings.Json)
        {
            var jsonOutput = JsonSerializer.Serialize(new
            {
                success = true,
                totalCount = result.TotalCount,
                workflows = result.Workflows.Select(w => new
                {
                    id = w.Id,
                    name = w.Name,
                    active = w.Active,
                    nodeCount = w.NodeCount,
                    updatedAt = w.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss")
                })
            }, new JsonSerializerOptions { WriteIndented = true });
            
            System.Console.WriteLine(jsonOutput);
            return 0;
        }

        // Regular table output
        AnsiConsole.Write(
            new FigletText("Workflows")
                .LeftJustified()
                .Color(Color.Blue));

        AnsiConsole.WriteLine();

        if (!result.Workflows.Any())
        {
            AnsiConsole.MarkupLine("[yellow]📂 No workflows found[/]");
            return 0;
        }

        var table = new Table();
        table.AddColumn("[bold]ID[/]");
        table.AddColumn("[bold]Name[/]");
        table.AddColumn("[bold]Status[/]");
        table.AddColumn("[bold]Nodes[/]");
        table.AddColumn("[bold]Last Updated[/]");

        foreach (var workflow in result.Workflows)
        {
            var status = workflow.Active ? "[green]Active[/]" : "[dim]Inactive[/]";
            var lastUpdated = workflow.UpdatedAt.ToString("yyyy-MM-dd HH:mm");
            
            table.AddRow(
                workflow.Id,
                workflow.Name,
                status,
                workflow.NodeCount.ToString(),
                lastUpdated);
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[dim]Total: {result.TotalCount} workflows[/]");

        return 0;
    }
}