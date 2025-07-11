#!/usr/bin/env bats
# Tests for scripts/n8n-api.sh

# Load test helpers
load helpers/setup
load helpers/mocks

setup() {
  setup_test_environment
  setup_api_mocks
}

teardown() {
  reset_mocks
  teardown_test_environment
}

@test "n8n-api.sh requires N8N_API_KEY environment variable" {
  unset N8N_API_KEY
  
  run scripts/n8n-api.sh list-workflows
  [ "$status" -eq 1 ]
  assert_output_contains "N8N_API_KEY environment variable not set"
  assert_output_contains "Generate your API key at"
}

@test "n8n-api.sh list-workflows returns workflow list" {
  run scripts/n8n-api.sh list-workflows
  [ "$status" -eq 0 ]
  assert_output_contains '"data"'
  assert_output_contains '"id"'
  assert_output_contains '"name"'
}

@test "n8n-api.sh get-workflow requires workflow ID" {
  run scripts/n8n-api.sh get-workflow
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "get-workflow [workflow-id]"
}

@test "n8n-api.sh get-workflow returns workflow details" {
  run scripts/n8n-api.sh get-workflow test-workflow-1
  [ "$status" -eq 0 ]
  assert_output_contains '"id"'
  assert_output_contains '"name"'
  assert_output_contains '"nodes"'
}

@test "n8n-api.sh create-workflow requires JSON file" {
  run scripts/n8n-api.sh create-workflow
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "create-workflow [json-file]"
}

@test "n8n-api.sh create-workflow validates file exists" {
  run scripts/n8n-api.sh create-workflow nonexistent.json
  [ "$status" -eq 1 ]
  assert_output_contains "Error: File nonexistent.json not found"
}

@test "n8n-api.sh create-workflow validates JSON syntax" {
  echo "invalid json" > "$TEST_TEMP_DIR/invalid.json"
  
  # Mock jq to fail on invalid JSON
  jq() {
    if [[ "$1" == "empty" ]]; then
      return 4  # Invalid JSON
    fi
  }
  export -f jq
  
  run scripts/n8n-api.sh create-workflow "$TEST_TEMP_DIR/invalid.json"
  [ "$status" -eq 1 ]
  assert_output_contains "Error: Invalid JSON"
}

@test "n8n-api.sh create-workflow creates workflow successfully" {
  create_test_workflow "$TEST_TEMP_DIR/test.json"
  
  run scripts/n8n-api.sh create-workflow "$TEST_TEMP_DIR/test.json"
  [ "$status" -eq 0 ]
  assert_output_contains '"id"'
  assert_output_contains '"name"'
}

@test "n8n-api.sh create-workflow handles API errors" {
  create_test_workflow "$TEST_TEMP_DIR/test.json"
  
  # Mock curl to return an error
  curl() {
    echo '{"message": "request/body must NOT have additional properties"}'
  }
  export -f curl
  
  run scripts/n8n-api.sh create-workflow "$TEST_TEMP_DIR/test.json"
  [ "$status" -eq 1 ]
  assert_output_contains "Error: request/body must NOT have additional properties"
}

@test "n8n-api.sh update-workflow requires workflow ID and file" {
  run scripts/n8n-api.sh update-workflow
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "update-workflow [workflow-id] [json-file]"
}

@test "n8n-api.sh update-workflow updates workflow" {
  create_test_workflow "$TEST_TEMP_DIR/test.json"
  
  run scripts/n8n-api.sh update-workflow test-workflow-1 "$TEST_TEMP_DIR/test.json"
  [ "$status" -eq 0 ]
  assert_output_contains '"id"'
}

@test "n8n-api.sh delete-workflow requires workflow ID" {
  run scripts/n8n-api.sh delete-workflow
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "delete-workflow [workflow-id]"
}

@test "n8n-api.sh delete-workflow deletes workflow" {
  run scripts/n8n-api.sh delete-workflow test-workflow-1
  [ "$status" -eq 0 ]
  assert_output_contains '"success"'
}

@test "n8n-api.sh activate-workflow requires workflow ID" {
  run scripts/n8n-api.sh activate-workflow
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "activate-workflow [workflow-id]"
}

@test "n8n-api.sh activate-workflow activates workflow" {
  # Mock activation response
  curl() {
    if [[ "$*" =~ "activate" ]]; then
      echo '{"active": true}'
    else
      mock_curl "$@"
    fi
  }
  export -f curl
  
  run scripts/n8n-api.sh activate-workflow test-workflow-1
  [ "$status" -eq 0 ]
  assert_output_contains '"active": true'
}

@test "n8n-api.sh deactivate-workflow deactivates workflow" {
  # Mock deactivation response
  curl() {
    if [[ "$*" =~ "deactivate" ]]; then
      echo '{"active": false}'
    else
      mock_curl "$@"
    fi
  }
  export -f curl
  
  run scripts/n8n-api.sh deactivate-workflow test-workflow-1
  [ "$status" -eq 0 ]
  assert_output_contains '"active": false'
}

@test "n8n-api.sh list-credentials returns credentials list" {
  run scripts/n8n-api.sh list-credentials
  [ "$status" -eq 0 ]
  assert_output_contains '"data"'
  assert_output_contains '"id"'
  assert_output_contains '"name"'
  assert_output_contains '"type"'
}

@test "n8n-api.sh get-credential requires credential ID" {
  run scripts/n8n-api.sh get-credential
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "get-credential [credential-id]"
}

@test "n8n-api.sh create-credential requires JSON file" {
  run scripts/n8n-api.sh create-credential
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "create-credential [json-file]"
}

@test "n8n-api.sh test-credential requires credential ID" {
  run scripts/n8n-api.sh test-credential
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "test-credential [credential-id]"
}

@test "n8n-api.sh list-executions lists executions" {
  # Mock executions response
  curl() {
    if [[ "$*" =~ "executions" ]]; then
      echo '{"data": [{"id": "exec-1", "status": "success"}]}'
    else
      mock_curl "$@"
    fi
  }
  export -f curl
  
  run scripts/n8n-api.sh list-executions
  [ "$status" -eq 0 ]
  assert_output_contains '"data"'
  assert_output_contains '"status"'
}

@test "n8n-api.sh get-execution requires execution ID" {
  run scripts/n8n-api.sh get-execution
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "get-execution [execution-id]"
}

@test "n8n-api.sh raw command allows custom API calls" {
  run scripts/n8n-api.sh raw GET workflows
  [ "$status" -eq 0 ]
  assert_output_contains '"data"'
}

@test "n8n-api.sh raw command requires method and endpoint" {
  run scripts/n8n-api.sh raw
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "raw [method] [endpoint]"
}

@test "n8n-api.sh shows help with unknown command" {
  run scripts/n8n-api.sh unknown-command
  [ "$status" -eq 0 ]
  assert_output_contains "n8n API Wrapper"
  assert_output_contains "WORKFLOW OPERATIONS"
  assert_output_contains "CREDENTIAL OPERATIONS"
  assert_output_contains "EXECUTION OPERATIONS"
}

@test "n8n-api.sh shows help with help command" {
  run scripts/n8n-api.sh help
  [ "$status" -eq 0 ]
  assert_output_contains "n8n API Operations"
  assert_output_contains "Examples:"
}