# WorkflowService Architecture Design

## Overview
The WorkflowService will provide comprehensive workflow management operations for the FlowForge .NET implementation, following the established clean architecture pattern with Commands → Services → Infrastructure separation.

## Architecture Components

### 1. Service Interface and Implementation

```csharp
// Services/WorkflowManagement/IWorkflowService.cs
public interface IWorkflowService
{
    Task<IEnumerable<WorkflowSummary>> ListWorkflowsAsync(CancellationToken cancellationToken = default);
    Task<WorkflowDetails?> GetWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task<string> CreateWorkflowAsync(WorkflowDefinition definition, CancellationToken cancellationToken = default);
    Task<bool> UpdateWorkflowAsync(string id, WorkflowDefinition definition, CancellationToken cancellationToken = default);
    Task<bool> DeleteWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ActivateWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> DeactivateWorkflowAsync(string id, CancellationToken cancellationToken = default);
}

// Services/WorkflowManagement/WorkflowService.cs
public class WorkflowService : IWorkflowService
{
    private readonly IN8nWorkflowClient _workflowClient;
    private readonly ILogger<WorkflowService> _logger;
    // Implementation details...
}
```

### 2. Data Models

```csharp
// Services/WorkflowManagement/Models/WorkflowSummary.cs
public record WorkflowSummary(
    string Id,
    string Name,
    bool Active,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

// Services/WorkflowManagement/Models/WorkflowDetails.cs
public record WorkflowDetails(
    string Id,
    string Name,
    bool Active,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string JsonDefinition
);

// Services/WorkflowManagement/Models/WorkflowDefinition.cs
public class WorkflowDefinition
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = false;
    public object Nodes { get; set; } = new object();
    public object Connections { get; set; } = new object();
    // Additional workflow properties as needed
}
```

### 3. Infrastructure Extension

```csharp
// Infrastructure/Http/IN8nWorkflowClient.cs
public interface IN8nWorkflowClient
{
    Task<HttpResponseMessage> GetWorkflowsAsync(CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> GetWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> CreateWorkflowAsync(string jsonPayload, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> UpdateWorkflowAsync(string id, string jsonPayload, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> DeleteWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> SetWorkflowActiveAsync(string id, bool active, CancellationToken cancellationToken = default);
}

// Infrastructure/Http/N8nWorkflowClient.cs - Extends existing N8nHttpClient pattern
public class N8nWorkflowClient : IN8nWorkflowClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<N8nWorkflowClient> _logger;
    private readonly string _baseApiUrl = "http://localhost:5678/api/v1";
    
    // Implementation follows existing N8nHttpClient pattern
}
```

### 4. Command Implementations

```csharp
// Commands/ListWorkflowsCommand.cs
[Description("List all n8n workflows")]
public class ListWorkflowsCommand : AsyncCommand<ListWorkflowsCommand.Settings>
{
    private readonly IWorkflowService _workflowService;
    
    public class Settings : CommandSettings
    {
        [Description("Output format")]
        [CommandOption("--format")]
        [DefaultValue("table")]
        public string Format { get; set; } = "table";
    }
}

// Commands/CreateWorkflowCommand.cs
[Description("Create a new workflow from JSON file")]
public class CreateWorkflowCommand : AsyncCommand<CreateWorkflowCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [Description("JSON file containing workflow definition")]
        [CommandArgument(0, "<file>")]
        public string? FilePath { get; set; }
    }
}
```

## API Endpoints Integration

Based on n8n REST API v1 structure:

| Operation | HTTP Method | Endpoint | Purpose |
|-----------|-------------|----------|---------|
| List | GET | `/api/v1/workflows` | Get all workflows |
| Get | GET | `/api/v1/workflows/{id}` | Get specific workflow |
| Create | POST | `/api/v1/workflows` | Create new workflow |
| Update | PUT | `/api/v1/workflows/{id}` | Update existing workflow |
| Delete | DELETE | `/api/v1/workflows/{id}` | Delete workflow |
| Activate | POST | `/api/v1/workflows/{id}/activate` | Activate workflow |
| Deactivate | POST | `/api/v1/workflows/{id}/deactivate` | Deactivate workflow |

## Authentication Integration

