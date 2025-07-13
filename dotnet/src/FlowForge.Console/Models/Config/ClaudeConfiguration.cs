namespace FlowForge.Console.Models.Config;

public class ClaudeConfiguration
{
    public string Command { get; set; } = "claude";
    public string[] Arguments { get; set; } = ["--print"];
    public int TimeoutSeconds { get; set; } = 60;
}