# CLAUDE.md

This file provides high-level guidance to AI assistants working on FlowForge. Detailed workflows and rules are in separate files for better compliance and maintainability.

## AI Assistant Identity
AI assistants working on this project identify as "George" in all commits and contributions.

## Project Overview
FlowForge is an AI-powered workflow automation CLI tool with two implementations:
- **Bash Implementation** (`/scripts/`) - Production-ready original
- **.NET Implementation** (`/dotnet/`) - Modern port in development

## Session Startup Checklist
1. Read project context files in `/docs/product/` and `/docs/architecture.md`
2. Review current state in `/docs/session-context/CURRENT_STATE.md`
3. Check task queue in `/docs/session-context/NEXT_TASKS.md`
4. Select ONE "Now" priority task and assign to self
5. Follow workflow rules below

## Core Workflow Rules
- **Session Workflow**: Follow `/docs/rules/session-workflow.md`
- **Git Operations**: Follow `/docs/rules/git-workflow.md` (CRITICAL - always pull before push)
- **Documentation**: Follow `/docs/rules/documentation-rules.md`
- **Check-ins**: Use formats in `/docs/rules/check-in-formats.md`
- **Task Management**: Follow `/docs/rules/task-management.md`

## Development Focus
- Use Test-Driven Development (TDD) for .NET implementation
- Work on ONE task GitHub issueat a time
- Create feature branche to work on one CRD GitHub issue at a time
- Provide regular check-ins to operator
- End sessions with `/clear context` instruction

## Key Commands Reference
```bash
# Quick development commands
cd dotnet && dotnet test --watch        # TDD development
./scripts/forge health                  # System health check
gh issue list --label "now"           # Current work queue
```

## Architecture Overview
See `/docs/architecture.md` for detailed technical overview and dual-implementation approach.

## Emergency Procedures
- Git issues: See `/docs/rules/git-workflow.md#troubleshooting`
- Build failures: Run health check, review test output
- Process conflicts: Follow check-in procedures for guidance

---
*This file focuses on high-level guidance. Detailed step-by-step procedures are in `/docs/rules/` for better compliance.*