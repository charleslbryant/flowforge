# CLAUDE.md

This file provides guidance to AI assistants when working with code in this repository.

## AI Assistant Identity
AI assistants working on this project should identify themselves as "George" in commits and contributions to maintain consistent attribution across different AI models and sessions.

## Project Overview

FlowForge is an AI-powered workflow automation CLI tool that integrates Claude Code with n8n workflow management. It generates n8n workflows from natural language prompts and has two implementations:

1. **Bash Implementation** (`/scripts/`) - Production-ready original implementation
2. **.NET Implementation** (`/dotnet/`) - Modern port with improved architecture (in development)

## Context Management

### Session Startup (Required Reading)
1. **Product Context**: Read `/docs/product/` for foundational product vision and strategy
2. **Architecture**: Read `/docs/architecture.md` for FlowForge overview and dual-implementation approach
3. **Current State**: Read `/docs/session-context/CURRENT_STATE.md` for development progress
4. **Task Queue**: Read `/docs/session-context/NEXT_TASKS.md` for prioritized work
5. **Focus**: Select ONE task from "Now" priority and work on it using TDD approach

### Session Workflow
1. Complete session startup reading (above)
2. Work on ONE task from the "Now" priority queue
3. Use Test-Driven Development (TDD) approach for .NET development
4. Update relevant ADRs for architectural decisions
5. Update documentation as needed
6. Commit changes with descriptive message
7. Update context files for next session
8. End session (clear context)

### Context Files
- `product/product-vision.md` - Long-term product vision and goals
- `product/product-strategy.md` - Go-to-market and growth strategy
- `architecture.md` - Technical overview and dual-implementation approach
- `CURRENT_STATE.md` - Current project state and development progress
- `NEXT_TASKS.md` - Prioritized task queue linked to GitHub Issues
- `ACTIVE_SESSION.md` - Current session progress tracking

## Key Architecture

### Bash Implementation (`/scripts/`)
- **`forge`** - Main CLI tool (bash script) that handles installation, validation, and formatting
- **`n8n-api.sh`** - Custom shell script providing comprehensive n8n API access
- **`workflow.json`** - Generated n8n workflow file (populated by Claude using the API script)
- **`workflow.prompt.yaml`** - Structured prompt template for workflow generation
- **`templates/`** - Directory containing JSON workflow templates
- **API Integration** - Direct n8n API access through authenticated shell script

### .NET Implementation (`/dotnet/`)
- **Commands Layer** - CLI command implementations using Spectre.Console.Cli
- **Services Layer** - Business logic (HealthChecking, ProcessManagement, SystemChecking)
- **Infrastructure Layer** - External dependencies (Http, Process)
- **Test-Driven Development** - Comprehensive test coverage with xUnit and Moq
- **Clean Architecture** - Proper separation of concerns and dependency injection

## Common Commands

### Flow Metrics Commands

#### Generate Flow Analysis Reports
```bash
# Generate comprehensive flow metrics analysis
./scripts/flow-analysis.sh

# View current flow dashboard
cat docs/metrics/reports/dashboard.md

# View weekly summary
cat docs/metrics/reports/weekly-summary.md
```

### Task Management Commands

#### Review Available Work
```bash
# Show unassigned tasks ready for work
gh issue list --label "task" --no-assignee --label "now,next" --json number,title,labels

# Show my currently assigned tasks
gh issue list --assignee "charleslbryant" --label "task" --state "open" --json number,title,labels

# Show all PRDs and CRDs for context
gh issue list --label "prd,crd" --state "open" --json number,title,labels
```

#### Task Assignment and Management
```bash
# Assign task to user and prioritize
gh issue edit [number] --add-assignee "charleslbryant"
gh issue edit [number] --add-label "now" --remove-label "next,future"

# Create new issues collaboratively
gh issue create --title "[PRD/CRD/Task]: [Title]" --body-file temp.md --label "[appropriate-labels]"
```

### .NET Development Commands
```bash
# Build and test .NET implementation
cd dotnet
dotnet build                    # Build the solution
dotnet test                     # Run all tests
dotnet run --project src/FlowForge.Console -- health    # Run health command
dotnet run --project src/FlowForge.Console -- doctor    # Run doctor command
dotnet run --project src/FlowForge.Console -- start     # Run start command

# TDD Development Workflow
dotnet test --watch             # Continuous testing during development
```

