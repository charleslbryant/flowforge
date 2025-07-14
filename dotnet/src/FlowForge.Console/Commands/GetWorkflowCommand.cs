using System.Text.Json;
using FlowForge.Console.Services.WorkflowManagement;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class GetWorkflowCommand : AsyncCommand<GetWorkflowSettings>
{
    private readonly ILogger<GetWorkflowCommand> _logger;
    private readonly IWorkflowService _workflowService;

    public GetWorkflowCommand(ILogger<GetWorkflowCommand> logger, IWorkflowService workflowService)
    {
        _logger = logger;
        _workflowService = workflowService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, GetWorkflowSettings settings)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var result = await _workflowService.GetWorkflowAsync(settings.Id, cancellationTokenSource.Token);

        if (!result.Success)
        {
            var errorMessage = result.ErrorMessage?.StartsWith("Failed to retrieve workflow:") == true
                ? result.ErrorMessage
                : $"Failed to retrieve workflow: {result.ErrorMessage}";

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

        var workflow = result.WorkflowDetails!;

        if (settings.Json)
        {
            var jsonOutput = JsonSerializer.Serialize(new
            {
                success = true,
                workflow = new
                {
                    id = workflow.Id,
                    name = workflow.Name,
                    active = workflow.Active,
                    nodeCount = workflow.NodeCount,
                    description = workflow.Description,
                    tags = workflow.Tags,
                    createdAt = workflow.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss"),
                    updatedAt = workflow.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss"),
                    nodes = workflow.Nodes
                }
            }, new JsonSerializerOptions { WriteIndented = true });

            System.Console.WriteLine(jsonOutput);
            return 0;
        }

        // Regular formatted output
        AnsiConsole.Write(
            new FigletText($"Workflow: {workflow.Name}")
                .LeftJustified()
                .Color(Color.Blue));

        AnsiConsole.WriteLine();

        var table = new Table();
        table.AddColumn("[bold]Property[/]");
        table.AddColumn("[bold]Value[/]");

        table.AddRow("ID", workflow.Id);
        table.AddRow("Name", workflow.Name);
        table.AddRow("Status", workflow.Active ? "[green]Active[/]" : "[dim]Inactive[/]");
        table.AddRow("Node Count", workflow.NodeCount.ToString());
        table.AddRow("Description", workflow.Description ?? "[dim]No description[/]");
        table.AddRow("Tags", string.Join(", ", workflow.Tags) ?? "[dim]No tags[/]");
        table.AddRow("Created", workflow.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        table.AddRow("Updated", workflow.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        if (workflow.Nodes != null)
        {
            AnsiConsole.MarkupLine("[bold]Workflow Definition:[/]");
            var jsonNodes = JsonSerializer.Serialize(workflow.Nodes, new JsonSerializerOptions { WriteIndented = true });
            AnsiConsole.WriteLine(jsonNodes);
        }

        return 0;
    }
}