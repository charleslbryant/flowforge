# Nullability Guidelines

## Overview
This document outlines our approach to handling null values in the WorkflowAgent codebase. These guidelines aim to reduce null-reference exceptions and make null handling more explicit and consistent across the codebase.

## General Principles

1. **Favor Non-Nullable Types**
   - Make reference types non-nullable by default
   - Only use nullable types when a value can genuinely be absent
   - Example:
     ```csharp
     // Prefer this:
     public string Name { get; set; }
     
     // Over this:
     public string? Name { get; set; }
     ```

2. **Empty vs Null for Strings**
   - Use `string.Empty` instead of `null` for optional string parameters
   - This simplifies string operations and reduces null checks
   - Example:
     ```csharp
     // In EmailResponseAgent:
     var templateId = GetOptionalInput<string>(context.Input, "templateId", string.Empty);
     var customInstructions = GetOptionalInput<string>(context.Input, "customInstructions", string.Empty);
     ```

3. **Null Checking**
   - Use `ArgumentNullException.ThrowIfNull()` for method parameters
   - Place null checks at the beginning of methods
   - Example:
     ```csharp
     public async Task ExecuteInternalAsync(AgentExecutionContext context)
     {
         ArgumentNullException.ThrowIfNull(context);
         // Rest of the method...
     }
     ```

4. **Optional Parameters**
   - Use the `GetOptionalInput<T>` method for optional inputs
   - Always provide a meaningful default value
   - Document the default behavior
   - Example:
     ```csharp
     var priority = GetOptionalInput<string>(context.Input, "priority", "Normal");
     ```

5. **Collections**
   - Return empty collections instead of null
   - Use `IReadOnlyCollection<T>` for read-only collection returns
   - Example:
     ```csharp
     // In BaseAgent:
     public IReadOnlyCollection<string> GetCapabilities()
     {
         return _capabilities.SupportedOperations;
     }
     ```

## Common Patterns

### 1. Agent Configuration
- Required configuration values should be non-nullable
- Optional configuration should use nullable types with clear documentation
```csharp
public class EmailAgentConfig
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public string? CustomHeader { get; set; }
}
```

### 2. Message Handling
- Core message properties (Subject, Body, etc.) should be non-nullable
- Optional properties should be nullable with documentation
```csharp
public class EmailMessage
{
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public string? Category { get; set; }
}
```

### 3. Service Methods
- Clearly document if a method can return null
- Use nullable return types when appropriate
```csharp
// Returns null if email not found
public async Task<EmailMessage?> GetEmailAsync(string id);

// Never returns null, throws if email not found
public async Task<EmailMessage> GetRequiredEmailAsync(string id);
```

## Testing
- Test both null and non-null scenarios
- Include explicit tests for null handling
- Use meaningful test names that indicate null scenarios
```csharp
[Fact]
public async Task GetEmailAsync_WithNonexistentId_ReturnsNull()
{
    var result = await _emailService.GetEmailAsync("nonexistent");
    Assert.Null(result);
}
```

## Code Review Checklist
When reviewing code for nullability:

1. [ ] Are reference types appropriately marked as nullable/non-nullable?
2. [ ] Are null checks present for non-nullable parameters?
3. [ ] Are string defaults using `string.Empty` instead of `null`?
4. [ ] Are collections returned as empty instead of null?
5. [ ] Is null handling documented where necessary?
6. [ ] Are there tests covering null scenarios?

## Migration Guidelines
When updating existing code:

1. Enable nullable reference types in the project
2. Address compiler warnings systematically
3. Update tests to cover new null handling
4. Document any breaking changes
5. Consider backward compatibility needs

## Tools and Resources
- Use the C# compiler's nullable reference type features
- Enable nullable warnings in .editorconfig
- Use static analysis tools to catch potential null reference issues

## Examples from Our Codebase

### BaseAgent
```csharp
public abstract class BaseAgent
{
    protected TValue GetRequiredInput<TValue>(IDictionary<string, object> input, string key)
    {
        ArgumentNullException.ThrowIfNull(input);
        
        if (!input.TryGetValue(key, out var value))
        {
            throw new ArgumentException($"Required input '{key}' not found");
        }
        
        return (TValue)value;
    }
}
```

### EmailResponseAgent
```csharp
protected override async Task ExecuteInternalAsync(AgentExecutionContext context)
{
    ArgumentNullException.ThrowIfNull(context);

    var templateId = GetOptionalInput<string>(context.Input, "templateId", string.Empty);
    var customInstructions = GetOptionalInput<string>(context.Input, "customInstructions", string.Empty);
    
    // Use string.IsNullOrEmpty for optional string checks
    var template = !string.IsNullOrEmpty(templateId)
        ? await _templateService.GetTemplateAsync(templateId)
        : await _templateService.GetBestTemplateAsync(category, sentiment);
}
```
