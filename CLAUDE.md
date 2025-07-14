# CLAUDE.md

This file provides high-level guidance to AI assistants working on this project. Detailed workflows and rules are in separate files for better compliance and maintainability.

## AI Assistant Identity
AI assistants working on this project identify as "George" in all commits and contributions.

## Mode-Based Workflow System (RECOMMENDED)
**NEW:** Use the mode-based workflow system for better process compliance and focused development:

### Available Modes:
- **`/begin`** - Session Startup Mode (initialization and context loading)
- **`/plan`** - Task Planning Mode (breaking down CRDs into GitHub issues)
- **`/dev`** - Core Development Mode (TDD implementation with tool restrictions)  
- **`/deliver`** - Delivery Mode (git workflow, PRs, session completion)

### Quick Start:
1. **Start every session with `/begin`** - loads focused startup context and TodoWrite checklist
2. **Switch modes appropriately** - each mode restricts tools and provides focused workflow
3. **Complete mode checklists** - TodoWrite automatically tracks progress and prevents rule violations
4. **See full documentation**: `/docs/modes/README.md` and ADR-0008

### Mode Benefits:
✅ **Focused Context** - Only relevant rules loaded per workflow phase  
✅ **Process Compliance** - Tool restrictions prevent common violations  
✅ **TodoWrite Integration** - Automatic checklists ensure no skipped steps  
✅ **Clear Transitions** - Explicit handoffs between workflow phases  

## Rule 0: Read the Rules (FALLBACK - Use Modes Instead)
**If not using modes:** Read ALL rule files in `/docs/rules/` to understand the complete process:
- `/docs/rules/session-workflow.md` - Development workflow and TDD approach
- `/docs/rules/git-workflow.md` - Branch management and commit standards  
- `/docs/rules/documentation-rules.md` - When and how to document features
- `/docs/rules/check-in-formats.md` - Required check-ins with operator
- `/docs/rules/task-management.md` - GitHub issue management and scope control
- `/docs/rules/todowrite-templates.md` - Workflow checklists and compliance

## Session Startup Checklist (MANUAL - Use `/begin` Mode Instead)
1. **FIRST:** Read ALL rule files listed in Rule 0 above
2. Read project context files in `/docs/product/` and `/docs/architecture.md`
3. Review current state in `/docs/session-context/CURRENT_STATE.md`
4. Check task queue in `/docs/session-context/NEXT_TASKS.md`
5. Select ONE "Now" priority task and assign to self
6. Create feature branch following git-workflow.md rules
7. Follow workflow rules below

## Core Workflow Rules
- **Session Workflow**: Follow `/docs/rules/session-workflow.md`
- **Git Operations**: Follow `/docs/rules/git-workflow.md` (CRITICAL - always pull before push)
- **Documentation**: Follow `/docs/rules/documentation-rules.md`
- **Check-ins**: Use formats in `/docs/rules/check-in-formats.md` (MANDATORY between tasks)
- **Task Management**: Follow `/docs/rules/task-management.md`

## Development Focus
- Use Test-Driven Development (TDD) for .NET implementation
- Work on ONE task GitHub issue at a time
- Create feature branch to work on one CRD GitHub issue at a time
- Provide regular check-ins to operator
- End sessions with `/clear context` instruction

## Key Commands Reference
```bash
# Mode system commands
/begin                                  # Start session startup mode
/plan                                   # Switch to planning mode
/dev                                    # Switch to development mode
/deliver                                # Switch to delivery mode

# Quick development commands  
cd dotnet && dotnet test --watch        # TDD development
./scripts/forge health                  # System health check
gh issue list --label "now"           # Current work queue
```

## Documentation Links
- **Mode System**: `/docs/modes/README.md` - Complete mode system guide
- **Architecture**: `/docs/architecture.md` - Technical overview and dual-implementation approach
- **Process Rules**: `/docs/rules/` - Detailed workflow procedures
- **ADR-0008**: `/docs/adr/adr-0008-mode-based-workflow-system.md` - Mode system decision
- **Product Context**: `/docs/product/` - Product vision and strategy
- **Session Context**: `/docs/session-context/` - Current state and task queue

## Emergency Procedures
- **Git issues**: See `/docs/rules/git-workflow.md#troubleshooting`
- **Build failures**: Run health check, review test output
- **Process conflicts**: Use `/begin` mode for proper startup
- **Mode confusion**: See `/docs/modes/README.md` for mode transitions

---
*This file focuses on high-level guidance. Use `/begin` mode for process-compliant development.*