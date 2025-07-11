# Claude n8n Tools Reference

This document describes the n8n API tools available for Claude Code to manage workflows.

## Tool: n8n-api.sh

**Description**: Command-line wrapper for n8n API operations

**Usage Pattern**: `scripts/n8n-api.sh [command] [arguments]`

## Available Commands

### Workflow Management
- `list-workflows` - Returns JSON array of all workflows
- `get-workflow [id]` - Returns complete workflow JSON by ID
- `create-workflow [file]` - Creates new workflow from JSON file, returns created workflow
- `update-workflow [id] [file]` - Updates existing workflow, returns updated workflow
- `delete-workflow [id]` - Deletes workflow, returns deletion confirmation
- `activate-workflow [id]` - Activates workflow for execution
- `deactivate-workflow [id]` - Deactivates workflow

### Execution Management
- `list-executions [workflow-id]` - Lists executions (all or for specific workflow)
- `get-execution [execution-id]` - Returns execution details and logs

### Credential Management
- `list-credentials` - Returns all available credentials
- `get-credential [id]` - Returns credential details (without sensitive data)
- `create-credential [file]` - Creates new credential from JSON file
- `test-credential [id]` - Tests credential connection

### Utilities
- `get-workflow-status [id]` - Returns workflow status and health
- `raw [method] [endpoint] [data]` - Direct API call for advanced operations

## Example Usage Patterns

```bash
# List all workflows
scripts/n8n-api.sh list-workflows

# Create a new workflow
scripts/n8n-api.sh create-workflow my-workflow.json

# Get specific workflow details
scripts/n8n-api.sh get-workflow ABC123

# Activate a workflow
scripts/n8n-api.sh activate-workflow ABC123

# Check execution history
scripts/n8n-api.sh list-executions ABC123
```

## Return Format
All commands return JSON responses that can be parsed with `jq` for specific data extraction.

## Error Handling
Commands return appropriate error messages and exit codes for automation scripts.