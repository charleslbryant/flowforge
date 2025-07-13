namespace FlowForge.Console.Models.Workflows;

public class N8nWorkflowListResponse
{
    public IEnumerable<N8nWorkflow>? Data { get; set; }
}

public class N8nWorkflow
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<string>? Tags { get; set; }
    public string? Description { get; set; }
    public IEnumerable<N8nNode>? Nodes { get; set; }
}

public class N8nNode
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
}