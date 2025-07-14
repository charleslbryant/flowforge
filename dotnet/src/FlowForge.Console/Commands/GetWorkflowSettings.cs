using Spectre.Console.Cli;

namespace FlowForge.Console.Commands;

public class GetWorkflowSettings : CommandSettings
{
    [CommandArgument(0, "<ID>")]
    public string Id { get; set; } = string.Empty;

    [CommandOption("--json")]
    public bool Json { get; set; }
}