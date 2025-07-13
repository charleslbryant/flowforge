# FlowForge .NET Port Plan

## Overview

This document outlines the comprehensive plan for porting FlowForge from a Bash-based CLI tool to a .NET console application, maintaining all core functionality while leveraging .NET's advantages in type safety, error handling, and cross-platform compatibility.

## Current Architecture Analysis

### Core Components

**Current FlowForge consists of:**
- **`forge`** - Main CLI tool (bash script) handling workflow creation, n8n management, health checks
- **`n8n-api.sh`** - Comprehensive HTTP API wrapper for all n8n operations (15+ commands)
- **Templates** - YAML/JSON workflow templates providing context for Claude
- **Process Management** - Start/stop n8n processes, health monitoring, diagnostics

### Key Functionality Mapping

| Current (Bash) | .NET Equivalent | Implementation |
|---|---|---|
| `forge create-workflow` | `CreateWorkflowCommand` | System.CommandLine + Claude process execution |
| `n8n-api.sh` operations | `N8nApiClient` | HttpClient with typed methods |
| Process management | `ProcessManager` | System.Diagnostics.Process |
| Health checks | `HealthChecker` | HTTP health endpoints + process validation |
| JSON validation | Built-in validation | System.Text.Json with schema validation |

## .NET Project Structure

```
FlowForge.Console/
├── Program.cs                          # Main CLI entry point with System.CommandLine
├── Commands/                           # Command pattern implementation
│   ├── CreateWorkflowCommand.cs        # Natural language → n8n workflow
│   ├── StartCommand.cs                 # Start n8n process
│   ├── StopCommand.cs                  # Stop n8n process
│   ├── RestartCommand.cs               # Restart n8n process
│   ├── HealthCommand.cs                # Quick n8n status check
│   ├── DoctorCommand.cs                # Comprehensive system diagnostics
│   ├── ValidateCommand.cs              # Workflow JSON validation
│   └── FormatCommand.cs                # JSON formatting
├── Services/                           # Core business logic
│   ├── N8nApiClient.cs                 # HTTP client for n8n API
│   ├── ProcessManager.cs               # n8n process lifecycle management
│   ├── HealthChecker.cs                # System health diagnostics
│   ├── WorkflowGenerator.cs            # Claude CLI integration
│   ├── TemplateService.cs              # Template loading and management
│   └── BrowserService.cs               # Cross-platform browser opening
├── Models/                             # Strongly-typed data models
│   ├── Workflows/
│   │   ├── WorkflowRequest.cs
│   │   ├── WorkflowResponse.cs
│   │   └── WorkflowNode.cs
│   ├── Executions/
│   │   ├── ExecutionRequest.cs
│   │   └── ExecutionResponse.cs
│   ├── Credentials/
│   │   ├── CredentialRequest.cs
│   │   └── CredentialResponse.cs
│   └── Config/
│       ├── N8nConfiguration.cs
│       └── ClaudeConfiguration.cs
├── Infrastructure/                     # Cross-cutting concerns
│   ├── Configuration/
│   │   ├── ConfigurationExtensions.cs
│   │   └── EnvironmentVariables.cs
│   ├── Logging/
│   │   ├── ConsoleLogger.cs
│   │   └── LoggingExtensions.cs
│   ├── Http/
│   │   ├── HttpClientExtensions.cs
│   │   └── N8nHttpMessageHandler.cs
│   └── Utilities/
│       ├── JsonUtilities.cs
│       ├── ProcessUtilities.cs
│       └── PathUtilities.cs
├── Templates/                          # Embedded resources
│   ├── email-workflow.json
│   ├── web-scraper.json
│   └── slack-notification.json
└── Tests/
    ├── FlowForge.Console.Tests/        # Unit tests
    ├── FlowForge.Console.Integration/  # Integration tests
    └── FlowForge.Console.E2E/          # End-to-end tests
```

## Core Technology Stack

### Dependencies