```csharp
// Infrastructure/Http/N8nApiConfiguration.cs
public class N8nApiConfiguration
{
    public string BaseUrl { get; set; } = "http://localhost:5678";
    public string ApiKey { get; set; } = string.Empty; // From environment variable
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
}
```

## Error Handling Strategy

```csharp
// Services/WorkflowManagement/Models/WorkflowOperationResult.cs
public record WorkflowOperationResult<T>(
    bool Success,
    T? Data,
    string? ErrorMessage,
    int? HttpStatusCode = null
);

// Custom exceptions for specific scenarios
public class WorkflowNotFoundException : Exception { }
public class WorkflowValidationException : Exception { }
public class WorkflowApiException : Exception { }
```

## File Structure

```
dotnet/src/FlowForge.Console/
├── Commands/
│   ├── ListWorkflowsCommand.cs
│   ├── GetWorkflowCommand.cs
│   ├── CreateWorkflowCommand.cs
│   ├── UpdateWorkflowCommand.cs
│   ├── DeleteWorkflowCommand.cs
│   ├── ActivateWorkflowCommand.cs
│   └── DeactivateWorkflowCommand.cs
├── Services/
│   └── WorkflowManagement/
│       ├── IWorkflowService.cs
│       ├── WorkflowService.cs
│       └── Models/
│           ├── WorkflowSummary.cs
│           ├── WorkflowDetails.cs
│           ├── WorkflowDefinition.cs
│           └── WorkflowOperationResult.cs
├── Infrastructure/
│   └── Http/
│       ├── IN8nWorkflowClient.cs
│       ├── N8nWorkflowClient.cs
│       └── N8nApiConfiguration.cs
└── Utilities/
    ├── JsonValidationService.cs
    └── OutputFormattingService.cs
```

## Dependency Injection Registration

```csharp
// Program.cs additions
services.Configure<N8nApiConfiguration>(configuration.GetSection("N8n"));
services.AddScoped<IN8nWorkflowClient, N8nWorkflowClient>();
services.AddScoped<IWorkflowService, WorkflowService>();
services.AddScoped<IJsonValidationService, JsonValidationService>();
services.AddScoped<IOutputFormattingService, OutputFormattingService>();
```

## Testing Strategy

### Unit Tests Structure
```
tests/FlowForge.Console.Tests/
├── Commands/
│   ├── ListWorkflowsCommandTests.cs
│   ├── CreateWorkflowCommandTests.cs
│   └── ... (one test file per command)
├── Services/
│   └── WorkflowManagement/
│       └── WorkflowServiceTests.cs
└── Infrastructure/
    └── Http/
        └── N8nWorkflowClientTests.cs
```

### Test Scenarios
- **Service Tests**: Mock HTTP client, test business logic
- **Command Tests**: Mock services, test CLI argument parsing and output
- **Integration Tests**: Test against actual n8n API (optional)

## Implementation Priority

1. **Phase 1**: Core infrastructure and list command
   - N8nWorkflowClient with basic HTTP operations
   - WorkflowService with list functionality
   - ListWorkflowsCommand with table output

2. **Phase 2**: Read operations
   - GetWorkflowCommand implementation
   - JSON output formatting

3. **Phase 3**: Write operations
   - CreateWorkflowCommand with file validation
   - ActivateWorkflowCommand and DeactivateWorkflowCommand

4. **Phase 4**: Advanced operations
   - UpdateWorkflowCommand
   - DeleteWorkflowCommand with confirmation

5. **Phase 5**: Error handling and output formatting enhancements

## Configuration Requirements

### Environment Variables
- `N8N_API_KEY`: API key for n8n authentication
- `N8N_BASE_URL`: Base URL for n8n instance (default: http://localhost:5678)

### appsettings.json
```json
{
  "N8n": {
    "BaseUrl": "http://localhost:5678",
    "Timeout": "00:00:30"
  }
}
```

## Success Metrics
- All workflow commands implemented with passing tests
- 90% code coverage on WorkflowService
- Consistent error handling across all commands
- Both table and JSON output formats working
- Performance: <2s response time for list operations

---
*Created: 2025-07-13*
*Next: Begin implementation with Issue #22 (List Workflows)*