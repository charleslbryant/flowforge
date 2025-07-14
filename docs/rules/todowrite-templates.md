# TodoWrite Templates for Compliance

## Session Startup Template
**Trigger**: Start of any development session
**Auto-generate these todos:**
```json
[
  {"id": "startup-1", "content": "Read /docs/product/ for product context", "status": "pending", "priority": "high"},
  {"id": "startup-2", "content": "Read /docs/architecture.md for technical overview", "status": "pending", "priority": "high"}, 
  {"id": "startup-3", "content": "Read /docs/session-context/CURRENT_STATE.md for progress", "status": "pending", "priority": "high"},
  {"id": "startup-4", "content": "Read /docs/session-context/NEXT_TASKS.md for work queue", "status": "pending", "priority": "high"},
  {"id": "startup-5", "content": "Select ONE task from 'Now' priority and assign to self", "status": "pending", "priority": "high"},
  {"id": "startup-6", "content": "Create feature branch following git-workflow.md rules", "status": "pending", "priority": "high"}
]
```

## Git Workflow Template  
**Trigger**: Before any git push operation
**Auto-generate these todos:**
```json
[
  {"id": "git-1", "content": "CRITICAL: Pull latest main before pushing (git checkout main && git pull origin main)", "status": "pending", "priority": "high"},
  {"id": "git-2", "content": "Merge main into feature branch (git checkout feature/branch && git merge main)", "status": "pending", "priority": "high"},
  {"id": "git-3", "content": "Use George attribution format in commit message", "status": "pending", "priority": "medium"},
  {"id": "git-4", "content": "Create PR with proper template after push", "status": "pending", "priority": "medium"}
]
```

## Check-in Template
**Trigger**: Before starting major work, before push, after completion
**Auto-generate these todos:**
```json
[
  {"id": "checkin-1", "content": "Provide check-in with options for operator before proceeding", "status": "pending", "priority": "high"},
  {"id": "checkin-2", "content": "Wait for operator response before continuing", "status": "pending", "priority": "high"},
  {"id": "checkin-3", "content": "Include recommendation but make clear operator decides", "status": "pending", "priority": "medium"}
]
```

## Development Work Template
**Trigger**: When working on .NET implementation
**Auto-generate these todos:**
```json
[
  {"id": "dev-1", "content": "Follow TDD approach: write failing test first", "status": "pending", "priority": "high"},
  {"id": "dev-2", "content": "Implement minimum code to pass test", "status": "pending", "priority": "high"},
  {"id": "dev-3", "content": "Refactor and improve while maintaining tests", "status": "pending", "priority": "medium"},
  {"id": "dev-4", "content": "Update documentation if feature affects user/developer workflows", "status": "pending", "priority": "medium"}
]
```

## Branch Management Template
**Trigger**: When creating or switching branches
**Auto-generate these todos:**
```json
[
  {"id": "branch-1", "content": "Verify current branch matches assigned task", "status": "pending", "priority": "high"},
  {"id": "branch-2", "content": "Check git status for uncommitted changes before branch operations", "status": "pending", "priority": "high"},
  {"id": "branch-3", "content": "Confirm task scope before making changes beyond original files", "status": "pending", "priority": "medium"},
  {"id": "branch-4", "content": "Commit all changes before closing any GitHub issues", "status": "pending", "priority": "high"}
]
```

## Session Completion Template
**Trigger**: End of development session
**Auto-generate these todos:**
```json
[
  {"id": "completion-1", "content": "Commit all changes following git-workflow.md", "status": "pending", "priority": "high"},
  {"id": "completion-2", "content": "Create Pull Request with comprehensive description", "status": "pending", "priority": "high"},
  {"id": "completion-3", "content": "Close completed GitHub issues with completion comments", "status": "pending", "priority": "medium"},
  {"id": "completion-4", "content": "Update context files for next session", "status": "pending", "priority": "medium"},
  {"id": "completion-5", "content": "Instruct operator to /clear context", "status": "pending", "priority": "high"}
]
```

## Mode-Specific Templates

