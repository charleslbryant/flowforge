# FlowForge Architecture

## Product Overview

**FlowForge** is an AI-powered workflow automation CLI tool that integrates Claude Code with n8n workflow management. It enables users to generate, validate, and deploy n8n workflows using natural language prompts through both bash and .NET implementations.

## Core Purpose
- **Generate n8n workflows** from natural language descriptions
- **Validate and format** workflow JSON against n8n schemas
- **Deploy workflows** directly to n8n instances via API
- **Manage n8n lifecycle** (start, stop, health checks)

## Architecture Overview

FlowForge has **two implementation tracks** for the same CLI functionality:

### 1. Original Bash Implementation (Production)
Located in `/scripts/` - The current production implementation

### 2. .NET Port Implementation (In Development)  
Located in `/dotnet/` - Modern .NET port with improved architecture

## Bash Implementation Architecture

### Core Components

```
scripts/
├── forge                    # Main CLI entry point
├── n8n-api.sh              # n8n API integration layer
├── install-n8n-claude-mcp.sh  # Installation script
└── nuke.sh                 # Clean removal script

templates/
├── email-workflow.json      # Email processing template
├── web-scraper.json        # Web scraping template
└── slack-notification.json # Notification template

workflow.json               # Generated workflow (temporary)
workflow.prompt.yaml        # Generated prompt (temporary)
```

### Key Scripts

#### `forge` - Main CLI Tool
- **Commands**: `start`, `stop`, `restart`, `health`, `doctor`, `create-workflow`, `validate`, `format`
- **Dependencies**: bash, jq, curl, nc, gh, claude CLI
- **Purpose**: Orchestrates all FlowForge operations

#### `n8n-api.sh` - API Integration
- **Authentication**: Session cookies + API key support
- **Operations**: CRUD operations for workflows, executions, credentials
- **Error Handling**: Graceful API error management
- **Purpose**: Abstracts n8n REST API complexity

### Workflow Generation Process
```
User Input → Prompt Generation → Claude Processing → JSON Cleanup → n8n Deployment → Browser Opening
```

## .NET Implementation Architecture

### Clean Architecture Approach

```
dotnet/src/FlowForge.Console/
├── Commands/               # CLI command implementations
│   ├── HealthCommand.cs
│   ├── DoctorCommand.cs
│   └── StartCommand.cs
├── Services/              # Business logic layer
│   ├── HealthChecking/
│   ├── ProcessManagement/
│   └── SystemChecking/
├── Infrastructure/        # External dependencies
│   ├── Http/
│   └── Process/
└── Program.cs            # DI container and CLI setup

dotnet/tests/FlowForge.Console.Tests/
└── Commands/             # Comprehensive test coverage
```

### Layer Responsibilities

#### Commands Layer
- **Purpose**: CLI command parsing and orchestration
- **Framework**: Spectre.Console.Cli for rich terminal output
- **Pattern**: Each command focuses on single responsibility
- **Dependencies**: Services layer only

#### Services Layer
- **HealthChecking**: n8n API health validation
- **ProcessManagement**: n8n process lifecycle management  
- **SystemChecking**: System requirements validation
- **Pattern**: Interface-based with dependency injection
- **Purpose**: Core business logic, independent of infrastructure

#### Infrastructure Layer
- **Http**: n8n API communication via HttpClient
- **Process**: System process management and execution
- **Pattern**: Implements service interfaces
- **Purpose**: External dependency abstraction

### Key Design Decisions

#### Technology Choices
- **.NET 8**: Modern platform with excellent CLI support
- **Spectre.Console**: Rich terminal output and command parsing
- **xUnit + Moq**: Test-driven development approach
- **Microsoft.Extensions.DI**: Native dependency injection

#### Architectural Patterns
- **Clean Architecture**: Clear separation of concerns
- **Dependency Injection**: Loose coupling and testability
- **Command Pattern**: Each CLI command is a separate class
- **Repository Pattern**: Infrastructure abstraction
- **Test-Driven Development**: Red-green-refactor cycles

