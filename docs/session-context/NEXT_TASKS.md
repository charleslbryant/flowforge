# Next Tasks Queue

## Now (Active Sprint)
1. [ ] **Issue #24: CRD-2.3 Workflow Create Command** - Create new workflows via n8n API (PRIORITY)
2. [ ] **Issue #25: CRD-2.6 Workflow Activation Commands** - Activate/deactivate workflows
3. [ ] **Issue #26: CRD-2.5 Workflow Delete Command** - Delete workflows by ID

## Next (Backlog)
1. [ ] **Issue #27: CRD-2.4 Workflow Update Command** - Update existing workflows
2. [ ] **Issue #43: PRD OpenTelemetry Integration** - Add telemetry and monitoring
3. [ ] Create workflow execution monitoring commands
4. [ ] Add credential management commands
5. [ ] Implement workflow validation against n8n schema

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
- [x] **Issue #23: CRD-2.2 Workflow Get Command** - 2025-07-14
- [x] Health command implementation - 2025-07-12
- [x] Doctor command implementation - 2025-07-12  
- [x] Start command implementation - 2025-07-12
- [x] Stop command implementation - 2025-07-13
- [x] Restart command implementation - 2025-07-13
- [x] List Workflows command implementation - 2025-07-14
- [x] Get Workflow command implementation - 2025-07-14
- [x] JSON Output Option for list-workflows command - 2025-07-14
- [x] JSON Output Option for get-workflow command - 2025-07-14
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

### Issue #24: Workflow Create Command (Priority: Now)
- **Goal**: Create new workflows via n8n API
- **Approach**: New CreateWorkflowCommand with workflow definition input
- **Tests**: Valid workflow, invalid workflow, API errors
- **Estimated time**: 2-3 hours

### Issue #25: Workflow Activation Commands (Priority: Now)
- **Goal**: Activate/deactivate workflows via n8n API
- **Approach**: New ActivateWorkflowCommand and DeactivateWorkflowCommand
- **Tests**: Valid workflow, invalid workflow, state transitions
- **Estimated time**: 1-2 hours

### Issue #26: Workflow Delete Command (Priority: Now)
- **Goal**: Delete workflows by ID via n8n API
- **Command**: `forge-dotnet delete-workflow <id>`
- **Approach**: New DeleteWorkflowCommand, extend WorkflowService with DeleteWorkflowAsync
- **Tests**: Valid ID, invalid ID, non-existent workflow, API errors, confirmation prompts
- **Estimated time**: 1-2 hours

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