# ADR-0004: Health Command Development Journey

## Status
**Accepted**

## Context
The health command was our first TDD implementation, serving as the foundation for establishing patterns, testing approaches, and architectural decisions. The bash version performed a simple n8n connectivity check:

```bash
check_n8n_health() {
  # Check if n8n process is running
  # Check if port is accessible  
  # Check if web interface responds
  # Check API authentication
}
```

We needed to replicate this functionality while establishing:
- TDD practices and tooling
- Service abstraction patterns
- Error handling approaches
- Rich console output patterns

## Decision
Implement the health command as the first TDD cycle, using it to establish:

1. **Service Layer Pattern**: Extract health checking logic into `IHealthChecker` service
2. **Dependency Injection**: Use DI container for testable command structure
3. **Rich Output**: Implement Spectre.Console formatting patterns
4. **HTTP Client Usage**: Proper HttpClient usage for n8n health endpoint
5. **Error Handling**: Structured error reporting with specific issues

Architecture established:
```csharp
HealthCommand -> IHealthChecker -> HttpClient -> n8n /healthz endpoint
```

## Consequences

### Positive
- **TDD Foundation**: Successful Red-Green-Refactor cycle established confidence
- **Service Pattern**: Clean separation between command (presentation) and service (logic)
- **Beautiful Output**: Figlet headers and colored status messages exceeded expectations
- **HTTP Best Practices**: Proper async HTTP client usage with timeout handling
- **Testability**: 100% test coverage with mocked dependencies
- **Real Validation**: Worked against actual running n8n instance

### Challenges Overcome
- **Framework Migration**: Switched from System.CommandLine to Spectre.Console.Cli mid-implementation
- **Test Structure**: Learned Spectre.Console.Cli testing patterns with CommandContext mocking
- **Async Patterns**: Established async/await patterns for HTTP operations

### Lessons Learned
- **TDD Drives Design**: Tests naturally led to IHealthChecker abstraction
- **Visual Impact**: Rich console output dramatically improves user experience
- **Mocking Strategy**: Simple mocking with Moq works well for HTTP dependencies
- **Service Boundaries**: Clear interfaces make testing and refactoring easier

## Implementation Details

**TDD Cycle:**
1. **Red**: Written failing tests for healthy/unhealthy scenarios
2. **Green**: Minimal implementation in test class, then moved to real command
3. **Refactor**: Enhanced with Spectre.Console formatting and better error reporting

**Key Components:**
- `HealthCommand`: Presentation layer with rich formatting
- `IHealthChecker`: Business logic interface
- `HealthChecker`: HTTP-based implementation checking `/healthz`
- `HealthResult`: Structured result with status and issues

**Test Coverage:**
- Healthy n8n scenarios (returns 0)
- Unhealthy n8n scenarios (returns 1)
- HTTP timeout and connection error handling
- Proper service method invocation

## Rationale
The health command was chosen as the first implementation because:
- **Simple scope**: Clear, bounded functionality  
- **Foundation setting**: Needed to establish patterns for other commands
- **High value**: Essential for debugging and validation
- **HTTP example**: Representative of external service integration patterns

Success metrics achieved:
- ✅ TDD cycle completed successfully
- ✅ Beautiful console output implemented
- ✅ Service pattern established
- ✅ 100% test coverage maintained
- ✅ Cross-platform functionality verified

## Related Decisions
- ADR-0002: TDD Approach (successfully applied)
- ADR-0003: Spectre.Console Adoption (beautiful output achieved)
- ADR-0005: Doctor Command Development (built on health patterns)

## References
- [Health Command Implementation](../../dotnet/src/FlowForge.Console/Commands/HealthCommand.cs)
- [Health Checker Service](../../dotnet/src/FlowForge.Console/Services/HealthChecker.cs)
- [Health Command Tests](../../dotnet/tests/FlowForge.Console.Tests/Commands/HealthCommandTests.cs)