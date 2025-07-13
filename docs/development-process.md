# FlowForge Development Process

## Issue Hierarchy

### 1. PRD (Product Requirement Document)
- **Purpose**: High-level feature specifications
- **Label**: `prd`
- **Example**: "PRD: Workflow Management Commands"
- **Action**: Break down into multiple CRDs

### 2. CRD (Change Request Document)  
- **Purpose**: Specific user story with acceptance criteria
- **Label**: `crd`
- **Parent**: Links to a PRD
- **Example**: "CRD: Operator can stop n8n process"
- **Action**: Break down into multiple Tasks

### 3. Task
- **Purpose**: Single implementable work item
- **Label**: `task` + priority (`now`, `next`, `future`)
- **Parent**: Links to a CRD
- **Example**: "Task: Create ProcessOperationResult class for detailed feedback"
- **Action**: Implement with TDD approach

## Development Workflow

### Step 1: Select Work
```bash
# View available CRDs
gh issue list --label "crd,now" --state "open"

# View available tasks
gh issue list --label "task,now" --state "open" --json number,title,assignees
```

### Step 2: Working on a CRD
1. **Assign CRD to yourself**
   ```bash
   gh issue edit [CRD-number] --add-assignee "charleslbryant"
   ```

2. **Create feature branch for the CRD**
   ```bash
   git checkout -b feature/issue-[CRD-number]-[brief-description]
   ```

3. **Break down into tasks** (if not already done)
   - Review "Tasks Breakdown" section in CRD
   - Create GitHub issue for each task
   - Label first task as "now", others as "next" or "future"
   - Link all tasks to parent CRD

4. **Work on tasks sequentially**
   - Assign first "now" task to yourself
   - Implement using TDD
   - Close task when complete
   - Move next task from "next" to "now"
   - Repeat until all tasks complete

### Step 3: Complete CRD
1. **Ensure all tasks are closed**
2. **Create Pull Request**
   ```bash
   gh pr create --title "Implement CRD #[number]: [description]" \
     --body "## Summary
   [Implementation summary]
   
   ## Resolves
   - Closes #[CRD-number]
   - Closes #[task-1-number]
   - Closes #[task-2-number]
   [etc...]
   
   ## Test Plan
   - [ ] All tests pass
   - [ ] Manual testing completed
   
   🤖 Generated with AI Assistant (George)"
   ```

3. **Merge and cleanup**
   ```bash
   gh pr merge --squash --delete-branch
   git checkout main
   git pull origin main
   git branch -d feature/issue-[CRD-number]-[brief-description]
   ```

## Priority Management

### Priority Labels
- **now**: Current sprint, work on immediately
- **next**: Next sprint, work on after "now" items
- **future**: Backlog, not yet prioritized

### Priority Flow
```
future → next → now → assigned → in progress → completed
```

### Updating Priorities
```bash
# Move task from next to now
gh issue edit [number] --add-label "now" --remove-label "next"

# Move task from future to next
gh issue edit [number] --add-label "next" --remove-label "future"
```

## Example: CRD #9 Process

1. **CRD #9**: "Operator gets clear feedback on process operations"
   
2. **Task Breakdown**:
   - Task #15: Create ProcessOperationResult class (now)
   - Task #16: Enhance ProcessManager (next)
   - Task #17: Update StopCommand (future)

3. **Workflow**:
   ```bash
   # Assign CRD
   gh issue edit 9 --add-assignee "charleslbryant"
   
   # Create feature branch
   git checkout -b feature/issue-9-process-feedback
   
   # Work on Task #15
   gh issue edit 15 --add-assignee "charleslbryant"
   # ... implement with TDD ...
   gh issue close 15 --comment "✅ Completed"
   
   # Move Task #16 to now
   gh issue edit 16 --add-label "now" --remove-label "next"
   # ... continue process ...
   ```

## Best Practices

1. **One CRD per feature branch** - All related tasks share the same branch
2. **One task at a time** - Focus on completing before moving to next
3. **TDD for all tasks** - Write tests first, then implementation
4. **Clear commit messages** - Reference task numbers in commits
5. **Update priorities** - Keep issue labels current with actual work
6. **Document decisions** - Update ADRs for architectural choices

## Common Mistakes to Avoid

1. ❌ Working directly on a PRD without breaking it down
2. ❌ Working on multiple tasks simultaneously  
3. ❌ Creating separate branches for each task in a CRD
4. ❌ Forgetting to create GitHub issues for CRD tasks
5. ❌ Not updating priority labels as work progresses

## Quick Reference

```bash
# Current work status
gh issue list --assignee "@me" --state "open"

# Available high-priority work
gh issue list --label "now" --state "open" --no-assignee

# Create task from CRD
gh issue create --title "Task: [description]" --body "[details]" --label "task,now"

# Complete task
gh issue close [number] --comment "✅ Completed: [summary]"

# Update priorities
gh issue edit [number] --add-label "now" --remove-label "next,future"
```