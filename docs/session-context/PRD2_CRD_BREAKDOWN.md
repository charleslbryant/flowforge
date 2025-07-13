# PRD #2: Workflow Management Commands - CRD Breakdown

## Source PRD
- **GitHub Issue**: #2 - PRD: Workflow Management Commands
- **Status**: Open, labeled `prd` and `now` (high priority)
- **Problem**: FlowForge operators need workflow CRUD operations via CLI

## Success Criteria from PRD
- List all workflows with `forge-dotnet list-workflows`
- Create workflows from JSON with `forge-dotnet create-workflow`
- Delete workflows with `forge-dotnet delete-workflow`
- Activate/deactivate workflows via CLI
- 90% workflow operation success rate
- Clear error handling and output formatting

## Component Requirements Documents (CRDs)

### Workflow List Command (Issue #22)
**User Story**: As an operator, I can list all workflows to see what's deployed
- **Command**: `forge-dotnet list-workflows`
- **Output**: Table showing ID, name, active status, created date
- **API**: GET /workflows endpoint
- **Error Handling**: Connection failures, empty lists
- **Acceptance Criteria**:
  - Shows all workflows from n8n API
  - Table format with ID, name, status columns
  - JSON output option with --json flag
  - Handles API connectivity issues gracefully

### Workflow Get Command (Issue #23)  
**User Story**: As an operator, I can get workflow details and JSON definition
- **Command**: `forge-dotnet get-workflow <id>`
- **Output**: Workflow metadata + JSON definition
- **API**: GET /workflows/{id} endpoint
- **Error Handling**: Invalid ID, workflow not found
- **Acceptance Criteria**:
  - Displays workflow metadata (name, status, dates)
  - Shows workflow JSON definition
  - Validates workflow ID format
  - Clear error for non-existent workflows

### Workflow Create Command (Issue #24)
**User Story**: As an operator, I can deploy new workflows from JSON files
- **Command**: `forge-dotnet create-workflow <file.json>`
- **Input**: JSON file containing workflow definition
- **API**: POST /workflows endpoint
- **Error Handling**: Invalid JSON, file not found, API validation errors
- **Acceptance Criteria**:
  - Validates JSON file exists and is readable
  - Validates workflow JSON schema
  - Creates workflow in n8n via API
  - Returns new workflow ID on success
  - Clear feedback on validation failures

### Workflow Update Command (Issue #27)
**User Story**: As an operator, I can update existing workflows from JSON
- **Command**: `forge-dotnet update-workflow <id> <file.json>`
- **Input**: Workflow ID + JSON file with updated definition
- **API**: PUT/PATCH /workflows/{id} endpoint
- **Error Handling**: Invalid ID, JSON validation, concurrent modifications
- **Acceptance Criteria**:
  - Validates workflow exists before update
  - Validates JSON file and schema
  - Updates workflow definition via API
  - Confirms successful update
  - Handles version conflicts gracefully

### Workflow Delete Command (Issue #26)
**User Story**: As an operator, I can safely remove workflows with confirmation
- **Command**: `forge-dotnet delete-workflow <id>`
- **Safety**: Confirmation prompt unless --force flag used
- **API**: DELETE /workflows/{id} endpoint
- **Error Handling**: Invalid ID, workflow not found, active workflows
- **Acceptance Criteria**:
  - Shows workflow details before deletion
  - Prompts for confirmation (unless --force)
  - Prevents deletion of active workflows (unless --force)
  - Confirms successful deletion
  - Clear error messages for failures

### Workflow Activation Commands (Issue #25)
**User Story**: As an operator, I can activate/deactivate workflows for execution
- **Commands**: 
  - `forge-dotnet activate-workflow <id>`
  - `forge-dotnet deactivate-workflow <id>`
- **API**: PUT/PATCH /workflows/{id} with active field
- **Error Handling**: Invalid ID, already in target state
- **Acceptance Criteria**:
  - Changes workflow active status via API
  - Confirms status change
  - Handles workflows already in target state
  - Shows current status after change

### Workflow Output Formatting (Issue #28)
**User Story**: As an operator, I need both human-readable and scriptable output
- **Formats**: Table (default), JSON (--json flag)
- **Consistency**: All workflow commands support both formats
- **Error Handling**: Consistent error format across commands
- **Acceptance Criteria**:
  - Default table output for human readability
  - --json flag for machine parsing
  - Consistent column names and data formats
  - Proper error codes for scripting

### Workflow Error Handling & Validation (Issue #29)
**User Story**: As an operator, I get clear feedback when operations fail
- **Validation**: JSON schema, workflow IDs, file existence
- **API Errors**: Connection timeouts, authentication, server errors
- **User Errors**: Invalid commands, missing files, permission issues
- **Acceptance Criteria**:
  - Validates inputs before API calls
  - Clear error messages with suggested fixes
  - Proper exit codes for different error types
  - Graceful handling of network issues

## Implementation Priority
1. **Issue #22**: Workflow List (foundation for all other commands)
2. **Issue #23**: Workflow Get (needed for create/update validation)
3. **Issue #24**: Workflow Create (core functionality)
4. **Issue #25**: Activation Commands (simple state toggle)
5. **Issue #26**: Workflow Delete (requires confirmation UX)
6. **Issue #27**: Workflow Update (most complex - combines get/create)
7. **Issue #28**: Output Formatting (enhancement across all commands)
8. **Issue #29**: Error Handling (ongoing improvement)

## Technical Dependencies
- **WorkflowService**: New service for n8n workflow API operations
- **JSON Validation**: Schema validation for workflow definitions
- **File Operations**: Reading JSON files securely
- **API Client**: Extension of existing N8nHttpClient
- **Output Formatting**: Consistent table/JSON output utilities

## Reference Implementation
- **Bash Version**: `/scripts/n8n-api.sh` - Complete working implementation
- **API Documentation**: n8n REST API for workflow operations
- **Current Architecture**: Extend existing clean architecture pattern

---
*Created: 2025-07-13*  
*Next: Create GitHub issues for each CRD and assign implementation order*