# Stop Command User Guide

The `stop` command gracefully stops the n8n workflow automation process.

## Usage

```bash
forge stop
```

## What It Does

The stop command will:

1. **Check Process Status**: Verify if n8n is currently running
2. **Graceful Shutdown**: Attempt to stop the n8n process cleanly
3. **Provide Feedback**: Show detailed status and suggested actions if needed

## Example Output

### Successful Stop
```
  ____    _                                ___          
 / ___|  | |_    ___    _ __      _ __    ( _ )   _ __  
 \___ \  | __|  / _ \  | '_ \    | '_ \   / _ \  | '_ \ 
  ___) | | |_  | (_) | | |_) |   | | | | | (_) | | | | |
 |____/   \__|  \___/  | .__/    |_| |_|  \___/  |_| |_|
                       |_|                              

🛑 Stopping n8n...

✅ n8n process stopped successfully
```

### Process Not Running
```
🛑 Stopping n8n...

✅ n8n process is not running
```

### Stop Failed with Suggestions
```
🛑 Stopping n8n...

❌ Failed to stop n8n process
Details: The process could not be terminated. This may be due to insufficient permissions.

💡 Suggested actions:
• Try running with elevated permissions (sudo on Linux/macOS)
• Use 'pkill -f n8n' or 'taskkill /F /IM n8n.exe' manually
• Check if the process is system-protected or locked
```

## Troubleshooting

### Permission Issues
If you receive permission-related errors:

**Linux/macOS:**
```bash
sudo forge stop
# or manually:
sudo pkill -f n8n
```

**Windows:**
```cmd
# Run as Administrator, then:
forge stop
# or manually:
taskkill /F /IM n8n.exe
```

### Process Won't Stop
If the process appears stuck:

1. **Check Process Status:**
   ```bash
   forge health
   ```

2. **Force Kill (Linux/macOS):**
   ```bash
   pkill -9 -f n8n
   ```

3. **Force Kill (Windows):**
   ```cmd
   taskkill /F /IM n8n.exe
   ```

4. **Check for Multiple Instances:**
   ```bash
   # Linux/macOS
   ps aux | grep n8n
   
   # Windows
   tasklist | findstr n8n
   ```

### Verify Stop was Successful
After stopping, verify n8n is no longer running:

```bash
forge health
```

Should show: `❌ n8n is not running`

## Related Commands

- [`forge start`](start-command.md) - Start n8n process
- [`forge restart`](restart-command.md) - Restart n8n process  
- [`forge health`](health-command.md) - Check n8n status
- [`forge doctor`](doctor-command.md) - Comprehensive system diagnostics

## Technical Details

The stop command uses the enhanced ProcessOperationResult system to provide:
- Detailed error messages
- Contextual suggested actions
- Clear success/failure indication
- Graceful handling of edge cases

For technical implementation details, see the [Developer Guide](../developer-guides/process-management.md).