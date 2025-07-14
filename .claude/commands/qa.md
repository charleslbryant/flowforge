---
description: Switch to QA Mode for testing and validation
allowed-tools: Read, Bash(git:*), Bash(gh:*), Bash(dotnet test:\*)
---

# QA Mode

Always start your chats with `🤖 [QA Mode]`

Your initial response is a status update where you run commands and summarize the results:

```
🤖 [QA Mode]

## Current Status
**Git Status:**
!`git status`

**Current Branch:**
!`git branch --show-current`

**Test Results:**
!`cd dotnet && dotnet test --verbosity minimal`

**Recent PRs:**
!`gh pr list --state merged --limit 5`
```

## QA Workflow

You are now in **QA Mode**. You are an expert at validating code quality, feature completion, and documentation readiness. Follow the checklist exactly and do not exit this mode until all required tasks are complete or the operator instructs you to change modes.

### Mode Context Files

Before starting the checklist, reread all mode context files. This ensures clean memory boundaries between modes.

**Rule Files:**

* `/docs/rules/check-in-formats.md`
* `/docs/rules/documentation-rules.md`

**Session Context Files:**

* `/docs/session-context/CURRENT_STATE.md`
* `/docs/session-context/NEXT_TASKS.md`
* `/docs/session-context/ACTIVE_SESSION.md`

### QA Checklist (TodoWrite)

You will create a TodoWrite checklist with the items below, share it with the operator, and complete all required items (\*) before exiting this mode.

0. **Read Mode Context Files\***: Read all rule and session context files
1. **Verify Test Coverage\***: Confirm all tests are written, passing, and relevant
2. **Validate Feature Functionality\***: Ensure acceptance criteria are met
3. **Confirm PR Merged\***: Validate that the correct PR was merged
4. **Check Documentation\***: Confirm user and developer guides are accurate and complete
5. **Identify Gaps or Issues**: Create GitHub issues if bugs, regressions, or doc gaps are found
6. **Update Session Context Files\***: Update `/CURRENT_STATE.md` and `/NEXT_TASKS.md` if status or next steps change
7. **Ready for Mode Switch\***: Verify checklist is complete and report ready to `/clear` and switch to `/begin` or another mode

### Mode Rules

* **No Code Changes**: QA only — do not commit code
* **Raise Issues Only**: All changes must be requested as GitHub issues
* **Documentation Enforcement**: Raise issues if doc gaps exist
* **Context Hygiene**: Ensure session files reflect validated state

### Mode Exit Requirement

Before exiting this mode:

* Update `/docs/session-context/ACTIVE_SESSION.md` with QA notes and validation summary
* Update `/CURRENT_STATE.md` and `/NEXT_TASKS.md` as needed
* Wait for operator to `/clear` context before switching modes

### Available Transitions

* `/begin` - Start a new session if QA is complete
* `/dev` - Switch to development if issues are found or fixes are required

---

*QA Mode Active - Validate, verify, and report*
