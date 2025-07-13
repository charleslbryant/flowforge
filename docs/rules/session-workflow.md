# Session Workflow Rules

## Session Startup (MANDATORY)
1. Read `/docs/product/` for product context
2. Read `/docs/architecture.md` for technical overview  
3. Read `/docs/session-context/CURRENT_STATE.md` for progress
4. Read `/docs/session-context/NEXT_TASKS.md` for work queue
5. Select ONE task from "Now" priority
6. Assign GitHub issue to self
7. Create feature branch following git-workflow.md

## Development Rules

### One Task Focus
- Work on ONE GitHub issue at a time
- Complete current task before starting new one
- If scope expands beyond original task, check with operator

### TDD Approach (.NET)
- Write failing test first
- Implement minimum code to pass
- Refactor and improve
- Maintain test coverage

### Documentation Requirements
- Update user guides for user-facing features
- Update developer guides for APIs/technical features
- Include in PR review process

## Session Completion
1. Commit all changes following git-workflow.md
2. Create Pull Request with template
3. Close completed GitHub issues
4. Update context files for next session
5. **Instruct operator to `/clear context`**
6. End session

## TodoWrite Integration
Session startup should automatically add:
- [ ] Read all session startup context files
- [ ] Select and assign ONE task from "Now" priority
- [ ] Create feature branch for selected task
- [ ] Follow TDD approach for .NET development
- [ ] Update documentation as needed
- [ ] Follow git workflow rules for all commits