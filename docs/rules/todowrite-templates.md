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
  {"id": "completion-3", "content": "Wait for PR approval and merge", "status": "pending", "priority": "high"},
  {"id": "completion-4", "content": "CRITICAL: Switch to main, pull, delete feature branch", "status": "pending", "priority": "high"},
  {"id": "completion-5", "content": "Update session context files on main branch", "status": "pending", "priority": "high"},
  {"id": "completion-6", "content": "Commit and push context updates to main", "status": "pending", "priority": "high"},
  {"id": "completion-7", "content": "Close completed GitHub issues with completion comments", "status": "pending", "priority": "medium"},
  {"id": "completion-8", "content": "Instruct operator to /clear context", "status": "pending", "priority": "high"}
]
```

## Usage Instructions

### For AI Assistant (George):
1. **Reference this file** at key workflow transition points
2. **Auto-generate relevant template todos** based on current activity
3. **Mark todos complete** as each step is accomplished
4. **Never skip compliance todos** - they ensure process adherence

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