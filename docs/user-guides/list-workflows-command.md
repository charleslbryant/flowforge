# List Workflows Command User Guide

The `list-workflows` command retrieves and displays all workflows from your n8n instance.

## Usage

```bash
# Display workflows in table format (default)
forge-dotnet list-workflows

# Display workflows in JSON format
forge-dotnet list-workflows --json
```

## Output Formats

### Table Format (Default)

The default table format provides a clean, readable view of your workflows:

```
 __        __                 _       __   _                          
 \ \      / /   ___    _ __  | | __  / _| | |   ___   __      __  ___ 
  \ \ /\ / /   / _ \  | '__| | |/ / | |_  | |  / _ \  \ \ /\ / / / __|
   \ V  V /   | (_) | | |    |   <  |  _| | | | (_) |  \ V  V /  \__ \
    \_/\_/     \___/  |_|    |_|\_\ |_|   |_|  \___/    \_/\_/   |___/
                                                                      

┌────┬─────────────────┬──────────┬───────┬──────────────────┐
│ ID │ Name            │ Status   │ Nodes │ Last Updated     │
├────┼─────────────────┼──────────┼───────┼──────────────────┤
│ 1  │ My First Flow   │ Active   │ 5     │ 2025-01-15 14:30 │
│ 2  │ Data Processor  │ Inactive │ 8     │ 2025-01-14 09:15 │
│ 3  │ Email Alerts    │ Active   │ 3     │ 2025-01-13 16:45 │
└────┴─────────────────┴──────────┴───────┴──────────────────┘

Total: 3 workflows
```

### JSON Format

Use the `--json` flag for machine-readable output, perfect for scripting and automation:

```bash
forge-dotnet list-workflows --json
```

Example output:
```json
{
  "success": true,
  "totalCount": 3,
  "workflows": [
    {
      "id": "1",
      "name": "My First Flow",
      "active": true,
      "nodeCount": 5,
      "updatedAt": "2025-01-15T14:30:00"
    },
    {
      "id": "2",
      "name": "Data Processor",
      "active": false,
      "nodeCount": 8,
      "updatedAt": "2025-01-14T09:15:00"
    },
    {
      "id": "3",
      "name": "Email Alerts",
      "active": true,
      "nodeCount": 3,
      "updatedAt": "2025-01-13T16:45:00"
    }
  ]
}
```

## Status Indicators

### Table Format
- **Active**: Green text indicating the workflow is enabled and running
- **Inactive**: Dimmed text indicating the workflow is disabled

### JSON Format
- **active**: Boolean field (`true` or `false`)

## Common Use Cases

### Scripting and Automation
```bash
# Get active workflows count
active_count=$(forge-dotnet list-workflows --json | jq '.workflows | map(select(.active)) | length')

# List only active workflow names
forge-dotnet list-workflows --json | jq -r '.workflows[] | select(.active) | .name'

# Get workflows updated in the last day
forge-dotnet list-workflows --json | jq '.workflows[] | select(.updatedAt > "2025-01-14")'
```

### Monitoring
```bash
# Check if any workflows exist
if [ $(forge-dotnet list-workflows --json | jq '.totalCount') -eq 0 ]; then
    echo "No workflows found!"
fi
```

## Error Handling

### Connection Issues
```
❌ Failed to retrieve workflows: Connection failed
```

In JSON format:
```json
{
  "error": "Failed to retrieve workflows: Connection failed",
  "success": false
}
```

### No Workflows Found
```
📂 No workflows found
```

In JSON format:
```json
{
  "success": true,
  "totalCount": 0,
  "workflows": []
}
```

## Troubleshooting

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

- [`forge-dotnet health`](health-command.md) - Check n8n connection status
- [`forge-dotnet start`](start-command.md) - Start n8n process
- [`forge-dotnet doctor`](doctor-command.md) - Comprehensive system diagnostics

## Technical Details

The list-workflows command:
- Connects to the n8n API at the configured endpoint
- Retrieves workflow metadata including status and update timestamps
- Supports both human-readable table and machine-readable JSON output
- Implements proper timeout handling (30 seconds)
- Provides detailed error messages for troubleshooting

For technical implementation details, see the [Developer Guide](../developer-guides/workflow-management.md).