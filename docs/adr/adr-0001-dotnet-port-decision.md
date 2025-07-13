# ADR-0001: .NET Port of FlowForge CLI

## Status
**Accepted**

## Context
FlowForge was originally implemented as a bash script collection providing workflow automation capabilities that integrate Claude Code with n8n workflow management. While the bash implementation works well on Unix systems, several forces drove the consideration of a .NET port:

- **Cross-platform compatibility**: Bash scripts have limited native Windows support
- **Type safety**: Bash lacks compile-time type checking leading to runtime errors
- **Testing challenges**: Bash testing requires complex mocking frameworks (like Bats)
- **Developer experience**: Limited IDE support, debugging capabilities, and tooling
- **Maintainability**: Large bash scripts become difficult to maintain and refactor
- **Error handling**: Bash error handling is verbose and error-prone
- **Package management**: No native dependency management for bash scripts

The existing bash implementation successfully demonstrates:
- n8n process management (start/stop/health)
- System diagnostics and dependency checking
- Workflow generation via Claude CLI integration
- Template-based workflow creation

## Decision
We will create a .NET 8 console application port of FlowForge that maintains feature parity with the bash implementation while providing enhanced developer experience, testability, and cross-platform compatibility.

Key architectural choices:
- .NET 8 for latest performance and cross-platform support
- Console application with rich CLI framework
- Dependency injection for testability and modularity
- Comprehensive test coverage using xUnit and Moq
- Maintain compatibility with existing n8n instances and templates

## Consequences

### Positive
- **Cross-platform native support**: Works identically on Windows, macOS, and Linux
- **Strong typing**: Compile-time error detection and IntelliSense support
- **Rich testing ecosystem**: Unit tests, integration tests, and mocking capabilities
- **Better error handling**: Structured exception handling with detailed error messages
- **Enhanced debugging**: Full debugger support with breakpoints and inspection
- **Package management**: NuGet ecosystem for dependencies
- **Performance**: Compiled binary with faster startup than script interpretation
- **IDE support**: Full Visual Studio/VS Code integration with refactoring tools

### Negative
- **Additional runtime dependency**: Requires .NET runtime installation
- **Learning curve**: Team members familiar with bash need .NET knowledge
- **Binary size**: Larger deployment footprint than shell scripts
- **Development time**: Initial port requires significant implementation effort

### Neutral
- **Two implementations**: Bash and .NET versions will coexist during transition
- **Template compatibility**: Both versions use same n8n workflow templates
- **Configuration compatibility**: Environment variables and settings remain consistent

## Rationale
The decision to port to .NET was driven by the need for better developer experience and maintainability while preserving all existing functionality. .NET 8 provides:

1. **Modern CLI tooling**: Rich console frameworks like Spectre.Console
2. **Proven cross-platform support**: Mature runtime across all target platforms
3. **Enterprise-grade tooling**: Comprehensive testing, debugging, and deployment tools
4. **Long-term maintainability**: Strong typing and refactoring support reduce technical debt

Alternatives considered:
- **Node.js/TypeScript**: Would provide good tooling but adds another runtime dependency
- **Go**: Excellent for CLI tools but team has more .NET experience
- **Python**: Good cross-platform support but performance and packaging concerns
- **Rust**: Excellent performance but steep learning curve

## Related Decisions
- ADR-0002: Test-Driven Development Approach
- ADR-0003: Spectre.Console for Rich Console Output

## References
- [.NET 8 Console Application Documentation](https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code)
- [FlowForge Bash Implementation](../../scripts/forge)
- [.NET Cross-Platform Guide](https://docs.microsoft.com/en-us/dotnet/core/introduction)