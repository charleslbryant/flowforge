namespace FlowForge.Console.Models.Workflows;

public class WorkflowDetails
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
    public int NodeCount { get; set; }
    public string? Description { get; set; }
    public IEnumerable<NodeDefinition> Nodes { get; set; } = Array.Empty<NodeDefinition>();
}