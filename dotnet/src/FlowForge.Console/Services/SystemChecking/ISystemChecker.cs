namespace FlowForge.Console.Services.SystemChecking;

public interface ISystemChecker
{
    Task<SystemCheckResult> CheckSystemRequirementsAsync(CancellationToken cancellationToken);
}

public class SystemCheckResult
{
    public bool IsHealthy { get; set; }
    public List<SystemCheck> Checks { get; set; } = new();
    public List<string> MissingDependencies { get; set; } = new();
}

public class SystemCheck
{
    public string Name { get; set; } = string.Empty;
    public bool IsInstalled { get; set; }
    public string? Version { get; set; }
    public string? Issue { get; set; }
    public string? InstallCommand { get; set; }
}