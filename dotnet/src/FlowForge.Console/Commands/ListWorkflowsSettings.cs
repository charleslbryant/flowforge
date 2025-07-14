using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class ListWorkflowsSettings : CommandSettings
{
    [CommandOption("--json")]
    public bool Json { get; set; }
}