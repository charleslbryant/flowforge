namespace FlowForge.Console.Models.Config;

public class N8nConfiguration
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5678;
    public bool UseHttps { get; set; } = false;
    public string ApiKey { get; set; } = string.Empty;
    public string ProcessPath { get; set; } = "n8n";
    public string LogPath { get; set; } = "~/n8n.log";
    public int StartupTimeoutSeconds { get; set; } = 30;

    public string BaseUrl => $"{(UseHttps ? "https" : "http")}://{Host}:{Port}";
    public string ApiBaseUrl => $"{BaseUrl}/api/v1";
    public string HealthUrl => $"{BaseUrl}/healthz";
}