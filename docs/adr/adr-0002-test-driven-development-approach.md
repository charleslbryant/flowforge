# ADR-0002: Test-Driven Development Approach

## Status
**Accepted**

## Context
The original FlowForge bash implementation had limited test coverage with a Bats-based testing framework that required complex mocking. As we port to .NET, we need to establish a robust testing strategy that ensures:

- High confidence in code correctness
- Easy refactoring and maintenance
- Clear documentation of expected behavior
- Prevention of regressions
- Rapid feedback during development

The bash version challenges:
- Complex mocking of external processes (pgrep, pkill, curl)
- Difficult integration testing with real n8n instances
- Manual testing required for cross-platform compatibility
- Limited test isolation and setup/teardown capabilities

## Decision
We will adopt a strict Test-Driven Development (TDD) approach for the .NET FlowForge port, following the Red-Green-Refactor cycle for all new functionality.

TDD Implementation:
1. **Red**: Write failing tests that define desired behavior
2. **Green**: Write minimal code to make tests pass
3. **Refactor**: Improve code structure while maintaining test coverage

Testing stack:
- **xUnit**: Primary testing framework for .NET
- **Moq**: Mocking framework for dependency isolation
- **TestContainers**: Integration testing with real n8n containers
- **FluentAssertions**: Readable test assertions (if needed)

## Consequences

### Positive
- **High test coverage**: Every feature backed by comprehensive tests
- **Regression prevention**: Changes immediately surface breaking behavior
- **Living documentation**: Tests serve as executable specifications
- **Design improvement**: TDD drives better API design and dependency injection
- **Refactoring confidence**: Safe to restructure code with test safety net
- **Rapid feedback**: Quick validation of changes during development
- **Cross-platform validation**: Tests verify behavior across platforms

### Negative
- **Initial development overhead**: Writing tests first requires more upfront time
- **Learning curve**: Team members need TDD discipline and practices
- **Test maintenance**: Tests require ongoing maintenance as features evolve

### Neutral
- **Different development rhythm**: TDD changes the flow of feature development
- **Mock vs integration balance**: Need to balance fast unit tests with real integration tests

## Rationale
TDD was chosen because the original bash implementation suffered from limited testability, making changes risky and time-consuming to validate. The .NET ecosystem provides excellent testing tools that make TDD practical and effective.

Evidence from initial implementation:
- **Health Command**: TDD cycle revealed clear service boundaries (IHealthChecker)
- **Doctor Command**: Tests drove separation of system checking from presentation
- **Start Command**: TDD identified process management abstractions (IProcessManager)

Each TDD cycle improved the architectural design by forcing consideration of dependencies, interfaces, and error conditions upfront.

Alternative approaches considered:
- **Test-after development**: Would have preserved existing bash testing challenges
- **Integration-only testing**: Too slow for rapid feedback cycles
- **Manual testing**: Not scalable for cross-platform and regression testing

## Related Decisions
- ADR-0001: .NET Port Decision (enabled robust testing ecosystem)
- ADR-0003: Spectre.Console adoption (testable UI components)

## References
- [TDD Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [TestContainers .NET](https://testcontainers.org/)