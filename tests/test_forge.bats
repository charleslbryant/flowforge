#!/usr/bin/env bats
# Tests for scripts/forge

# Load test helpers
load helpers/setup
load helpers/mocks

setup() {
  setup_test_environment
  setup_common_mocks
}

teardown() {
  reset_mocks
  teardown_test_environment
}

@test "forge shows help when no command provided" {
  run scripts/forge
  [ "$status" -eq 0 ]
  assert_output_contains "FlowForge CLI"
  assert_output_contains "Usage:"
  assert_output_contains "Commands:"
}

@test "forge shows help with help command" {
  run scripts/forge help
  [ "$status" -eq 0 ]
  assert_output_contains "FlowForge CLI"
  assert_output_contains "Examples:"
}

@test "forge health checks n8n status" {
  mock_n8n_process running
  
  run scripts/forge health
  [ "$status" -eq 0 ]
  assert_output_contains "n8n process running"
  assert_output_contains "n8n port 5678 accessible"
  assert_output_contains "n8n API authentication working"
}

@test "forge health fails when n8n not running" {
  mock_n8n_process not_running
  
  run scripts/forge health
  [ "$status" -eq 1 ]
  assert_output_contains "n8n process not found"
}

@test "forge doctor shows system diagnostic" {
  run scripts/forge doctor
  [ "$status" -eq 0 ]
  assert_output_contains "FlowForge System Check"
  assert_output_contains "node installed"
  assert_output_contains "npm installed"
  assert_output_contains "jq installed"
  assert_output_contains "claude CLI installed"
}

@test "forge doctor fails with missing dependencies" {
  # Mock missing commands
  command() {
    if [[ "$1" == "-v" ]]; then
      return 1  # Command not found
    fi
  }
  export -f command
  
  run scripts/forge doctor
  [ "$status" -eq 1 ]
  assert_output_contains "Missing dependencies"
}

@test "forge start starts n8n successfully" {
  mock_n8n_process not_running
  mock_n8n_process start
  
  run scripts/forge start
  [ "$status" -eq 0 ]
  assert_output_contains "Starting n8n"
  assert_output_contains "n8n started successfully"
}

@test "forge start handles already running n8n" {
  mock_n8n_process running
  
  run scripts/forge start
  [ "$status" -eq 0 ]
  assert_output_contains "n8n is already running"
}

@test "forge stop stops n8n successfully" {
  mock_n8n_process running
  mock_n8n_process stop
  
  run scripts/forge stop
  [ "$status" -eq 0 ]
  assert_output_contains "Stopping n8n"
  assert_output_contains "n8n stopped"
}

@test "forge stop handles not running n8n" {
  mock_n8n_process not_running
  
  run scripts/forge stop
  [ "$status" -eq 0 ]
  assert_output_contains "n8n is not running"
}

@test "forge restart restarts n8n" {
  mock_n8n_process running
  mock_n8n_process stop
  mock_n8n_process start
  
  run scripts/forge restart
  [ "$status" -eq 0 ]
  assert_output_contains "Restarting n8n"
  assert_output_contains "n8n restarted successfully"
}

@test "forge validate validates workflow.json" {
  create_test_workflow "workflow.json"
  
  run scripts/forge validate
  [ "$status" -eq 0 ]
  assert_output_contains "Validating workflow.json"
}

@test "forge format formats workflow.json" {
  create_test_workflow "workflow.json"
  
  run scripts/forge format
  [ "$status" -eq 0 ]
  assert_output_contains "Formatting workflow.json"
}

@test "forge create-workflow requires description" {
  run scripts/forge create-workflow
  [ "$status" -eq 1 ]
  assert_output_contains "Usage:"
  assert_output_contains "workflow description"
}

@test "forge create-workflow checks n8n health first" {
  mock_n8n_process not_running
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 1 ]
  assert_output_contains "n8n is not healthy"
}

@test "forge create-workflow generates and creates workflow" {
  mock_n8n_process running
  mock_claude_cli success
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 0 ]
  assert_output_contains "Generating workflow from prompt"
  assert_output_contains "Workflow generated and validated"
  assert_output_contains "Creating workflow in n8n"
  assert_output_contains "Workflow created successfully"
}

@test "forge create-workflow handles Claude generation failure" {
  mock_n8n_process running
  mock_claude_cli error
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 1 ]
  assert_output_contains "Failed to generate workflow with Claude"
}

@test "forge create-workflow handles invalid JSON from Claude" {
  mock_n8n_process running
  mock_claude_cli invalid_json
  mock_jq invalid_json
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 1 ]
  assert_output_contains "Generated workflow is not valid JSON"
}

@test "forge create-workflow cleans markdown wrapped JSON" {
  mock_n8n_process running
  
  # Create a workflow with markdown wrapper
  claude() {
    if [[ "$1" == "--print" ]]; then
      cat << 'EOF'
Here's your workflow:
```json
{
  "name": "Test Workflow",
  "nodes": []
}
```
EOF
    fi
  }
  export -f claude
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 0 ]
  
  # Check that the workflow.json was cleaned
  assert_file_exists "workflow.json"
  run cat workflow.json
  assert_output_not_contains "\`\`\`json"
  assert_output_not_contains "Here's your workflow"
}

@test "forge create-workflow opens browser on success" {
  mock_n8n_process running
  mock_claude_cli success
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 0 ]
  assert_output_contains "Opening workflow in browser"
}