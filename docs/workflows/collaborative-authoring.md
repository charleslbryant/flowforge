# Collaborative Authoring Workflow

## Purpose
Collaborate with Claude to draft PRDs, CRDs, and Tasks, then create them directly in GitHub Issues.

## New Session Startup

### Required Reading (5 minutes)
1. **Product Context**: Read `/docs/product/product-vision.md` and `/docs/product/product-strategy.md`
2. **Architecture**: Read `/docs/architecture.md` for technical overview
3. **Current State**: Read `/docs/session-context/CURRENT_STATE.md` for development progress
4. **Task Queue**: Read `/docs/session-context/NEXT_TASKS.md` for prioritized work

### Session Startup Command
**User**: "Show me available tasks" 
**Claude**: Reviews current GitHub Issues and provides task options

## Workflow Commands

### 1. Review Available Tasks
**Command**: "Show me available tasks"
**Claude Action**: 
```bash
# Show unassigned tasks
gh issue list --label "task" --no-assignee --label "now,next" --json number,title,labels,state

# Show my assigned tasks  
gh issue list --assignee "charleslbryant" --label "task" --state "open" --json number,title,labels,state
```

### 2. Choose and Start Task
**Command**: "Let's work on task #123"
**Claude Action**:
```bash
# Assign to user
gh issue edit 123 --add-assignee "charleslbryant"

# Update priority to 'now' if needed
gh issue edit 123 --add-label "now" --remove-label "next,future"

# Update session context
# Update ACTIVE_SESSION.md with current task
```

### 3. Create PRD Together
**Workflow**:
1. User: "Let's create a PRD for [feature description]"
2. Claude: Draft PRD content based on template
3. User: Review and refine together
4. Claude: Create in GitHub when approved
5. **IMPORTANT**: Do NOT break down PRD into CRDs immediately

**Claude Command**:
```bash
gh issue create \
  --title "PRD: [Feature Name]" \
  --body-file temp-prd.md \
  --label "prd,future" \
  --assignee "charleslbryant"
```

### 4. Break Down PRD into CRDs (Just-In-Time)
**Trigger**: When starting work on a specific PRD
**Workflow**:
1. User: "Let's start work on PRD #X" 
2. Claude: Break down PRD into user stories (CRDs)
3. User: Review and approve user stories
4. Claude: Create all CRDs as GitHub Issues linked to parent PRD
5. **IMPORTANT**: Do NOT break down CRDs into tasks immediately

### 5. Break Down CRD into Tasks (Just-In-Time)
**Trigger**: When starting development on a specific CRD labeled 'now'
**Workflow**:
1. User: "Create tasks for CRD #X" (only for 'now' priority CRDs)
2. Claude: Break down CRD into specific implementation tasks
3. User: Review and approve task breakdown
4. Claude: Create all tasks as GitHub Issues linked to parent CRD
5. Ready to start development using TDD approach

**Rule**: Only create tasks for CRDs with 'now' priority to maintain just-in-time flow

### 6. Create CRD Together (Manual)  
**Workflow**:
1. User: "Let's create a CRD for [user story]"
2. Claude: Draft user story in format "As a [user], I want [capability] so that [benefit]"
3. User: Review and refine together
4. Claude: Create in GitHub when approved

**Claude Command**:
```bash
gh issue create \
  --title "CRD: User can [capability]" \
  --body-file temp-crd.md \
  --label "crd,future"
```

### 7. Create Task Together (Manual)
**Workflow**:
1. User: "Let's create a task for [specific work]"
2. Claude: Draft task with technical details
3. User: Review and refine together
4. Claude: Create in GitHub when approved

**Claude Command**:
```bash
gh issue create \
  --title "Task: [Specific Implementation]" \
  --body-file temp-task.md \
  --label "task,future"
```

## Templates for Collaboration

### PRD Template (for drafting)
```markdown
# PRD: [Feature Name]

## Problem Statement
What problem are we solving?

## User Impact
Who benefits and how?

## Success Criteria
How do we measure success?

## Requirements
- [ ] Requirement 1
- [ ] Requirement 2

## Out of Scope
What we're NOT doing in this PRD.
```

### CRD Template (for drafting)
```markdown
# CRD: User can [capability]

## User Story
As a **[user type]**, I want **[capability]** so that **[benefit]**.

## Parent PRD
- [#issue-number](github-url)

## Acceptance Criteria
- [ ] Given [context] when [action] then [outcome]
- [ ] Given [context] when [action] then [outcome]

## Technical Approach
High-level implementation strategy.

## Tasks Breakdown
- [ ] Task 1: [Description]
- [ ] Task 2: [Description]
```

### Task Template (for drafting)
```markdown
# Task: [Specific Implementation]

## Description
Specific work to be completed.

## Parent CRD
- [#issue-number](github-url)

## Acceptance Criteria
- [ ] Specific deliverable 1
- [ ] Specific deliverable 2

## Implementation Details
- Files to modify
- Technical approach
- Testing strategy

## Time Estimate
X hours
```

## Example Collaborative Session

```
User: "Show me available tasks"

Claude: 
Available Tasks (Unassigned):
- #125: Implement ProcessManager.StopN8nAsync() [task, now]
- #126: Add StopCommand to CLI registration [task, next]

My Assigned Tasks:
- #124: Write failing tests for StopCommand [task, now, in-progress]

User: "Let's work on task #125"

Claude: 
✓ Assigned task #125 to charleslbryant
✓ Updated priority to 'now'
✓ Updated ACTIVE_SESSION.md

Task #125: "Implement ProcessManager.StopN8nAsync()" is ready!
Let's start with TDD - should I read the current ProcessManager implementation?
```

## Integration with Session Context

When starting/changing tasks, Claude automatically:
1. Updates `ACTIVE_SESSION.md` with current task
2. Ensures proper GitHub Issue assignment and labeling
3. Reads relevant context (parent CRD, related code)
4. Suggests next steps based on task requirements

This creates a seamless flow from collaborative planning to focused implementation!