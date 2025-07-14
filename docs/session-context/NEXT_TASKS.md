# Next Tasks Queue

## Now (Active Sprint)
1. [ ] **Workflow Create Command** - Create new workflows via n8n API
2. [ ] **Workflow Delete Command** - Delete workflows via n8n API
3. [ ] **Workflow Update Command** - Update existing workflows
4. [ ] **Configuration Management** - Add n8n API settings configuration

## Next (Backlog)
1. [ ] Implement comprehensive error handling and logging
2. [ ] Create workflow execution monitoring commands
3. [ ] Add credential management commands
4. [ ] Implement workflow validation against n8n schema
5. [ ] Add workflow export/import functionality
6. [ ] Create workflow template management

## Future (Icebox)
1. [ ] Add performance optimizations for large workflow operations
2. [ ] Implement workflow template system integration
3. [ ] Create workflow testing framework
4. [ ] Add monitoring and alerting for n8n health
5. [ ] Implement workflow versioning and rollback
6. [ ] Add workflow collaboration features

## Completed ✅
- [x] **PRD #1: Stop/Restart Commands for n8n Lifecycle Management** - 2025-07-13
- [x] **PRD #2: Workflow List Command (Issue #22)** - 2025-07-14
- [x] Health command implementation - 2025-07-12
- [x] Doctor command implementation - 2025-07-12  
- [x] Start command implementation - 2025-07-12
- [x] Stop command implementation - 2025-07-13
- [x] Restart command implementation - 2025-07-13
- [x] List Workflows command implementation - 2025-07-14
- [x] JSON Output Option for list-workflows command - 2025-07-14
- [x] Service refactoring with proper namespaces - 2025-07-12
- [x] Clean architecture implementation - 2025-07-12
- [x] Comprehensive test suite setup - 2025-07-12
- [x] ADR documentation system - 2025-07-12

### Completed Workflow List Tasks
- [x] Task #30 - Create WorkflowSummary model
- [x] Task #31 - Create IWorkflowService interface
- [x] Task #32 - Create N8nWorkflowClient infrastructure
- [x] Task #33 - Implement WorkflowService 
- [x] Task #34 - Implement ListWorkflowsCommand
- [x] Task #35 - Write comprehensive unit tests
- [x] Tasks #36-#40 - Additional implementation tasks

## Task Details

### Workflow Create Command (Priority: Now)
- **Goal**: Create new workflows via n8n API
- **Approach**: New CreateWorkflowCommand with workflow definition input
- **Tests**: Valid workflow, invalid workflow, API errors
- **Estimated time**: 2-3 hours

### Workflow Delete Command (Priority: Now)  
- **Goal**: Delete workflows by ID via n8n API
- **Approach**: New DeleteWorkflowCommand with confirmation
- **Tests**: Existing workflow, non-existent workflow, API errors
- **Estimated time**: 1-2 hours

### Workflow Update Command (Priority: Now)
- **Goal**: Update existing workflows via n8n API
- **Approach**: New UpdateWorkflowCommand with workflow definition
- **Tests**: Valid updates, invalid updates, non-existent workflow
- **Estimated time**: 2-3 hours

## Session Planning Notes

### For Next Session
- Read CURRENT_STATE.md for architecture context
- Pick ONE task from "Now" priority
- Use TDD approach (red-green-refactor)
- Update ADR if architectural decisions are made
- Update this file when task is completed

### Context Requirements
- Minimal: Current task + basic architecture understanding
- Reference: ADRs for previous decisions
- Tests: Existing test patterns to follow
- Workflow infrastructure now available in WorkflowService

---
*Last updated: 2025-07-14*  
*This file should be updated at the end of each development session*