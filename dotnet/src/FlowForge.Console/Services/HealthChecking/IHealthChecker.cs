namespace FlowForge.Console.Services.HealthChecking;

public interface IHealthChecker
{
    Task<HealthResult> CheckN8nHealthAsync(CancellationToken cancellationToken);
}

public class HealthResult
{
    public bool IsHealthy { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Issues { get; set; } = new();
}