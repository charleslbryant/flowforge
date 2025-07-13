# ADR-0003: Spectre.Console for Rich Console Output

## Status
**Accepted**

## Context
The original FlowForge bash implementation used basic console output with emojis and simple text formatting. While functional, it lacked:

- Consistent visual hierarchy and branding
- Rich formatting capabilities (colors, tables, progress bars)
- Cross-platform color support
- Interactive elements and progress indication
- Professional appearance for enterprise usage

Initial .NET implementation started with System.CommandLine for argument parsing, but console output remained basic. Forces driving enhanced console experience:

- **User experience**: CLI tools benefit from clear, visually appealing output
- **Status communication**: Complex operations need progress indication
- **Information hierarchy**: Different types of information need visual distinction
- **Error communication**: Errors need to stand out clearly from normal output
- **Brand consistency**: Professional appearance builds user confidence

## Decision
Adopt Spectre.Console as the primary framework for rich console output, replacing basic Console.WriteLine calls with formatted, colored, and interactive output.

Implementation approach:
- **Figlet headers**: ASCII art headers for each command
- **Color coding**: Green for success, red for errors, yellow for warnings, blue for info
- **Progress bars**: For long-running operations like n8n startup
- **Tables**: For structured data display (system checks, diagnostics)
- **Markup syntax**: Rich text formatting with embedded color codes

Also switched from System.CommandLine to Spectre.Console.Cli for unified framework approach.

## Consequences

### Positive
- **Professional appearance**: Beautiful ASCII art headers and consistent branding
- **Better information hierarchy**: Colors and formatting make output easier to scan
- **Enhanced user experience**: Progress bars provide feedback during waits
- **Cross-platform consistency**: Unified appearance across Windows, macOS, and Linux
- **Developer productivity**: Rich markup syntax simplifies output formatting
- **Error clarity**: Red error messages with clear visual distinction
- **Interactive elements**: Potential for future interactive features

### Negative
- **Dependency addition**: Additional NuGet package increases application size
- **Learning curve**: Team needs to learn Spectre.Console markup and APIs
- **Terminal compatibility**: Some older terminals may not support all features

### Neutral
- **Framework migration**: Moved from System.CommandLine to Spectre.Console.Cli
- **Output verbosity**: Richer output uses more screen space

## Rationale
The decision was driven by the stark visual improvement demonstrated in early implementations:

**Before (basic console)**:
```
Health check not implemented yet
```

**After (Spectre.Console)**:
```
  _   _                  _   _     _       
 | | | |   ___    __ _  | | | |_  | |__    
 | |_| |  / _ \  / _` | | | | __| | '_ \   
 |  _  | |  __/ | (_| | | | | |_  | | | |  
 |_| |_|  \___|  \__,_| |_|  \__| |_| |_|  

✅ n8n is healthy
   n8n is running and responding
```

The visual impact was immediately apparent and significantly improved the user experience.

Alternatives considered:
- **System.CommandLine**: Provided argument parsing but limited output formatting
- **Basic Console.WriteLine**: Simple but lacks visual appeal and hierarchy
- **Custom formatting**: Would require significant development effort to match Spectre.Console features
- **Colorful.Console**: Alternative rich console library but less feature-complete

Evidence from implementation:
- Health command TDD cycle was enhanced by rich visual feedback
- Doctor command benefited from colored status indicators and structured output
- Start command used progress bars to improve perceived performance

## Related Decisions
- ADR-0001: .NET Port Decision (enabled rich console libraries)
- ADR-0002: TDD Approach (Spectre.Console components are testable)

## References
- [Spectre.Console Documentation](https://spectreconsole.net/)
- [Spectre.Console.Cli](https://spectreconsole.net/cli/)
- [Console Application Best Practices](https://docs.microsoft.com/en-us/dotnet/standard/commandline/)