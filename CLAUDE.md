# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FlowForge is an AI-powered workflow automation tool that integrates Claude Code with n8n workflow management. It generates n8n workflows from natural language prompts using a custom shell script that provides direct API access to n8n.

## Key Architecture

- **`forge`** - Main CLI tool (bash script) that handles installation, validation, and formatting
- **`n8n-api.sh`** - Custom shell script providing comprehensive n8n API access
- **`workflow.json`** - Generated n8n workflow file (populated by Claude using the API script)
- **`workflow.prompt.yaml`** - Structured prompt template for workflow generation
- **`prompts/`** - Directory containing YAML prompt templates for different workflow types
- **`scripts/`** - Installation and utility scripts
- **API Integration** - Direct n8n API access through authenticated shell script

## Common Commands

### n8n Process Management
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