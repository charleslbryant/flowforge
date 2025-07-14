# Get Workflow Command User Guide

The `get-workflow` command retrieves detailed information and the JSON definition for a specific workflow from your n8n instance.

## Usage

```bash
# Display workflow details in table format (default)
forge-dotnet get-workflow <workflow-id>

# Display workflow details in JSON format
forge-dotnet get-workflow <workflow-id> --json
```

## Examples

```bash
# Get workflow with ID "1"
forge-dotnet get-workflow 1

# Get workflow details as JSON
forge-dotnet get-workflow 1 --json
```

## Output Formats

### Table Format (Default)

The default table format provides a clean, readable view of the workflow details:

```
 __        __                 _       __   _                    
 \ \      / /   ___    _ __  | | __  / _| | |   ___   __      __
  \ \ /\ / /   / _ \  | '__| | |/ / | |_  | |  / _ \  \ \ /\ / /
   \ V  V /   | (_) | | |    |   <  |  _| | | | (_) |  \ V  V / 
    \_/\_/     \___/  |_|    |_|\_\ |_|   |_|  \___/    \_/\_/  

┌─────────────┬─────────────────────┐
│ Property    │ Value               │
├─────────────┼─────────────────────┤
│ ID          │ 1                   │
│ Name        │ My First Flow       │
│ Status      │ Active              │
│ Node Count  │ 5                   │
│ Description │ Process user data   │
│ Tags        │ automation, data    │
│ Created     │ 2025-01-15 14:30:15 │
│ Updated     │ 2025-01-15 16:45:30 │
└─────────────┴─────────────────────┘

Workflow Definition:
[
  {
    "id": "webhook-node",
    "name": "Webhook",
    "type": "n8n-nodes-base.webhook",
    "position": [250, 300]
  },
  {
    "id": "http-request",
    "name": "HTTP Request",
    "type": "n8n-nodes-base.httpRequest",
    "position": [450, 300]
  }
]
```

### JSON Format

Use the `--json` flag for machine-readable output, perfect for scripting and automation:

```bash
forge-dotnet get-workflow 1 --json
```

Example output:
```json
{
  "success": true,
  "workflow": {
    "id": "1",
    "name": "My First Flow",
    "active": true,
    "nodeCount": 5,
    "description": "Process user data",
    "tags": ["automation", "data"],
    "createdAt": "2025-01-15T14:30:15",
    "updatedAt": "2025-01-15T16:45:30",
    "nodes": [
      {
        "id": "webhook-node",
        "name": "Webhook",
        "type": "n8n-nodes-base.webhook",
        "position": [250, 300]
      },
      {
        "id": "http-request",
        "name": "HTTP Request",
        "type": "n8n-nodes-base.httpRequest",
        "position": [450, 300]
      }
    ]
  }
}
```

## Status Indicators

### Table Format
- **Active**: Green text indicating the workflow is enabled and running
- **Inactive**: Dimmed text indicating the workflow is disabled

### JSON Format
- **active**: Boolean field (`true` or `false`)

## Common Use Cases

### Workflow Analysis
```bash
# Get workflow structure for analysis
forge-dotnet get-workflow 1 --json | jq '.workflow.nodes | length'

# Extract node types
forge-dotnet get-workflow 1 --json | jq -r '.workflow.nodes[].type'

# Check if workflow is active
forge-dotnet get-workflow 1 --json | jq '.workflow.active'
```

### Backup and Export
```bash
# Export workflow definition to file
forge-dotnet get-workflow 1 --json > workflow-1-backup.json

# Extract just the workflow definition
forge-dotnet get-workflow 1 --json | jq '.workflow' > workflow-definition.json
```

### Automation Scripts
```bash
# Check last update time
last_update=$(forge-dotnet get-workflow 1 --json | jq -r '.workflow.updatedAt')

# Validate workflow has required nodes
node_count=$(forge-dotnet get-workflow 1 --json | jq '.workflow.nodeCount')
if [ "$node_count" -lt 2 ]; then
    echo "Warning: Workflow has insufficient nodes"
fi
```

## Error Handling

### Workflow Not Found
```
❌ Failed to retrieve workflow: Workflow 999 not found
```

In JSON format:
```json
{
  "error": "Failed to retrieve workflow: Workflow 999 not found",
  "success": false
}
```

### Connection Issues
```
❌ Failed to retrieve workflow: Connection failed
```

In JSON format:
```json
{
  "error": "Failed to retrieve workflow: Connection failed",
  "success": false
}
```

### Invalid Workflow ID
```
❌ Failed to retrieve workflow: Invalid workflow ID format
```

## Troubleshooting

### Workflow Not Found
If you receive "workflow not found" errors:

1. **Verify Workflow ID:**
   ```bash
   forge-dotnet list-workflows
   ```

2. **Check Available Workflows:**
   ```bash
   forge-dotnet list-workflows --json | jq -r '.workflows[].id'
   ```

### Connection Failed
If you see connection errors:

1. **Check n8n Status:**
   ```bash
   forge-dotnet health
   ```

2. **Ensure n8n is Running:**
   ```bash
   forge-dotnet start
   ```

3. **Verify n8n URL Configuration:**
   - Default: `http://localhost:5678`
   - Check your n8n instance is accessible

### Permission Issues
If you receive permission errors:
- Ensure your user has access to the n8n API
- Check n8n authentication settings
- Verify network connectivity to n8n instance

## Related Commands

- [`forge-dotnet list-workflows`](list-workflows-command.md) - List all workflows
- [`forge-dotnet health`](health-command.md) - Check n8n connection status
- [`forge-dotnet start`](start-command.md) - Start n8n process
- [`forge-dotnet doctor`](doctor-command.md) - Comprehensive system diagnostics

## Technical Details

The get-workflow command:
- Connects to the n8n API at the configured endpoint (`/workflows/{id}`)
- Retrieves complete workflow metadata and node definitions
- Supports both human-readable table and machine-readable JSON output
- Implements proper timeout handling (30 seconds)
- Provides detailed error messages for troubleshooting
- Validates workflow ID format before making API requests

For technical implementation details, see the [Developer Guide](../developer-guides/workflow-management.md).