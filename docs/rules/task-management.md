# Task Management Rules

## GitHub Issue Types

### PRD (Product Requirement Document)
- **Scope**: High-level feature specifications  
- **Action**: Do NOT implement directly - too broad for single session
- **Process**: Break down into CRDs (specific user stories)
- **Linking**: Reference parent PRD in each CRD

### CRD (Change Request Document)  
- **Scope**: Specific user stories with acceptance criteria
- **Process**: 
  1. Review "Tasks Breakdown" section in CRD
  2. Create separate GitHub task issues for each breakdown item
  3. Work on ONE task at a time
  4. Use single feature branch for entire CRD

### Task Issues
- **Creation**: One GitHub issue per CRD task breakdown item
- **Format**:
  ```bash
  gh issue create --title "Task: [specific task description]" \
    --body "Part of CRD #[CRD-number] - [CRD title]
  
  ## Summary
  [Task description]
  
  ## Acceptance Criteria
  - [ ] [Specific criteria for this task]
  
  ## Parent Issues
  - Resolves part of #[CRD-number]
  - Depends on #[previous-task-number] (if applicable)"
  ```

## Task Prioritization

### Label System
- **now**: Current active work (limit 1-2 tasks)
- **next**: Ready for work after current tasks
- **future**: Planned but not yet ready

### Branch Strategy
- **One branch per CRD**: `feature/crd-[number]-[description]`
- **All CRD tasks share the same branch**
- **Complete CRD before merging branch**

## Work Process

### Task Assignment
1. Assign "now" priority task to yourself
2. Move from "next" to "now" when ready for new work
3. Work on ONE task at a time until completion

### Task Completion
1. Complete implementation and tests
2. Mark GitHub issue as completed with comment
3. Move to next task in same CRD or switch CRDs

### Scope Management
- If work expands beyond original task: STOP and check with operator
- If multiple concerns discovered: create separate tasks/branches
- When in doubt about scope: ask operator before proceeding

## TodoWrite Integration
Task management todos:
- [ ] Review task breakdown in assigned CRD
- [ ] Create GitHub issues for each CRD task if not already created
- [ ] Assign current "now" priority task to self
- [ ] Verify current branch matches the task being worked on
- [ ] Complete current task before starting new work