## Integration Points

### n8n Integration
- **API Endpoints**: REST API on localhost:5678
- **Authentication**: Session cookies + JWT API keys
- **Operations**: Workflows, executions, credentials management
- **Health Checks**: Connection validation and API testing

### Claude Integration
- **CLI Tool**: `claude --print` for natural language processing
- **Context**: Template-aware prompt generation
- **Output**: Clean JSON workflow generation
- **Error Handling**: Automatic cleanup and validation

### System Dependencies
- **Required**: Node.js, npm, n8n, claude CLI, jq (bash version)
- **Optional**: Browser for workflow editing interface
- **Platform**: Cross-platform (Linux, macOS, Windows)

## Development Approach

### Current State
- **Bash Implementation**: ✅ Production ready, full feature set
- **.NET Implementation**: 🚧 In development, basic commands implemented

### Implementation Parity
The .NET port aims for **feature parity** with the bash implementation:

| Feature | Bash | .NET | Status |
|---------|------|------|--------|
| Health Check | ✅ | ✅ | Complete |
| System Doctor | ✅ | ✅ | Complete |
| Start n8n | ✅ | ✅ | Complete |
| Stop n8n | ✅ | ⏳ | Next |
| Restart n8n | ✅ | ⏳ | Next |
| Create Workflow | ✅ | ⏳ | Planned |
| Validate JSON | ✅ | ⏳ | Planned |
| Format JSON | ✅ | ⏳ | Planned |

### Testing Strategy

#### Bash Implementation
- **Manual Testing**: Command validation and integration testing
- **System Tests**: Full end-to-end workflow validation
- **Platform Testing**: Cross-platform compatibility validation

#### .NET Implementation  
- **Unit Tests**: TDD approach with comprehensive coverage
- **Integration Tests**: Service integration validation
- **Mocking**: External dependencies mocked for isolation
- **Current Coverage**: 9/9 tests passing, all major components covered

## File Organization

### Project Structure
```
flowforge/
├── scripts/              # Bash implementation
├── templates/            # Workflow templates (shared)
├── dotnet/              # .NET implementation
├── docs/                # Documentation
│   ├── product/         # Product vision and strategy
│   ├── adr/             # Architecture Decision Records
│   ├── session-context/ # CMDS session management
│   └── architecture.md  # This document
└── .github/workflows/   # CI/CD automation
```

### Shared Resources
- **Templates**: Both implementations use same workflow templates
- **Documentation**: Shared ADRs and process documentation
- **CI/CD**: Automated testing and issue syncing

## Future Architecture Considerations

### Convergence Strategy
- **Phase 1**: Complete .NET feature parity
- **Phase 2**: Performance and UX improvements in .NET
- **Phase 3**: Gradual migration from bash to .NET
- **Phase 4**: Bash implementation deprecated (optional)

### Extensibility
- **Plugin System**: Template-based workflow generation
- **API Extensions**: Additional n8n API operations
- **Integration Points**: Additional workflow platforms
- **Configuration**: Environment-specific settings

### Quality Assurance
- **ADR Documentation**: All architectural decisions recorded
- **Test Coverage**: Comprehensive test suites for both implementations
- **Cross-Platform**: Consistent behavior across operating systems
- **Error Handling**: Graceful degradation and clear error messages

## Context for Development Sessions

### Essential Context
1. **Product**: CLI tool for n8n workflow automation using AI
2. **Dual Implementation**: Bash (production) + .NET (development)
3. **Current Focus**: .NET port with TDD approach
4. **Architecture**: Clean Architecture with Services/Infrastructure separation

### Session Startup Context
1. Read `/docs/product/` for foundational product vision and strategy
2. Read `/docs/session-context/CURRENT_STATE.md` for current progress
3. Read `/docs/session-context/NEXT_TASKS.md` for prioritized work
4. Understand this architecture document for technical context
5. Reference existing bash implementation for feature requirements

---

*This architecture document should be read at the start of each development session to understand FlowForge's dual-implementation approach and current development focus.*