# ADR-0006: Start Command Development Journey

## Status
**Accepted**

## Context
The start command represented our most complex TDD challenge yet, requiring process lifecycle management across platforms. The bash version handled n8n startup with background execution:

```bash
start)
  echo "🚀 Starting n8n..."
  if pgrep -f "n8n" > /dev/null; then
    echo "⚠️  n8n is already running"
    check_n8n_health
  else
    echo "Starting n8n in background..."
    nohup n8n > ~/n8n.log 2>&1 &
    echo "Waiting for n8n to start..."
    sleep 5
    if check_n8n_health; then
      echo "✅ n8n started successfully"
    fi
  fi
```

This required:
- Cross-platform process detection (`pgrep` vs `tasklist`)
- Background process spawning with output redirection
- Process lifecycle management and cleanup
- Integration with existing health checking
- User experience during startup delays

## Decision
Implement start command as the third TDD cycle, introducing:

1. **Process Management Service**: New `IProcessManager` for cross-platform process operations
2. **Background Process Handling**: Proper async process spawning with output capture
3. **Startup Progress Indication**: Spectre.Console progress bars for user feedback
4. **Cross-Platform Process Detection**: Windows (`tasklist`) vs Unix (`pgrep`) implementations
5. **Log File Management**: Structured logging to user's home directory

Architecture designed:
```csharp
StartCommand -> IProcessManager + IHealthChecker -> Process management + HTTP validation
```

## Consequences

### Positive
- **Process Management Foundation**: Established comprehensive process lifecycle patterns
- **Cross-Platform Excellence**: Robust Windows/Unix process handling
- **Beautiful User Experience**: Progress bars and rich feedback during startup
- **Background Process Mastery**: Proper async process spawning with output capture
- **Log Management**: Structured logging to predictable file locations
- **State Management**: Intelligent handling of already-running processes

### Challenges Overcome
- **Background Process Complexity**: Learning .NET Process class for background execution
- **Cross-Platform Process Detection**: Different command patterns (pgrep vs tasklist)
- **Output Redirection**: Capturing and redirecting process output to log files
- **Async Process Handling**: Proper async/await patterns for process operations
- **Test Complexity**: Mocking process operations and startup sequences

### Technical Innovations
- **ProcessStartResult**: Structured result type for process launch outcomes
- **Log File Redirection**: Real-time output capture to ~/n8n.log
- **Progress Bar Integration**: Spectre.Console progress indication during startup delay
- **Smart State Detection**: Checking existing processes before attempting start
- **Cross-Platform Abstraction**: Clean abstraction over platform-specific commands

## Implementation Details

**TDD Cycle:**
1. **Red**: Tests for already running, successful start, failed start, process start failure
2. **Green**: Minimal logic implementing all four test scenarios
3. **Refactor**: Enhanced with rich Spectre.Console progress bars and detailed messaging

**Key Components:**
- `StartCommand`: Rich presentation layer with progress indication
- `IProcessManager`: Process lifecycle management interface  
- `ProcessManager`: Cross-platform implementation with Windows/Unix handling
- `ProcessStartResult`: Structured process launch result
- Integration with `IHealthChecker` for startup validation

**Cross-Platform Process Management:**
```csharp
// Unix process detection
pgrep -f n8n

// Windows process detection  
tasklist /FI "IMAGENAME eq n8n.exe"

// Process startup with output redirection
var process = new Process {
    StartInfo = new ProcessStartInfo {
        FileName = "n8n",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false
    }
};
```

**Test Scenarios:**
- n8n already running → check health, return 0
- n8n not running, starts successfully → return 0
- n8n not running, health check fails after start → return 1
- Process start fails → return 1

## Rationale
The start command was chosen as the third implementation because:
- **Process Foundation**: Establishes patterns needed for stop/restart commands
- **Complex Integration**: Combines process management with health checking
- **High User Value**: Essential functionality for n8n workflow management
- **UX Opportunity**: Great place to showcase progress bars and rich feedback

Success demonstrated:
- ✅ Complex TDD cycle with process mocking
- ✅ Cross-platform process management working
- ✅ Beautiful progress bars and startup feedback
- ✅ Proper background process handling with logging
- ✅ Smart state detection for already-running processes

**Visual Experience Achieved:**
```
  ____    _                    _                ___          
 / ___|  | |_    __ _   _ __  | |_     _ __    ( _ )   _ __  
 \___ \  | __|  / _` | | '__| | __|   | '_ \   / _ \  | '_ \ 
  ___) | | |_  | (_| | | |    | |_    | | | | | (_) | | | | |
 |____/   \__|  \__,_| |_|     \__|   |_| |_|  \___/  |_| |_|

🚀 Starting n8n...

⚠️  n8n is already running

Checking n8n health...
✅ n8n is running and healthy
🌐 Access n8n at: http://localhost:5678
```

When starting new process:
```
🚀 Starting n8n...

Starting n8n in background...
✅ n8n process started (PID: 12345)

[████████████████████████████████████████] 100% Waiting for n8n to start...

Checking n8n health...
✅ n8n started successfully!
🌐 Access n8n at: http://localhost:5678

💡 Logs are written to: ~/n8n.log
```

## Related Decisions
- ADR-0005: Doctor Command Development (built on process execution patterns)
- ADR-0002: TDD Approach (applied to most complex scenario yet)
- ADR-0003: Spectre.Console (progress bars showcase rich UI capabilities)

## References
- [Start Command Implementation](../../dotnet/src/FlowForge.Console/Commands/StartCommand.cs)
- [Process Manager Service](../../dotnet/src/FlowForge.Console/Services/ProcessManager.cs)
- [Start Command Tests](../../dotnet/tests/FlowForge.Console.Tests/Commands/StartCommandTests.cs)