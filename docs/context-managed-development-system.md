# Context-Managed Development System (CMDS)

## Overview

A comprehensive development workflow that minimizes AI context usage while maintaining team alignment, product focus, and individual developer productivity through structured documentation, task management, and session boundaries.

## Core Principles

### 1. Context Minimization
- **Single Task Sessions**: Each AI session focuses on ONE discrete task
- **Context Boundaries**: Clear start/stop points with context handoff documentation
- **Memory Externalization**: Critical context stored in files, not AI memory
- **Session Isolation**: Each session starts fresh with minimal required context

### 2. Product Alignment
- **GitHub Issues as Source of Truth**: All product decisions tracked in issues
- **PRD → CRD → Task Chain**: Clear traceability from product vision to implementation
- **Label-Based Prioritization**: Simple `now`/`next`/`future` priority system
- **Milestone-Driven Development**: Time-boxed delivery cycles

### 3. Team Coordination
- **GitHub Issues as Single Source**: All work tracked in centralized GitHub Issues
- **Automated Synchronization**: GitHub Actions keep local context files current
- **ADR Documentation**: Architectural decisions preserved across sessions
- **Session Handoff Protocols**: Standardized context transfer between sessions

## System Architecture

### Information Hierarchy
```
Product Strategy (GitHub Issues)
├── PRD Requirements (prd label)
│   ├── CRD Epics (crd label) 
│   │   ├── Personal Tasks (task label)
│   │   │   ├── Development Sessions
│   │   │   │   ├── ADRs (technical decisions)
│   │   │   │   ├── Code Changes
│   │   │   │   └── Tests
│   │   │   └── Session Context Files
│   │   └── Team Coordination
│   └── Roadmap Planning
└── Context Management Files
```

### File Structure
```
/docs/
├── session-context/           # AI Session Management
│   ├── CURRENT_STATE.md      # Current project state
│   ├── NEXT_TASKS.md         # Prioritized task queue  
│   ├── ACTIVE_SESSION.md     # Current session progress
│   └── HANDOFF_TEMPLATE.md   # Session transfer template
├── adr/                      # Architecture Decision Records
├── product/                  # Foundational product vision and strategy
└── guides/                   # Process documentation
```

## Workflow Implementation

### 1. Product Planning Workflow
```
Product Vision → PRD (Feature Story) → CRD (User Story) → Tasks → Development Sessions
```

**Process:**
1. **PRD Creation**: Feature stories as GitHub Issues with `prd` label  
2. **CRD Planning**: User stories as GitHub Issues with `crd` label
3. **Task Breakdown**: Individual work items as GitHub Issues with `task` label
4. **Priority Management**: Use `now`/`next`/`future` labels for prioritization
5. **Epic/Release Grouping**: Group related PRDs and CRDs into epics for releases
6. **Collaborative Authoring**: Draft PRDs/CRDs/Tasks with Claude, then create via `gh`

### 2. Development Session Workflow
```
Session Start → Read Context → Single Task → Document → Commit → Update Context → Session End
```

**Detailed Steps:**
1. **Pre-Session Setup**
   - Read `CURRENT_STATE.md` for project context
   - Read `NEXT_TASKS.md` for task queue
   - Select ONE task from `now` priority

2. **Session Execution**
   - Update `ACTIVE_SESSION.md` with current task
   - Implement using TDD approach
   - Document decisions in ADRs
   - Update relevant documentation

3. **Session Closure**
   - Commit all changes with descriptive message
   - Update `CURRENT_STATE.md` with new state
   - Update `NEXT_TASKS.md` with task completion
   - Create session summary using handoff template
   - Clear AI context (end session)

### 3. Context Management Workflow

#### CURRENT_STATE.md Format
```markdown
# Current State - [Project Name]

## Last Completed Task
- [Task description] ✅
- Issue: [#issue-number](github-url)
- Completed: [date]

## Current Architecture
- [Key components and structure]
- [Recent architectural decisions]

## Build Status
- ✅/❌ Build: [status]
- ✅/❌ Tests: [count]/[total]
- ✅/❌ Quality: [status]

## Active Work
- Current task: [description]
- Issue: [#issue-number](github-url)
- Started: [date]
- Estimated completion: [date]

## Blockers/Issues
- [Any current blockers]
- [Technical debt items]

## Next Session Should Start With
- [Specific next action]
- [Context to review]
```

#### NEXT_TASKS.md Format
```markdown
# Next Tasks Queue

## Now (Active Sprint)
1. [ ] [Task description] - [#issue-number](github-url)
2. [ ] [Task description] - [#issue-number](github-url)

## Next (Backlog)
1. [ ] [Task description] - [#issue-number](github-url)
2. [ ] [Task description] - [#issue-number](github-url)

## Future (Icebox)
1. [ ] [Task description] - [#issue-number](github-url)
2. [ ] [Task description] - [#issue-number](github-url)

## Completed
- [x] [Task description] - [#issue-number](github-url) - [date]
```

