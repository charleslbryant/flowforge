# Post-PR Creation Workflow Guide

## Overview
This guide documents the workflow an AgenticOps agent should follow after successfully creating a pull request and pushing changes to a feature branch.

## Scenario
- ✅ Feature branch created and implemented
- ✅ Changes committed and pushed to remote
- ✅ Pull request created via GitHub CLI
- ✅ PR opened in browser for review

## Immediate Actions Required

### 1. Branch Management
```bash
# Switch back to main branch
git checkout main

# Pull latest changes
git pull origin main

# Verify clean working directory
git status
```

### 2. Task Status Updates
- **Personal Task Log**: Update task status to "Awaiting Review" or "PR Created"
- **Add PR Information**: Include PR link, number, and review status
- **Note Follow-ups**: Document any actions needed after PR review

### 3. Context Preparation for Next Session
- **Memory Bank**: Ensure all recent changes are documented
- **Next Task**: Identify and prepare for next priority task
- **Session Handoff**: Document current state for next development session

## AgenticOps Agent Workflow

### Step 1: Confirm PR Creation Success
```bash
# Verify PR was created successfully
gh pr list --head feature/issue-{number}-{description}

# Check PR status
gh pr view {pr-number} --json state,mergeable,reviewDecision
```

### Step 2: Update Task Documentation
- **GitHub Issue**: Update issue status to "In Review" or "Awaiting Review"
- **Personal Task Log**: Add PR link and review status
- **Memory Bank**: Update progress.md with current status

### Step 3: Prepare for Next Development Session
- **Identify Next Task**: Check memory bank for next priority
- **Update Task Log**: Move next task to "In Progress"
- **Create New Branch**: If starting new task immediately
- **Document Handoff**: Note current state and next steps

### Step 4: Clean Up Development Environment
- **Stop Services**: Stop any running development servers
- **Save Context**: Document any important state or configurations
- **Clear Temporary Files**: Remove any temporary files or configurations

## Common Post-PR Scenarios

### Scenario A: Start New Task Immediately
```bash
# Create new feature branch for next task
git checkout -b feature/issue-{next-number}-{next-description}

# Update task log to reflect new task
# Begin implementation of next feature
```

### Scenario B: Wait for PR Review
```bash
# Switch to main and stay ready
git checkout main
git pull origin main

# Monitor PR status
gh pr list --author @me

# Document waiting status in task log
```

### Scenario C: Address PR Feedback
```bash
# Switch back to feature branch
git checkout feature/issue-{number}-{description}

# Make requested changes
# Commit and push updates
git add .
git commit -m "fix: address PR feedback"
git push origin feature/issue-{number}-{description}
```

## Task Status Transitions

### Before PR Creation
- **Status**: "In Progress"
- **Location**: Personal task log "In Progress" section

### After PR Creation
- **Status**: "Awaiting Review" or "PR Created"
- **Location**: Personal task log "In Progress" section (with PR link)

### After PR Review/Approval
- **Status**: "Ready to Merge" or "Approved"
- **Location**: Personal task log "In Progress" section

### After PR Merge
- **Status**: "Completed"
- **Location**: Personal task log "Done" section

## Documentation Requirements

### PR Information to Document
- **PR Number**: GitHub PR identifier
- **PR URL**: Direct link to the pull request
- **Review Status**: Awaiting review, approved, changes requested
- **Reviewers**: Who needs to review the PR
- **Merge Status**: Ready to merge, blocked, etc.

### Task Log Entry Template
```markdown
- [ ] **Task Name** (#issue-number) - Status: Awaiting Review
  - PR: #123 - [View PR](https://github.com/org/repo/pull/123)
  - Created: 2025-07-05
  - Reviewers: @username1, @username2
  - Status: Awaiting review
  - Next: Address feedback or wait for approval
```

## Quality Assurance Checklist

### Before Switching Tasks
- [ ] PR created successfully
- [ ] PR link documented in task log
- [ ] GitHub issue status updated
- [ ] Memory bank reflects current state
- [ ] Development environment cleaned up
- [ ] Next task identified and prepared

### Before Ending Session
- [ ] Current task status documented
- [ ] Next session context prepared
- [ ] Any blocking issues noted
- [ ] Repository in clean state
- [ ] All changes committed and pushed

## Error Handling

### PR Creation Failed
```bash
# Check GitHub CLI status
gh auth status

# Verify repository access
gh repo view

# Retry PR creation
gh pr create --title "[issue-{number}] {description}" --body "{description}" --base main --head {current-branch} --label {labels} --web
```

### Branch Issues
```bash
# Check current branch
git branch

# Verify remote tracking
git branch -vv

# Fix remote tracking if needed
git branch --set-upstream-to=origin/{branch-name} {branch-name}
```

## Integration with Other Workflows

### Finalizer Mode Integration
- Post-PR creation is part of the Finalizer workflow
- Should be handled automatically by Finalizer mode
- Includes all documentation and status updates

### Initiator Mode Integration
- Next task preparation connects to Initiator mode
- Context handoff ensures smooth session transitions
- Task identification feeds into session planning

## Success Metrics

A successful post-PR workflow should result in:
- ✅ PR created and documented
- ✅ Task status properly updated
- ✅ Repository in clean state
- ✅ Next session context prepared
- ✅ All changes traceable and documented

---

*This workflow ensures smooth transitions between development tasks and maintains proper documentation throughout the development cycle.* 