```xml
<PackageReference Include="System.CommandLine" Version="2.0.0" />
<PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Logging" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Http" Version="8.0.0" />
<PackageReference Include="System.Text.Json" Version="8.0.0" />
<PackageReference Include="NJsonSchema" Version="11.0.0" />
```

### Key Framework Choices

- **CLI Framework:** System.CommandLine for robust command parsing and help generation
- **HTTP Client:** HttpClient with IHttpClientFactory for n8n API operations
- **Process Management:** System.Diagnostics.Process for n8n lifecycle management
- **Configuration:** Microsoft.Extensions.Configuration with appsettings.json + environment variables
- **Logging:** Microsoft.Extensions.Logging with console provider
- **JSON:** System.Text.Json for workflow serialization with NJsonSchema for validation
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection

## Detailed Component Design

### N8nApiClient Service

```csharp
public interface IN8nApiClient
{
    // Workflow Operations (mirrors n8n-api.sh)
    Task<WorkflowResponse[]> ListWorkflowsAsync(CancellationToken cancellationToken = default);
    Task<WorkflowResponse> GetWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task<WorkflowResponse> CreateWorkflowAsync(WorkflowRequest workflow, CancellationToken cancellationToken = default);
    Task<WorkflowResponse> UpdateWorkflowAsync(string id, WorkflowRequest workflow, CancellationToken cancellationToken = default);
    Task DeleteWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task ActivateWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task DeactivateWorkflowAsync(string id, CancellationToken cancellationToken = default);
    
    // Execution Operations
    Task<ExecutionResponse[]> ListExecutionsAsync(string? workflowId = null, CancellationToken cancellationToken = default);
    Task<ExecutionResponse> GetExecutionAsync(string id, CancellationToken cancellationToken = default);
    
    // Credential Operations
    Task<CredentialResponse[]> ListCredentialsAsync(CancellationToken cancellationToken = default);
    Task<CredentialResponse> GetCredentialAsync(string id, CancellationToken cancellationToken = default);
    Task<CredentialResponse> CreateCredentialAsync(CredentialRequest credential, CancellationToken cancellationToken = default);
    Task TestCredentialAsync(string id, CancellationToken cancellationToken = default);
    
    // Utility Operations
    Task<WorkflowStatusResponse> GetWorkflowStatusAsync(string id, CancellationToken cancellationToken = default);
    Task<T> RawApiCallAsync<T>(HttpMethod method, string endpoint, object? data = null, CancellationToken cancellationToken = default);
}
```

### Command Architecture

```csharp
public abstract class BaseCommand
{
    protected readonly ILogger Logger;
    protected readonly N8nConfiguration Config;
    protected readonly IN8nApiClient ApiClient;
    protected readonly IHealthChecker HealthChecker;
    
    protected BaseCommand(ILogger logger, N8nConfiguration config, IN8nApiClient apiClient, IHealthChecker healthChecker)
    {
        Logger = logger;
        Config = config;
        ApiClient = apiClient;
        HealthChecker = healthChecker;
    }
    
    public abstract Task<int> ExecuteAsync(CancellationToken cancellationToken);
}
```

### CreateWorkflowCommand Implementation

**Key Features:**
- Natural language prompt processing
- Template context integration
- Claude CLI process execution with stdin/stdout handling
- JSON cleanup (remove markdown wrappers)
- Schema validation
- Automatic n8n deployment
- Cross-platform browser opening

```csharp
public class CreateWorkflowCommand : BaseCommand
{
    private readonly IWorkflowGenerator _workflowGenerator;
    private readonly ITemplateService _templateService;
    private readonly IBrowserService _browserService;
    
    public async Task<int> ExecuteAsync(string prompt, CancellationToken cancellationToken)
    {
        // 1. Health check
        var healthResult = await HealthChecker.CheckHealthAsync(cancellationToken);
        if (!healthResult.IsHealthy)
        {
            Logger.LogError("n8n is not healthy. Run 'forge doctor' for diagnosis.");
            return 1;
        }
        
        // 2. Generate workflow with Claude
        var templateContext = await _templateService.GetTemplateContextAsync(cancellationToken);
        var workflowJson = await _workflowGenerator.GenerateWorkflowAsync(prompt, templateContext, cancellationToken);
        
        // 3. Validate and create in n8n
        var workflow = JsonSerializer.Deserialize<WorkflowRequest>(workflowJson);
        var createdWorkflow = await ApiClient.CreateWorkflowAsync(workflow, cancellationToken);
        
        // 4. Open in browser
        var workflowUrl = $"{Config.BaseUrl}/workflow/{createdWorkflow.Id}";
        await _browserService.OpenUrlAsync(workflowUrl, cancellationToken);
        
        Logger.LogInformation("✅ Workflow created successfully with ID: {WorkflowId}", createdWorkflow.Id);
        return 0;
    }
}
```