### 4. Team Coordination Workflow

#### Daily Coordination
1. **Task Review**: Use `gh` commands to review available and assigned tasks
2. **Collaborative Planning**: Draft new PRDs/CRDs/Tasks with Claude, create via `gh`
3. **Task Assignment**: Claude automatically assigns and prioritizes chosen tasks
4. **Progress Updates**: Comment on GitHub Issues with progress and blockers
5. **Automated Sync**: GitHub Actions sync issues to session context files

#### Sprint Coordination
1. **Sprint Planning**: Use GitHub Projects/Milestones for sprint management
2. **Issue Labels**: Manage priority with `now`/`next`/`future` labels
3. **Automated Reports**: GitHub Actions generate progress summaries
4. **Retrospective**: Update process documentation and ADRs based on learnings

## Context Management Strategies

### 1. AI Session Boundaries
- **Maximum Session Length**: 2-4 hours of development work
- **Single Task Focus**: One GitHub Issue per session maximum
- **Context Handoff**: Always update context files before ending session
- **Fresh Start**: Each new session reads context files, doesn't assume memory

### 2. Information Architecture
- **GitHub Issues**: Source of truth for all product decisions
- **Local Files**: Implementation details, technical decisions, current state
- **Personal Logs**: Individual work tracking and detailed notes
- **ADRs**: Architectural decisions that persist across sessions

### 3. Context Preservation
- **Technical Context**: Stored in ADRs and code comments
- **Product Context**: Stored in GitHub Issues and PRDs
- **Process Context**: Stored in session context files
- **Team Context**: Stored in GitHub Issues comments and automated sync files

## Implementation Benefits

### For AI Development
- **Reduced Context**: Minimal information needed to start productive work
- **Clear Boundaries**: Well-defined start/stop points for sessions
- **Consistent Handoffs**: Standardized context transfer mechanism
- **Focus Maintenance**: Single task per session prevents context drift

### For Team Collaboration
- **Centralized Work Tracking**: All team work visible in GitHub Issues
- **Automated Synchronization**: No manual file management or merge conflicts
- **Clear Ownership**: GitHub Issues show responsibility and progress
- **Scalable Process**: Works with any team size or structure

### For Product Development
- **Traceability**: Clear path from product vision to implementation
- **Priority Management**: Simple label-based prioritization system
- **Progress Visibility**: Real-time view of development progress
- **Quality Assurance**: Built-in documentation and decision recording

## Migration Strategy

### Phase 1: Context Infrastructure (Week 1)
1. Create session context file structure
2. Establish GitHub Issue label system
3. Set up ADR documentation process
4. Create handoff templates

### Phase 2: Process Implementation (Week 2)
1. Convert existing work to GitHub Issues
2. Implement session boundary protocols
3. Train team on new workflow
4. Establish daily coordination rhythm

### Phase 3: Optimization (Week 3-4)
1. Monitor effectiveness and adjust
2. Automate repetitive processes
3. Refine context management based on usage
4. Scale process to full team

## Success Metrics

### Context Efficiency
- **Session Startup Time**: < 5 minutes to become productive
- **Context Size**: < 2000 tokens needed to start work
- **Handoff Completeness**: 100% successful session transfers
- **Information Loss**: Zero critical context lost between sessions

### Team Productivity
- **Issue Velocity**: Consistent completion rate
- **Context Sharing**: All team members can contribute effectively
- **Decision Quality**: Improved architectural decisions through ADRs
- **Rework Reduction**: Fewer changes due to misalignment

### Product Quality
- **Feature Delivery**: Consistent progress on roadmap items
- **Technical Debt**: Managed through systematic documentation
- **Bug Reduction**: Improved quality through structured development
- **Stakeholder Clarity**: Clear visibility into development progress

## Future Enhancements

### Automation Opportunities
1. **GitHub Actions**: Automated issue synchronization
2. **Context Validation**: Automated checks for session handoff completeness
3. **Progress Reporting**: Automated sprint summaries and progress reports
4. **Quality Gates**: Automated checks for ADR and documentation completeness

### Tool Integration
1. **IDE Integration**: Context files directly accessible in development environment
2. **Slack/Teams**: Progress updates automatically posted to team channels
3. **Project Management**: Integration with existing PM tools
4. **Monitoring**: Dashboards for context efficiency and team productivity

### Process Evolution
1. **Metric-Driven Improvement**: Regular analysis of success metrics
2. **Team Feedback Loops**: Regular retrospectives on process effectiveness
3. **Cross-Team Sharing**: Share successful patterns with other teams
4. **Industry Best Practices**: Incorporate learnings from broader development community

---

This Context-Managed Development System provides a comprehensive approach to managing AI-assisted development while maintaining team coordination and product focus. The key is consistent application of session boundaries, structured documentation, and GitHub Issues as the source of truth for all product decisions.