# ADR-0007: Services and Infrastructure Separation

## Status
**Accepted**

## Context
After successfully implementing three commands using TDD, our service layer had grown to include both business logic and infrastructure concerns mixed together. This created several issues:

- **Mixed Responsibilities**: Services contained both "what to do" (business logic) and "how to do it" (infrastructure)
- **Testing Complexity**: Infrastructure concerns made unit testing more complex
- **Reusability**: Infrastructure code was duplicated across services
- **Maintainability**: Changes to infrastructure details required touching business logic

Example before refactoring:
```csharp
public class HealthChecker : IHealthChecker
{
    public async Task<HealthResult> CheckN8nHealthAsync(...)
    {
        // Business logic mixed with HTTP infrastructure
        var response = await _httpClient.GetAsync("http://localhost:5678/healthz", ...);
        if (response.IsSuccessStatusCode) { ... }
    }
}
```

## Decision
Implement a clear separation between Services (business logic) and Infrastructure (external dependencies) layers:

**Services Layer** (`/Services`):
- Contains business logic and domain rules
- Defines "what" the application does
- Depends on infrastructure abstractions (interfaces)
- Examples: `HealthChecker`, `ProcessManager`, `SystemChecker`

**Infrastructure Layer** (`/Infrastructure`):
- Contains implementation details for external dependencies
- Defines "how" operations are performed
- Implements infrastructure interfaces
- Organized by concern: `/Http`, `/Process`, `/FileSystem`

Architecture Pattern:
```
Commands -> Services -> Infrastructure -> External Systems
```

## Consequences

### Positive
- **Clear Separation of Concerns**: Business logic separated from implementation details
- **Better Testability**: Services can be tested with mocked infrastructure
- **Improved Reusability**: Infrastructure components can be shared across services
- **Easier Maintenance**: Changes to external systems only affect infrastructure layer
- **Enhanced Flexibility**: Can swap infrastructure implementations without changing business logic

### Refactoring Benefits Achieved
- **ProcessExecutor**: Centralized cross-platform process execution logic
- **N8nHttpClient**: Dedicated HTTP client for n8n communication
- **Simplified Services**: Services now focus purely on business logic
- **Better Error Handling**: Infrastructure layer handles technical errors, services handle business errors

### Implementation Details
```csharp
// Infrastructure Interface
public interface IProcessExecutor
{
    Task<bool> IsProcessRunningAsync(string processName, CancellationToken cancellationToken);
    Task<ProcessResult> ExecuteAsync(string fileName, string arguments, CancellationToken cancellationToken);
}

// Service Using Infrastructure
public class ProcessManager : IProcessManager
{
    public ProcessManager(IProcessExecutor processExecutor) { ... }
    
    public async Task<bool> IsN8nRunningAsync(CancellationToken cancellationToken)
    {
        return await _processExecutor.IsProcessRunningAsync("n8n", cancellationToken);
    }
}
```

### Negative
- **Additional Abstraction**: More interfaces and classes to maintain
- **Initial Refactoring Effort**: Required updating existing working code

## Rationale
This refactoring was motivated by the realization that our services were becoming complex and hard to test due to mixed concerns. The separation provides:

1. **Single Responsibility**: Each layer has one clear purpose
2. **Dependency Inversion**: Services depend on abstractions, not implementations
3. **Testability**: Infrastructure can be easily mocked for unit tests
4. **Maintainability**: Changes to external systems are isolated

Evidence of success:
- ProcessManager simplified from 150+ lines to 30 lines
- HealthChecker now focuses purely on health logic
- SystemChecker delegates all process execution to infrastructure
- Clear boundaries make the codebase easier to understand

## Related Decisions
- ADR-0002: TDD Approach (enabled safe refactoring with test coverage)
- ADR-0004-0006: Command Development (revealed the need for this separation)

## References
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Dependency Inversion Principle](https://en.wikipedia.org/wiki/Dependency_inversion_principle)
- [Separation of Concerns](https://en.wikipedia.org/wiki/Separation_of_concerns)