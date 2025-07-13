# Product Management

## Overview
Our product development is organized using GitHub Issues for requirements, change requests, and task tracking. This modern approach enables better team collaboration, automation, and traceability.

## Product Management Workflow

### 1. Product Requirements (PRDs)
**Location**: GitHub Issues with `prd` label
**Purpose**: Define feature requirements, user problems, jobs-to-be-done, and user journeys
**Template**: Use GitHub Issue template "PRD Requirement"
**Process**: 
- Create new PRD using GitHub Issue template
- Define jobs-to-be-done, user journey, requirements, and success criteria
- Apply appropriate labels (`prd`, `requirement`, `product`, `phase-X`)
- Assign to team members responsible for implementation

### 2. Roadmap
**Location**: GitHub Issues organized by labels and milestones
**Purpose**: Prioritize PRDs into Now, Next, and Future buckets
**Process**:
- Use GitHub labels to categorize by phase (`phase-1`, `phase-2`, `phase-3`, `phase-4`)
- Use GitHub Projects or Milestones for sprint planning
- Update issue status to reflect current priorities

### 3. Change Requests (CRDs)
**Location**: GitHub Issues with `crd` and `epic` labels
**Purpose**: Break down "Now" PRDs into actionable implementation tasks
**Template**: Use GitHub Issue template "CRD Epic"
**Process**:
- Create CRD for each active PRD
- Link CRD to parent PRD issue
- Define acceptance criteria and implementation tasks
- Assign to team members

### 4. Task Management
**Location**: 
- **Team Level**: GitHub Issues with `task` label
- **Personal Level**: `docs/tasks/personal/{username}/tasklog-{date}.md`
**Purpose**: Track individual work and daily progress
**Process**:
- Create personal tasklogs for daily work tracking
- Link personal tasks to GitHub Issues using `#issue-number`
- Update task status as work progresses
- Use GitHub Issues for team coordination

## GitHub Issues Hierarchy

### Level 1: Product Requirements (PRDs)
- **Issue Numbers**: #100, #101, #102, #103
- **Labels**: `prd`, `requirement`, `product`, `phase-X`
- **Content**: Jobs-to-be-done, user journey, requirements, success criteria

### Level 2: Change Requests (CRDs)
- **Issue Numbers**: #121, #122, #123, #124
- **Labels**: `epic`, `crd`, `phase-X`
- **Content**: Implementation plans, acceptance criteria, tasks

### Level 3: Personal Tasks
- **Issue Numbers**: #125, #126, #127, #128
- **Labels**: `task`, `personal`
- **Content**: Individual work items linked to CRDs

## Session Workflow

### Session Start (trigger: "let's go")
1. **Review Memory Bank**: Read all memory bank files
2. **Review GitHub Issues**: Check current PRDs, CRDs, and personal tasks
3. **Review Personal Tasklog**: Check today's tasklog or create new one
4. **Determine Next Task**: Work with operator to identify priority
5. **Create Plan**: Document plan in chat

### Execution
- Work with operator to execute plan
- Update personal tasklog with progress
- Update GitHub Issues with status changes
- Document decisions and challenges

### Session Recap (trigger: "recap")
1. **Review Changes**: What was accomplished during session
2. **Update Tasklog**: Move completed tasks to "Done" section
3. **Update GitHub Issues**: Update status, add comments, close completed issues
4. **Update Memory Bank**: Reflect current state and decisions
5. **Git Operations**: Commit changes and push to repository

## Benefits of GitHub Issues Approach

### Team Collaboration
- **No Conflicts**: Namespaced personal tasklogs prevent merge conflicts
- **Clear Ownership**: GitHub Issues show who's responsible
- **Better Visibility**: Team can see all work in one place
- **Automated Workflows**: GitHub Actions for CI/CD integration

### Individual Productivity
- **Personal Space**: Individual tasklogs for personal work tracking
- **Flexible Format**: Personal logs can include detailed notes
- **Easy Linking**: Simple reference to team-level work
- **No Interference**: Work independently without conflicts

### Project Management
- **Traceability**: Link personal work to team goals
- **Progress Tracking**: Clear visibility into project status
- **Scalability**: Works with any team size
- **Integration**: Connects with existing GitHub workflow

## Related Documentation
- [Team Collaboration Guide](../tasks/team-collaboration-guide.md)
- [Traceability Matrix](../github-issues/traceability-matrix.md)
- [Task Management](../tasks/readme.md)
- [Memory Bank](../memory-bank/)

## Migration Notes
- **Legacy Files**: Old PRD and CRD markdown files archived in `docs/archive/legacy-docs/`
- **Migration Date**: $(date)
- **Status**: Complete migration to GitHub Issues workflow
