# FlowForge Mode System

## Overview
The FlowForge mode system provides focused, context-aware agentic workflows through custom Claude Code slash commands. Each mode restricts tools and context to specific phases of development work.

## Available Modes

### `/begin` - Session Startup Mode
**Purpose**: Initialize development session with proper context loading
**Tools**: Read, TodoWrite, Bash(git/gh status commands)
**Focus**: Reading project state, selecting tasks, creating branches
**Transitions**: → `/dev` when startup complete

### `/dev` - Development Mode  
**Purpose**: TDD implementation following one-task focus
**Tools**: Read, Edit, Write, Bash, Testing tools, TodoWrite
**Focus**: Test-driven development, single task completion
**Transitions**: → `/plan` for scope changes, → `/deliver` when ready

### `/plan` - Task Planning Mode
**Purpose**: Breaking down CRDs into actionable GitHub issues
**Tools**: Read, Write, TodoWrite, Bash(gh issue commands)
**Focus**: Task breakdown, scope management, prioritization
**Transitions**: → `/dev` with selected task, → `/deliver` if complete

### `/deliver` - Delivery Mode
**Purpose**: Completing work, following git workflow, creating PRs
**Tools**: Read, Write, TodoWrite, Bash(git/gh commands), Testing
**Focus**: Git operations, PR creation, documentation, session completion
**Transitions**: → `/begin` for new session

## Mode Benefits

✅ **Focused Context** - Only relevant rules and tools loaded
✅ **Process Compliance** - Automatic TodoWrite checklists prevent rule violations  
✅ **Clear Transitions** - Explicit handoffs between workflow phases
✅ **Reduced Cognitive Load** - No irrelevant information in context
✅ **Better Quality** - Mode restrictions prevent common mistakes

## Usage

1. **Start Every Session**: Use `/begin` at session start
2. **Follow Mode Rules**: Stay within mode boundaries
3. **Complete Checklists**: Mark TodoWrite items complete as you progress
4. **Suggest Next Mode**: When mode checklist is complete, ALWAYS suggest appropriate next mode to operator

## Mode Transition Rules

### When Mode Complete - MANDATORY:
**Always suggest next mode when current mode checklist is finished:**

From `/begin` → Suggest `/dev` (if task selected) or `/plan` (if need task breakdown)
From `/dev` → Suggest `/deliver` (if ready to push) or `/plan` (if scope expanding)  
From `/plan` → Suggest `/dev` (with selected task) or `/deliver` (if planning complete)
From `/deliver` → Suggest `/begin` (for new session after /clear context)

### Mode Completion Check-in Format:
```
## Mode Completion Check-in
**Just completed**: [Current mode] with all checklist items
**Current state**: [Brief summary of what was accomplished]
**Selected task**: [For /begin mode - the task selected for work]
**Recommended next mode**: /[mode] because [reason]
**Your options**:
A) Approve task and switch to: /[mode]
B) Select different task: [alternative task]
C) Stay in current mode for: [specific reason]
D) Switch to different mode: [alternative with reason]
```

### Special Rules for `/begin` Mode Completion:
- **MUST get operator approval** for selected task before switching to `/dev`
- **Task selection requires confirmation** - never assume task is approved
- **Include task details** in completion check-in for operator review

## Troubleshooting

**Commands not showing in `/help`**: This is a known limitation. Commands are functional but may not appear in help list.

**Mode switching**: Always complete current mode checklist before switching modes.

**Tool restrictions**: If you need tools not allowed in current mode, consider if you should switch modes first.

## File Locations
- **Commands**: `.claude/commands/*.md`
- **Documentation**: `/docs/modes/`
- **Templates**: `/docs/rules/todowrite-templates.md`