# FlowForge .NET Implementation

This directory contains the .NET port of FlowForge, a console application that provides the same functionality as the original bash implementation.

## Project Structure

```
dotnet/
├── FlowForge.sln                      # Solution file
├── src/
│   └── FlowForge.Console/             # Main console application
│       ├── Commands/                  # Command implementations
│       ├── Services/                  # Core business logic
│       ├── Models/                    # Data models and configuration
│       │   ├── Workflows/
│       │   ├── Executions/
│       │   ├── Credentials/
│       │   └── Config/
│       ├── Infrastructure/            # Cross-cutting concerns
│       │   ├── Configuration/
│       │   ├── Logging/
│       │   ├── Http/
│       │   └── Utilities/
│       ├── Program.cs                 # Entry point
│       └── appsettings.json          # Configuration
└── tests/
    └── FlowForge.Console.Tests/       # Unit and integration tests
        ├── Commands/
        ├── Services/
        └── TestData/
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- n8n installed and running
- Claude CLI installed

### Building

```bash
cd dotnet
dotnet restore
dotnet build
```

### Running

```bash
cd dotnet/src/FlowForge.Console
dotnet run -- --help
```

### Configuration

Configuration is handled through:
1. `appsettings.json` - Base configuration
2. `appsettings.Development.json` - Development overrides
3. Environment variables with `FLOWFORGE_` prefix

Key environment variables:
- `FLOWFORGE_N8N__APIKEY` - n8n API key
- `FLOWFORGE_N8N__HOST` - n8n host (default: localhost)
- `FLOWFORGE_N8N__PORT` - n8n port (default: 5678)

### Commands

The .NET implementation provides the same commands as the bash version:

- `create-workflow "description"` - Generate and create workflow from natural language
- `start` - Start n8n process
- `stop` - Stop n8n process  
- `restart` - Restart n8n process
- `health` - Check n8n status
- `doctor` - Full system diagnostics
- `validate` - Validate workflow.json
- `format` - Format workflow.json

### Testing

```bash
dotnet test
```

## Implementation Status

This is the initial project setup. The following components need to be implemented:

- [ ] Command implementations
- [ ] n8n API client
- [ ] Process management
- [ ] Health checking
- [ ] Workflow generation with Claude
- [ ] Template system
- [ ] Cross-platform browser opening
- [ ] Comprehensive test suite

See [DOTNET_PORT_PLAN.md](../DOTNET_PORT_PLAN.md) for the complete implementation roadmap.