---
description: Switch to Session Startup Mode and execute startup workflow
allowed-tools: Read, TodoWrite, Bash(git status:*), Bash(git branch --show-current), Bash(gh issue list:*)
---

# Begin Mode

Always start your chats with `🤖 [Begin Mode]`

Your initial response is a status update where you run commands and summarize the results:

```
🤖 [Begin Mode]

## Current Status
**Git Status:**
!`git status --porcelain`

**Current Branch:**
!`git branch --show-current`

**Now Priority Issues:**
!`gh issue list --label "now" --limit 10`
```

## Session Startup Workflow

You are now in **Session Startup Mode**. You are an expert at preparing development context. Follow the checklist exactly and do not exit this mode until all required tasks are complete or the operator instructs you to change modes.

### Mode Context Files

Before starting the checklist, reread all mode context files. This ensures clean memory boundaries between modes.

**Rule Files:**

* `/docs/rules/session-workflow.md`
* `/docs/rules/task-management.md`
* `/docs/rules/git-workflow.md`
* `/docs/rules/check-in-formats.md`

**Session Context Files:**

* `/docs/product/`
* `/docs/architecture.md`
* `/docs/session-context/CURRENT_STATE.md`
* `/docs/session-context/NEXT_TASKS.md`

### Startup Checklist (TodoWrite)

You will create a TodoWrite checklist with the items below, share it with the operator, and complete at least the required items (\*) before exiting this mode.

0. **Read Mode Context Files\***: Read all rule and session context files
1. **Read Product Context\***: Read `/docs/product/` files
2. **Read Architecture\***: Read `/docs/architecture.md`
3. **Read Current State\***: Read `/docs/session-context/CURRENT_STATE.md`
4. **Read Task Queue\***: Read `/docs/session-context/NEXT_TASKS.md`
5. **Select ONE Task\***: Work with the operator to select a task from "now" priority or request a new task from the operator
6. **Create Feature Branch\***: If a branch hasn't been created, follow `/docs/rules/git-workflow.md`, and verify `main` is up to date before creating a new branch
7. **Update Session Context Files\***: Write to `/docs/session-context/ACTIVE_SESSION.md` with selected task ID and branch info
8. **Ready for Mode Switch\***: Verify checklist is complete and report ready to `/clear` and switch to `/dev` or `/plan` mode

### Mode Rules

* **Mandatory Rule Reading**: Read all mode context files before starting. Do not assume prior knowledge across modes or sessions.
* **Focus**: Session initialization only
* **No Implementation**: Do not write files in this mode
* **Read Everything**: Complete all context readings before task selection
* **One Task Selection**: Must select exactly ONE task before exiting mode
* **Ask for Help**: Ask operator if you’re unsure about anything

### Mode Exit Requirement

Before exiting this mode:

* Write session state to `/docs/session-context/ACTIVE_SESSION.md`
* Wait for operator to `/clear` context before switching modes

### Available Transitions

* `/dev` - Switch to development mode with selected task
* `/plan` - Switch to planning mode if task needs to be broken down

---

*Session Startup Mode Active - Complete checklist before proceeding*
