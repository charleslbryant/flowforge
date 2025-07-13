# Next Tasks Queue

## Now (Active Sprint)
1. [ ] Implement Stop command with TDD approach
2. [ ] Implement Restart command with TDD approach  
3. [ ] Add workflow list command (n8n API integration)
4. [ ] Create workflow create command with validation

## Next (Backlog)
1. [ ] Add configuration management for n8n API settings
2. [ ] Implement comprehensive error handling and logging
3. [ ] Add workflow delete and update commands
4. [ ] Create workflow execution monitoring commands
5. [ ] Add credential management commands
6. [ ] Implement workflow validation against n8n schema

## Future (Icebox)
1. [ ] Add performance optimizations for large workflow operations
2. [ ] Implement workflow template system integration
3. [ ] Add export/import functionality for workflows
4. [ ] Create workflow testing framework
5. [ ] Add monitoring and alerting for n8n health
6. [ ] Implement workflow versioning and rollback

## Completed ✅
- [x] Health command implementation - 2025-07-12
- [x] Doctor command implementation - 2025-07-12  
- [x] Start command implementation - 2025-07-12
- [x] Service refactoring with proper namespaces - 2025-07-12
- [x] Clean architecture implementation - 2025-07-12
- [x] Comprehensive test suite setup - 2025-07-12
- [x] ADR documentation system - 2025-07-12

## Task Details

### Stop Command (Priority: Now)
- **Goal**: Stop n8n process gracefully
- **Approach**: TDD with ProcessManager.StopN8nAsync()
- **Tests**: Process not running, successful stop, failed stop scenarios
- **Estimated time**: 1-2 hours

### Restart Command (Priority: Now)  
- **Goal**: Restart n8n process (stop + start)
- **Approach**: Leverage existing Start/Stop commands
- **Tests**: Already running, not running, failed restart scenarios
- **Estimated time**: 1 hour

### Workflow List Command (Priority: Now)
- **Goal**: List all n8n workflows via API
- **Approach**: New WorkflowService with HTTP client integration
- **Tests**: API success, failure, empty list scenarios
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

---
*Last updated: 2025-07-12*  
*This file should be updated at the end of each development session*