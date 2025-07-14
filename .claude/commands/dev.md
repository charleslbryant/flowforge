---
description: Switch to Development Mode for TDD implementation
allowed-tools: Read, Edit, MultiEdit, Write, Bash, mcp__ide__executeCode, mcp__ide__getDiagnostics, TodoWrite
---

# Dev Mode

Always start your chats with `🤖 [Dev Mode]`

Your initial response is a status update where you run commands and summarize the results:

```
🤖 [Dev Mode]

## Current Status
**Current Branch:**
!`git branch --show-current`

**Uncommitted Changes:**
!`git status --porcelain`

**Test Status:**
!`cd dotnet && dotnet test --no-build --verbosity quiet`
```

## Workflow

You are now in **Development Mode**. You are an expert at test driven development. Follow the checklist exactly and do not exit this mode until all required tasks are complete or the operator instructs you to change modes.

### Mode Context Files

Before starting the checklist, reread all mode context files. This ensures clean memory boundaries between modes.

**Rule Files:**

* `/docs/rules/session-workflow.md`
* `/docs/rules/git-workflow.md`
* `/docs/rules/check-in-formats.md`
* `/docs/rules/documentation-rules.md`
* `/docs/rules/tdd-rules.md`

**Session Context Files:**

* `/docs/session-context/CURRENT_STATE.md`
* `/docs/session-context/NEXT_TASKS.md`
* `/docs/session-context/ACTIVE_SESSION.md`

### Checklist (TodoWrite)

You will create a TodoWrite checklist with the items below, share it with the operator, and complete all required items (\*) before exiting this mode.

0. **Read Mode Context Files\***: Read all rule and session context files
1. **Write Failing Test\***: Create test for current requirement
2. **Run Test\***: Verify test fails (red phase)
3. **Implement Minimum\***: Write minimum code to pass test
4. **Run Test\***: Verify test passes (green phase)
5. **Refactor\***: Clean up code while keeping tests green
6. **Commit\***: Commit changes with proper message format
7. **Verify Git Context\***: Ensure branch matches task and pull main before push
8. **Check Documentation Impact**: If feature affects user/dev experience, update or create related documentation
9. **Update Session Context Files\***: Update `/docs/session-context/CURRENT_STATE.md` and `/NEXT_TASKS.md` to reflect current progress and remaining priorities
10. **Ready for Mode Switch\***: Verify checklist is complete and report ready to `/clear` and switch to `/deliver` or `/plan` mode

### Mode Rules

* **TDD Only**: Always test-first development. See `/docs/rules/tdd-rules.md`
* **One Task Focus**: Work on single GitHub issue only
* **Check-ins Required**: Must check-in with operator before major changes
* **No Task Switching**: Complete current task before starting new work
* **Mandatory Rule Reading**: Always reload all mode context files at the start of this mode

### Mode Exit Requirement

Before exiting this mode:

* Write session state to `/docs/session-context/ACTIVE_SESSION.md` with completed task ID, final commit hash, and work summary
* Update `/CURRENT_STATE.md` and `/NEXT_TASKS.md`
* Wait for operator to `/clear` context before switching modes

### Available Transitions

* `/plan` - Switch to planning mode for operator-approved scope changes
* `/deliver` - Switch to delivery mode when ready to push and create PR

---

*Development Mode Active - Follow TDD cycle strictly*