### Configuration Management

**appsettings.json:**
```json
{
  "N8n": {
    "Host": "localhost",
    "Port": 5678,
    "UseHttps": false,
    "ApiKey": "",
    "ProcessPath": "n8n",
    "LogPath": "~/n8n.log",
    "StartupTimeoutSeconds": 30
  },
  "Claude": {
    "Command": "claude",
    "Arguments": ["--print"],
    "TimeoutSeconds": 60
  },
  "Templates": {
    "Directory": "Templates",
    "EmbeddedResourceNamespace": "FlowForge.Console.Templates"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "FlowForge": "Debug"
    }
  }
}
```

**Environment Variable Support:**
- `N8N_API_KEY` - API key for authentication
- `N8N_HOST` - Override default host
- `N8N_PORT` - Override default port
- `CLAUDE_COMMAND` - Override Claude CLI command

### Cross-Platform Considerations

#### Process Management
```csharp
public class ProcessManager : IProcessManager
{
    public async Task<bool> IsN8nRunningAsync()
    {
        var processName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "n8n.exe" : "n8n";
        var processes = Process.GetProcessesByName(processName);
        return processes.Length > 0;
    }
    
    public async Task StopN8nAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            await ExecuteCommandAsync("taskkill", "/f /im n8n.exe");
        }
        else
        {
            await ExecuteCommandAsync("pkill", "-f n8n");
        }
    }
}
```

#### Browser Opening
```csharp
public class BrowserService : IBrowserService
{
    public Task OpenUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        var startInfo = new ProcessStartInfo(url)
        {
            UseShellExecute = true
        };
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            startInfo.FileName = "xdg-open";
            startInfo.Arguments = url;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            startInfo.FileName = "open";
            startInfo.Arguments = url;
        }
        
        Process.Start(startInfo);
        return Task.CompletedTask;
    }
}
```

## Testing Strategy

### Test Framework Structure

**Unit Tests (XUnit + Moq):**
```csharp
public class CreateWorkflowCommandTests
{
    private readonly Mock<IN8nApiClient> _mockApiClient;
    private readonly Mock<IWorkflowGenerator> _mockGenerator;
    private readonly Mock<IHealthChecker> _mockHealthChecker;
    private readonly CreateWorkflowCommand _command;
    
    [Fact]
    public async Task ExecuteAsync_WithValidPrompt_CreatesWorkflowSuccessfully()
    {
        // Arrange
        _mockHealthChecker.Setup(x => x.CheckHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = true });
        
        // Act & Assert
        var result = await _command.ExecuteAsync("test prompt", CancellationToken.None);
        Assert.Equal(0, result);
    }
}
```

**Integration Tests (TestContainers):**
```csharp
public class N8nApiClientIntegrationTests : IAsyncLifetime
{
    private readonly N8nContainer _n8nContainer;
    private readonly IN8nApiClient _apiClient;
    
    [Fact]
    public async Task CreateWorkflow_WithValidWorkflow_ReturnsCreatedWorkflow()
    {
        // Test against real n8n instance in container
    }
}
```

**CLI Testing (System.CommandLine.Testing):**
```csharp
public class CliTests
{
    [Fact]
    public async Task CreateWorkflowCommand_WithPrompt_ExecutesSuccessfully()
    {
        var command = new RootCommand();
        // Add commands
        
        var result = await command.InvokeAsync("create-workflow \"test prompt\"");
        Assert.Equal(0, result);
    }
}
```

