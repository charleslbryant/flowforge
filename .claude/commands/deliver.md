---
description: Switch to Delivery Mode for completing and pushing work
allowed-tools: Read, Write, TodoWrite, Bash(git:*), Bash(gh:*), Bash(dotnet test:*)
---

---

description: Switch to Delivery Mode for completing and pushing work
allowed-tools: Read, Write, TodoWrite, Bash(git:*), Bash(gh:*), Bash(dotnet test:\*)
------------------------------------------------------------------------------------

# Deliver Mode

Always start your chats with `🤖 [Delivery Mode]`

Your initial response is a status update where you run commands and summarize the results:

```
🤖 [Delivery Mode]

## Current Status
**Git Status:**
!`git status`

**Current Branch:**
!`git branch --show-current`

**Test Results:**
!`cd dotnet && dotnet test --verbosity minimal`

**Open PRs:**
!`gh pr list --head $(git branch --show-current)`
```

## Workflow

You are now in **Delivery Mode**. You are an expert at shipping work through GitHub. Follow the checklist exactly and do not exit this mode until all required tasks are complete or the operator instructs you to change modes.

### Mode Context Files

Before starting the checklist, reread all mode context files. This ensures clean memory boundaries between modes.

**Rule Files:**

* `/docs/rules/git-workflow.md`
* `/docs/rules/documentation-rules.md`
* `/docs/rules/check-in-formats.md`

**Session Context Files:**

* `/docs/product/`
* `/docs/architecture.md`
* `/docs/session-context/CURRENT_STATE.md`
* `/docs/session-context/NEXT_TASKS.md`
* `/docs/session-context/ACTIVE_SESSION.md`

### Checklist (TodoWrite)

You will create a TodoWrite checklist with the items below, share it with the operator, and complete all required items (\*) before exiting this mode.

0. **Read Mode Context Files\***: Read all rule and session context files
1. **Final Testing\***: Ensure the solution builds and all tests pass
2. **Documentation Update\***: Write or update user/developer guides and create ADRs to journal decisions
3. **Git Workflow\***: Pull main, merge to feature branch, and commit per rules
4. **Push Branch\***: Push branch to remote origin
5. **Create Pull Request\***: Create PR with proper template and test plan
6. **Await Review\***: Wait for operator to approve or reject PR
7. **Decision Outcome\***:
   * If approved, continue to post-merge cleanup
   * If rejected, continue to update session contet files and switch to `/dev` mode to fix issues
8. **Post-Merge Cleanup\*** (CRITICAL - After PR merge):
   * Switch to main branch: `git checkout main`
   * Pull latest main: `git pull origin main` 
   * Delete feature branch: `git branch -d feature/branch-name`
9. **Update Session Context Files\*** (On main branch):
   * Update `/CURRENT_STATE.md` to reflect completed work
   * Update `/NEXT_TASKS.md` with remaining tasks
   * Update `/ACTIVE_SESSION.md` with delivery summary
   * Commit context updates to main and push
10. **Ready for Mode Switch\***: Verify checklist is complete and report ready to `/clear` and switch to `/qa`, `/dev`, or `/begin` for next session

### Mode Rules
* **No New Work**: Only completion and delivery activities
* **Git Rules Mandatory**: Follow `/docs/rules/git-workflow.md`
* **Documentation Required**: Ensure all impacted documentation is up to date
* **Clean Exit**: Leave the project in a clean and up-to-date state

### Mode Exit Requirement

Before exiting this mode:

* Update `/docs/session-context/ACTIVE_SESSION.md` with delivery summary, PR link, and any remaining tasks
* Update `/CURRENT_STATE.md` and `/NEXT_TASKS.md`
* Wait for operator to `/clear` context before switching modes

### Available Transitions

* `/begin` - Start a new session after delivery is complete and context is cleared
* `/qa` - Switch to QA mode to test the delivery
* `/dev` - Return to development mode if PR is rejected or requires additional work

---

*Delivery Mode Active - Complete handoff and ship the work*
