# ADR-0008: JSON Output Option for CLI Commands

## Status
**Accepted**

## Context
The FlowForge .NET CLI commands initially provided only human-readable table output for data display. However, users need machine-readable output for automation, scripting, and integration purposes. The `list-workflows` command was the first to require this functionality, but the approach needs to be consistent across all future commands that output structured data.

Key requirements:
- Machine-readable output for automation and scripting
- Consistent JSON format across all commands
- Maintain backward compatibility with table format
- Clear error handling in JSON responses
- Proper structure for extensibility

## Decision
We implemented a `--json` flag option for commands that output structured data, starting with the `list-workflows` command. The JSON output follows a consistent envelope format:

```json
{
  "success": true/false,
  "totalCount": number,
  "workflows": [...],
  "error": "error message" // only present when success: false
}
```

Key design decisions:
1. **Envelope Pattern**: All JSON responses use a consistent envelope with `success`, error handling, and metadata
2. **Flag-based**: Using `--json` flag rather than separate commands
3. **Backward Compatibility**: Table format remains the default
4. **Error Consistency**: Both success and error cases return valid JSON when flag is used
5. **Extensible Structure**: Envelope allows for future metadata additions

## Consequences

### Positive
- Enables automation and scripting integration
- Consistent API-like experience across CLI commands
- Clean separation between human and machine interfaces
- Easy to parse and process programmatically
- Future commands can follow the same pattern

### Negative
- Additional complexity in command implementation
- Need to maintain two output formats per command
- JSON parsing requirements for consuming scripts

### Neutral
- Code patterns established for future command implementations
- Testing requirements for both output formats

## Rationale
Alternative approaches considered:

1. **Separate Commands**: Creating `list-workflows-json` commands
   - **Rejected**: Would double the command surface area and create maintenance overhead

2. **Output Format Parameter**: Using `--format=json|table`
   - **Considered**: More extensible but overkill for current needs
   - **Decision**: Start with boolean flag, can extend to format parameter later

3. **Environment Variable**: Using `FLOWFORGE_OUTPUT=json`
   - **Rejected**: Less discoverable and explicit than command flags

4. **Raw JSON Output**: Direct API response without envelope
   - **Rejected**: Inconsistent error handling and less extensible

The `--json` flag approach provides the best balance of simplicity, discoverability, and functionality for the current use case while allowing future expansion.

## Related Decisions
- ADR-0003: Spectre.Console adoption (provides table formatting foundation)
- ADR-0007: Services infrastructure separation (enables clean output formatting separation)

## References
- [Spectre.Console CLI Documentation](https://spectreconsole.net/cli/)
- [JSON API Specification](https://jsonapi.org/) (inspiration for envelope pattern)
- [Command Line Interface Guidelines](https://clig.dev/) (CLI best practices)