## Implementation Roadmap

### Phase 1: Foundation (Week 1)
**Deliverables:**
- [ ] Project setup with System.CommandLine
- [ ] Basic command structure and routing
- [ ] Configuration system with appsettings.json + environment variables
- [ ] Logging infrastructure
- [ ] Basic CLI commands (help, version)

**Success Criteria:**
- CLI runs and displays help
- Configuration loads correctly
- Logging works to console

### Phase 2: Process Management (Week 2)
**Deliverables:**
- [ ] Cross-platform process detection
- [ ] Start/stop/restart commands
- [ ] Health checking infrastructure
- [ ] Doctor command with comprehensive diagnostics

**Success Criteria:**
- Can start/stop n8n on all platforms
- Health checks work reliably
- Doctor command identifies common issues

### Phase 3: n8n API Integration (Week 3)
**Deliverables:**
- [ ] Complete N8nApiClient implementation
- [ ] All workflow CRUD operations
- [ ] Execution and credential management
- [ ] Error handling and retry logic

**Success Criteria:**
- All n8n API operations work
- Proper error handling for API failures
- API client passes integration tests

### Phase 4: Workflow Generation (Week 4)
**Deliverables:**
- [ ] Claude CLI integration
- [ ] Template system with embedded resources
- [ ] Prompt generation and cleanup
- [ ] End-to-end create-workflow command
- [ ] JSON validation and formatting

**Success Criteria:**
- Can generate workflows from natural language
- Templates are properly integrated
- Workflows validate against n8n schema

### Phase 5: Testing & Polish (Week 5)
**Deliverables:**
- [ ] Comprehensive test suite (unit, integration, E2E)
- [ ] Error handling improvements
- [ ] Performance optimizations
- [ ] Documentation and packaging
- [ ] CI/CD pipeline

**Success Criteria:**
- >90% test coverage
- All edge cases handled gracefully
- Ready for production use

## Risk Assessment & Mitigation

### High Risks
1. **Claude CLI Integration Complexity**
   - *Risk:* Process execution and stdin/stdout handling
   - *Mitigation:* Extensive testing with mock processes, fallback error handling

2. **Cross-Platform Process Management**
   - *Risk:* Different process APIs on Windows vs Linux/Mac
   - *Mitigation:* Abstract process operations behind interfaces, platform-specific implementations

3. **n8n API Breaking Changes**
   - *Risk:* API changes breaking our client
   - *Mitigation:* Version-specific API clients, comprehensive integration tests

### Medium Risks
1. **JSON Schema Validation**
   - *Risk:* n8n workflow schema changes
   - *Mitigation:* Downloadable schema validation, graceful degradation

2. **Template System Complexity**
   - *Risk:* Managing embedded vs file-based templates
   - *Mitigation:* Hybrid approach with clear fallback mechanisms

## Success Metrics

### Functionality Parity
- [ ] All current `forge` commands implemented
- [ ] All `n8n-api.sh` operations available
- [ ] Workflow generation maintains quality
- [ ] Performance matches or exceeds bash version

### Quality Improvements
- [ ] Strong typing throughout
- [ ] Comprehensive error handling
- [ ] >90% test coverage
- [ ] Cross-platform compatibility verified

### Developer Experience
- [ ] Clear documentation
- [ ] Easy setup and configuration
- [ ] Helpful error messages
- [ ] Consistent CLI interface

## Future Enhancements

### Post-MVP Features
1. **Multi-Bot Pipeline Integration** - Support for the delivery pipeline from `delivery-pipe.md`
2. **Workflow Templates Management** - CRUD operations for custom templates
3. **Batch Operations** - Bulk workflow operations
4. **Monitoring & Metrics** - Workflow execution monitoring
5. **Plugin System** - Extensible command architecture
6. **GUI Components** - Optional web interface for workflow management

This plan provides a comprehensive roadmap for creating a production-ready .NET port of FlowForge while maintaining backward compatibility and improving on the original design.