### Mode: `/begin` (Session Startup)
**Auto-generate these todos:**
```json
[
  {"id": "startup-1", "content": "Read Rule 0: Read ALL rule files in /docs/rules/", "status": "pending", "priority": "high"},
  {"id": "startup-2", "content": "Read Product Context: Read /docs/product/ files", "status": "pending", "priority": "high"},
  {"id": "startup-3", "content": "Read Architecture: Read /docs/architecture.md", "status": "pending", "priority": "high"},
  {"id": "startup-4", "content": "Read Current State: Read /docs/session-context/CURRENT_STATE.md", "status": "pending", "priority": "high"},
  {"id": "startup-5", "content": "Read Task Queue: Read /docs/session-context/NEXT_TASKS.md", "status": "pending", "priority": "high"},
  {"id": "startup-6", "content": "Select ONE Task: Choose single task from 'now' priority", "status": "pending", "priority": "high"},
  {"id": "startup-7", "content": "Create Feature Branch: Following git-workflow.md rules", "status": "pending", "priority": "high"},
  {"id": "startup-8", "content": "Ready for Work Mode: Complete startup, ready to switch to /work", "status": "pending", "priority": "high"}
]
```

### Mode: `/plan` (Task Planning)
**Auto-generate these todos:**
```json
[
  {"id": "plan-1", "content": "Review Current CRD: Understand scope and acceptance criteria", "status": "pending", "priority": "high"},
  {"id": "plan-2", "content": "Break Down Tasks: Create specific, testable sub-tasks", "status": "pending", "priority": "high"},
  {"id": "plan-3", "content": "Create GitHub Issues: One issue per task with proper labels", "status": "pending", "priority": "high"},
  {"id": "plan-4", "content": "Set Priorities: Assign 'now', 'next', 'future' labels appropriately", "status": "pending", "priority": "medium"},
  {"id": "plan-5", "content": "Validate Scope: Ensure tasks align with CRD goals", "status": "pending", "priority": "medium"},
  {"id": "plan-6", "content": "Select Next Task: Choose single task for immediate work", "status": "pending", "priority": "high"},
  {"id": "plan-7", "content": "Check-in: Confirm plan before implementation", "status": "pending", "priority": "high"}
]
```

### Mode: `/dev` (Core Development)
**Auto-generate these todos:**
```json
[
  {"id": "dev-1", "content": "Write Failing Test: Create test for current requirement", "status": "pending", "priority": "high"},
  {"id": "dev-2", "content": "Run Test: Verify test fails (red phase)", "status": "pending", "priority": "high"},
  {"id": "dev-3", "content": "Implement Minimum: Write minimum code to pass test", "status": "pending", "priority": "high"},
  {"id": "dev-4", "content": "Run Test: Verify test passes (green phase)", "status": "pending", "priority": "high"},
  {"id": "dev-5", "content": "Refactor: Clean up code while keeping tests green", "status": "pending", "priority": "medium"},
  {"id": "work-6", "content": "Commit: Commit changes with proper message format", "status": "pending", "priority": "high"},
  {"id": "work-7", "content": "Check-in: Provide check-in before next task", "status": "pending", "priority": "high"}
]
```

### Mode: `/deliver` (Delivery)
**Auto-generate these todos:**
```json
[
  {"id": "deliver-1", "content": "Final Testing: Ensure all tests pass", "status": "pending", "priority": "high"},
  {"id": "deliver-2", "content": "Git Workflow: Pull main, merge, commit with George attribution", "status": "pending", "priority": "high"},
  {"id": "deliver-3", "content": "Push Branch: Push to remote following git rules", "status": "pending", "priority": "high"},
  {"id": "deliver-4", "content": "Create PR: Use proper PR template with test plan", "status": "pending", "priority": "high"},
  {"id": "deliver-5", "content": "Update Issues: Close completed GitHub issues", "status": "pending", "priority": "medium"},
  {"id": "deliver-6", "content": "Update Documentation: Add user/developer guides if needed", "status": "pending", "priority": "medium"},
  {"id": "deliver-7", "content": "Session Completion: Update context files for next session", "status": "pending", "priority": "medium"},
  {"id": "deliver-8", "content": "Clear Context: Instruct operator to /clear context", "status": "pending", "priority": "high"}
]
```

## Usage Instructions

### For AI Assistant (George):
1. **Use Mode Commands**: Load appropriate mode with `/begin`, `/plan`, `/dev`, `/deliver`
2. **Auto-generate mode templates** when entering each mode
3. **Mark todos complete** as each step is accomplished
4. **Never skip compliance todos** - they ensure process adherence
5. **Transition modes** only after completing current mode checklist

### Template Activation Triggers:
- **Session Start**: Always use Session Startup Template
- **Git Operations**: Use Git Workflow Template before any push
- **Major Work**: Use Check-in Template before significant changes
- **Development**: Use Development Work Template for .NET coding
- **Branch Work**: Use Branch Management Template when switching contexts
- **Session End**: Use Session Completion Template

### Customization:
- Templates can be combined for complex workflows
- Priority levels guide execution order
- Templates ensure no critical steps are missed