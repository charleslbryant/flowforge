# ADR-0005: Doctor Command Development Journey

## Status
**Accepted**

## Context
Building on the successful health command TDD cycle, the doctor command represented a more complex challenge requiring system-level diagnostics. The bash version performed comprehensive system checking:

```bash
run_doctor() {
  # Check required commands: node, npm, jq, curl, nc
  # Check n8n installation  
  # Check Claude Code CLI
  # Check n8n health
  # Provide final verdict and guidance
}
```

This command needed to:
- Expand beyond HTTP checking to process/command validation
- Handle cross-platform differences in command detection
- Provide actionable guidance for missing dependencies
- Integrate multiple service dependencies
- Present complex information clearly

## Decision
Implement doctor command as the second TDD cycle, introducing:

1. **System Checking Service**: New `ISystemChecker` for dependency validation
2. **Cross-Platform Process Detection**: Handle Windows vs Unix command checking
3. **Rich Data Structures**: `SystemCheckResult` with detailed check information
4. **Complex Service Composition**: Combine system and health checking
5. **Enhanced Output Formatting**: Structured presentation of multiple check results

Architecture designed:
```csharp
DoctorCommand -> ISystemChecker + IHealthChecker -> Process execution + HTTP client
```

## Consequences

### Positive
- **Service Composition**: Successfully demonstrated multiple service dependencies
- **Cross-Platform Foundation**: Established patterns for Windows/Unix differences
- **Rich Data Modeling**: `SystemCheck` and `SystemCheckResult` provide structured information
- **Comprehensive Testing**: Complex scenarios with multiple mock setups
- **Beautiful Diagnostics**: Color-coded output with clear success/failure indication
- **Actionable Guidance**: Specific install commands for missing dependencies

### Challenges Overcome
- **Complex Test Setup**: Multiple mock services required careful coordination
- **Process Execution**: Learned `which`/`--version` patterns for command detection
- **Error Aggregation**: Collected and presented multiple failure scenarios
- **Version Extraction**: Simple but effective version string parsing

### Technical Innovations
- **Version Detection**: Multi-step process checking (existence → version)
- **Install Commands**: Contextual guidance for different dependency types
- **Cross-Platform Abstraction**: Process execution patterns that work universally
- **Error Collection**: Accumulating issues for comprehensive reporting

## Implementation Details

**TDD Cycle:**
1. **Red**: Tests for healthy system, missing dependencies, n8n unhealthy scenarios
2. **Green**: Minimal logic to satisfy all test conditions
3. **Refactor**: Enhanced with rich Spectre.Console output and detailed diagnostics

**Key Components:**
- `DoctorCommand`: Orchestrates system and health checks with rich presentation
- `ISystemChecker`: System dependency validation interface
- `SystemChecker`: Process-based implementation using `which`, `--version`
- `SystemCheckResult`: Structured result with individual check details
- Integration with existing `IHealthChecker`

**Cross-Platform Handling:**
```csharp
// Tool checking
var process = new Process {
    StartInfo = new ProcessStartInfo {
        FileName = tool,
        Arguments = "--version",
        // ... standard process configuration
    }
};
```

**Test Scenarios:**
- All systems healthy → return 0
- Missing dependencies → return 1  
- n8n unhealthy despite good dependencies → return 1
- Mixed scenarios with partial failures

## Rationale
The doctor command was the logical second choice because:
- **Builds on Health**: Reused established health checking patterns
- **Adds Complexity**: Introduced multi-service composition patterns
- **High User Value**: Comprehensive diagnostics essential for troubleshooting
- **Platform Foundation**: Established cross-platform process execution patterns

Success demonstrated:
- ✅ Complex TDD cycle with multiple dependencies
- ✅ Cross-platform process execution working
- ✅ Beautiful structured output with colors and guidance
- ✅ Service composition patterns established
- ✅ Real-world validation showing actual system state

**Visual Impact Achieved:**
```
  ____                  _                        
 / ___|   _   _   ___  | |_    ___   _ __ ___    
 \___ \  | | | | / __| | __|  / _ \ | '_ ` _ \   
  ___) | | |_| | \__ \ | |_  |  __/ | | | | | |  
 |____/   \__, | |___/  \__|  \___| |_| |_| |_|  

🩺 FlowForge System Check
========================

📋 Dependencies Check
✅ node (v22.17.0)
✅ npm (10.9.2)
✅ jq (jq-1.7)
✅ curl (curl 8.5.0...)
✅ nc
✅ n8n (1.101.2)
❌ claude - Check failed: No such file or directory
   Install with: npm install -g @anthropic-ai/claude-code

🌐 n8n Health Check
✅ n8n is running and responding

❌ System not ready. Please fix the issues above.
💡 Try running 'forge-dotnet health' after fixing dependencies
```

## Related Decisions
- ADR-0004: Health Command Development (built upon health checking patterns)
- ADR-0006: Start Command Development (will build on process management patterns)
- ADR-0002: TDD Approach (successfully applied to complex scenario)

## References
- [Doctor Command Implementation](../../dotnet/src/FlowForge.Console/Commands/DoctorCommand.cs)
- [System Checker Service](../../dotnet/src/FlowForge.Console/Services/SystemChecker.cs)
- [Doctor Command Tests](../../dotnet/tests/FlowForge.Console.Tests/Commands/DoctorCommandTests.cs)