### n8n Process Management (Bash Implementation)
```bash
# Start n8n in background
scripts/forge start

# Stop n8n process
scripts/forge stop

# Restart n8n process
scripts/forge restart

# Check n8n health and API status
scripts/forge health

# Full system diagnostic
scripts/forge doctor
```

### n8n API Management
```bash
# Workflow Operations
scripts/n8n-api.sh list-workflows              # List all workflows
scripts/n8n-api.sh get-workflow [id]          # Get workflow by ID
scripts/n8n-api.sh create-workflow [file]     # Create workflow from JSON file
scripts/n8n-api.sh update-workflow [id] [file] # Update workflow from JSON file
scripts/n8n-api.sh delete-workflow [id]       # Delete workflow by ID
scripts/n8n-api.sh activate-workflow [id]     # Activate workflow
scripts/n8n-api.sh deactivate-workflow [id]   # Deactivate workflow

# Execution Operations
scripts/n8n-api.sh list-executions [id]       # List workflow executions (optional workflow-id)
scripts/n8n-api.sh get-execution [id]         # Get execution details

# Credential Operations
scripts/n8n-api.sh list-credentials           # List all credentials
scripts/n8n-api.sh get-credential [id]       # Get credential by ID
scripts/n8n-api.sh create-credential [file]  # Create credential from JSON file
scripts/n8n-api.sh test-credential [id]      # Test credential

# Utility Operations
scripts/n8n-api.sh get-workflow-status [id]  # Get workflow status
scripts/n8n-api.sh raw [method] [endpoint] [data] # Raw API call
```

### Development and Testing
```bash
# Install dependencies and setup n8n + API script
make install
# or
scripts/forge install

# Run demo workflow generation (requires n8n running)
make demo

# Validate workflow.json against n8n schema
scripts/forge validate

# Format workflow.json with jq
scripts/forge format

# Use Claude with n8n API to create workflows
scripts/n8n-api.sh create-workflow workflow.json
```

### Installation Scripts
```bash
# Full setup with n8n and API configuration
bash scripts/install-n8n-claude-mcp.sh

# Clean removal of n8n and reset
bash scripts/nuke.sh
```

## Workflow Generation Process

### Streamlined Creation (Recommended)
```bash
# One-command workflow creation
scripts/forge create-workflow "your workflow description"
```

This single command:
1. **Checks system health** - Validates n8n and API connectivity
2. **Generates workflow** - Uses Claude with template context
3. **Validates JSON** - Ensures proper formatting
4. **Creates in n8n** - Deploys via API
5. **Opens in browser** - Automatically opens workflow editor

### Manual Process (Advanced)
1. **Start n8n**: `scripts/forge start` (if not already running)
2. **Verify system**: `scripts/forge doctor` (should show "Ready to run")
3. **Generate workflow**: Create JSON file and use `scripts/n8n-api.sh create-workflow [file]`
4. **Validate**: `scripts/forge validate`
5. **Format**: `scripts/forge format`

### Available Templates
Claude is aware of these workflow templates in `templates/`:
- **email-workflow.json** - Email processing with IMAP and filtering
- **web-scraper.json** - Web scraping with HTTP requests and data processing
- **slack-notification.json** - Slack messaging with scheduled triggers

These templates are automatically included in Claude's context when generating workflows.

## Troubleshooting

### Common Issues

**"n8n process not found"**
```bash
scripts/forge start
```

**"Unauthorized" API errors**
- Generate new API key at http://localhost:5678/settings/api
- Export key: `export N8N_API_KEY="your-new-key"`

**Empty workflow.json**
- Check n8n is running: `scripts/forge health`
- Verify API key is valid
- Check n8n logs: `tail -f ~/n8n.log`

**System health check**
```bash
scripts/forge doctor  # Shows all issues and solutions
```

## API Configuration

The n8n-api.sh script is configured with:
- n8n API connection on localhost:5678
- Session-based authentication using browser cookies
- API key authentication for secure access
- Environment variables for N8N_API_KEY

## Prompt Template Structure

YAML templates should include:
- `task`: High-level workflow description
- `context`: Platform and tools specification
- `purpose`: Specific workflow objective
- `constraints`: Technical requirements
- `output_format`: Expected output type (n8n_workflow_json)

## Platform Requirements

- Ubuntu/WSL2 environment
- Node.js with npm
- n8n installed globally
- Claude Code CLI installed globally
- jq for JSON formatting
- ajv-cli for schema validation