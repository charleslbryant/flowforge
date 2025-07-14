# ADR-0008: Mode-Based Workflow System

## Status
**Accepted**

## Context
FlowForge development was experiencing significant process compliance failures due to context pollution and cognitive overload. AI assistants working on the project were:

- Violating "one task at a time" rules by completing multiple tasks sequentially
- Skipping mandatory check-ins between workflow phases
- Missing critical git workflow steps (pull before push, proper attribution)
- Struggling to follow TDD approach consistently
- Loading all process rules simultaneously, leading to confusion and rule violations

The existing rule system in `/docs/rules/` contained comprehensive procedures, but assistants were either not reading them or getting overwhelmed by trying to follow all rules simultaneously across different workflow phases.

## Decision
Implement a mode-based workflow system using Claude Code custom slash commands to provide focused, context-aware development workflows:

### Four Primary Modes:
1. **`/begin` (Session Startup Mode)**: Initialize sessions with proper context loading
2. **`/work` (Core Development Mode)**: TDD implementation with tool restrictions
3. **`/plan` (Task Planning Mode)**: Break down CRDs and manage GitHub issues
4. **`/deliver` (Delivery Mode)**: Complete work following git workflow and create PRs

### Mode Characteristics:
- **Tool Restrictions**: Each mode allows only relevant tools for that phase
- **Focused Context**: Mode-specific rules and TodoWrite checklists
- **Explicit Transitions**: Clear handoffs between workflow phases
- **Process Enforcement**: Automatic checklist generation prevents rule skipping

## Consequences

### Positive:
- **Improved Process Compliance**: Mode restrictions prevent common rule violations
- **Reduced Cognitive Load**: Only relevant context loaded per mode
- **Better Quality**: TDD and git workflow enforcement through tool restrictions
- **Clear Workflow Structure**: Explicit phases with defined entry/exit criteria
- **Automatic Documentation**: TodoWrite checklists provide real-time process tracking

### Negative:
- **Additional Complexity**: New system to learn and maintain
- **Mode Discovery**: Custom commands don't appear in `/help` command list
- **Transition Overhead**: Explicit mode switching required
- **Tool Limitation Risk**: May need tools not allowed in current mode

### Neutral:
- **File Structure Changes**: New `.claude/commands/` directory and `/docs/modes/`
- **Documentation Requirements**: Mode system needs ongoing maintenance

## Rationale
The mode-based approach directly addresses the root cause of process failures: **context pollution**. By restricting tools and context to specific workflow phases, we eliminate the cognitive burden of managing all rules simultaneously.

### Alternatives Considered:
1. **Enhanced Rule Documentation**: More detailed procedures - rejected due to continued context pollution
2. **Checklist-Only Approach**: Manual TodoWrite checklists - rejected due to lack of tool enforcement
3. **Git Hooks**: Automatic enforcement via git - rejected due to limited scope and flexibility
4. **Separate AI Agents**: Different agents per workflow phase - rejected due to context loss between phases

### Trade-offs Evaluated:
- **Flexibility vs. Compliance**: Chose compliance through restrictions over unlimited flexibility
- **Learning Curve vs. Quality**: Accepted initial complexity for long-term process adherence
- **Tool Freedom vs. Focus**: Chose focused tool sets over unrestricted access

## Related Decisions
- ADR-0002: Test-Driven Development Approach (supports `/work` mode TDD enforcement)
- ADR-0007: Services Infrastructure Separation (informs development workflow structure)
- Future: May need ADR for automatic mode transition detection

## References
- Claude Code Custom Commands: https://docs.anthropic.com/en/docs/claude-code/slash-commands
- FlowForge Process Rules: `/docs/rules/`
- TodoWrite Templates: `/docs/rules/todowrite-templates.md`
- Mode Documentation: `/docs/modes/README.md`

---
*ADR-0008 establishes the foundation for process-compliant AI-assisted development through focused workflow modes.*