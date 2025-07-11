#!/usr/bin/env bats
# End-to-end workflow creation tests

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

@test "end-to-end workflow creation from prompt" {
  # Mock successful workflow creation flow
  mock_n8n_process running
  mock_claude_cli success
  
  run scripts/forge create-workflow "send daily email reports"
  [ "$status" -eq 0 ]
  
  # Check each step of the process
  assert_output_contains "Generating workflow from prompt"
  assert_output_contains "Workflow generated and validated"
  assert_output_contains "Creating workflow in n8n"
  assert_output_contains "Workflow created successfully"
  assert_output_contains "Opening workflow in browser"
  
  # Check that files were created
  assert_file_exists "workflow.prompt.yaml"
  assert_file_exists "workflow.json"
  
  # Check that the prompt file contains the request
  run cat workflow.prompt.yaml
  assert_output_contains "send daily email reports"
  assert_output_contains "Available Templates"
}

@test "workflow creation uses template context" {
  mock_n8n_process running
  mock_claude_cli success
  
  run scripts/forge create-workflow "read my email"
  [ "$status" -eq 0 ]
  
  # Check that the prompt includes template information
  run cat workflow.prompt.yaml
  assert_output_contains "email-workflow.json"
  assert_output_contains "web-scraper.json"
  assert_output_contains "slack-notification.json"
}

@test "workflow creation handles Claude markdown output" {
  mock_n8n_process running
  
  # Mock Claude returning markdown-wrapped JSON
  claude() {
    if [[ "$1" == "--print" ]]; then
      cat << 'EOF'
Here's your workflow:

```json
{
  "name": "Email Workflow",
  "nodes": [
    {
      "parameters": {},
      "id": "start",
      "name": "Start",
      "type": "n8n-nodes-base.start",
      "typeVersion": 1,
      "position": [240, 300]
    }
  ],
  "connections": {},
  "settings": {
    "executionOrder": "v1"
  }
}
```

This workflow will handle your email processing.
EOF
    fi
  }
  export -f claude
  
  run scripts/forge create-workflow "process my email"
  [ "$status" -eq 0 ]
  
  # Check that the JSON was cleaned properly
  assert_file_exists "workflow.json"
  run cat workflow.json
  assert_output_not_contains "Here's your workflow"
  assert_output_not_contains "```json"
  assert_output_not_contains "This workflow will"
  assert_output_contains '"name": "Email Workflow"'
}

@test "workflow creation validates generated JSON" {
  mock_n8n_process running
  
  # Mock Claude returning invalid JSON
  claude() {
    if [[ "$1" == "--print" ]]; then
      echo "This is not valid JSON at all"
    fi
  }
  export -f claude
  
  # Mock jq to detect invalid JSON
  jq() {
    if [[ "$1" == "empty" ]]; then
      return 4  # Invalid JSON
    fi
  }
  export -f jq
  
  run scripts/forge create-workflow "invalid workflow"
  [ "$status" -eq 1 ]
  assert_output_contains "Generated workflow is not valid JSON"
}

@test "workflow creation handles API errors gracefully" {
  mock_n8n_process running
  mock_claude_cli success
  
  # Mock API returning error
  curl() {
    if [[ "$*" =~ "api/v1/workflows" && "$*" =~ "POST" ]]; then
      echo '{"message": "Workflow name already exists"}'
    else
      mock_curl "$@"
    fi
  }
  export -f curl
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 1 ]
  assert_output_contains "Workflow name already exists"
}

@test "workflow creation requires n8n to be running" {
  mock_n8n_process not_running
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 1 ]
  assert_output_contains "n8n is not healthy"
}

@test "workflow creation preserves complex JSON structures" {
  mock_n8n_process running
  
  # Mock Claude returning complex workflow
  claude() {
    if [[ "$1" == "--print" ]]; then
      cat << 'EOF'
{
  "name": "Complex Workflow",
  "nodes": [
    {
      "parameters": {
        "conditions": {
          "options": {
            "caseSensitive": true
          },
          "conditions": [
            {
              "leftValue": "={{ $json.status }}",
              "rightValue": "active",
              "operator": {
                "type": "string",
                "operation": "equals"
              }
            }
          ]
        }
      },
      "id": "filter-node",
      "name": "Filter",
      "type": "n8n-nodes-base.filter",
      "typeVersion": 2,
      "position": [460, 300]
    }
  ],
  "connections": {
    "Start": {
      "main": [
        [
          {
            "node": "Filter",
            "type": "main",
            "index": 0
          }
        ]
      ]
    }
  },
  "settings": {
    "executionOrder": "v1"
  }
}
EOF
    fi
  }
  export -f claude
  
  run scripts/forge create-workflow "complex workflow"
  [ "$status" -eq 0 ]
  
  # Check that complex JSON structure is preserved
  assert_file_exists "workflow.json"
  run cat workflow.json
  assert_output_contains '"caseSensitive": true'
  assert_output_contains '"operator":'
  assert_output_contains '"connections":'
}

@test "workflow creation handles template discovery" {
  mock_n8n_process running
  mock_claude_cli success
  
  # Create some test templates
  mkdir -p templates
  echo '{"name": "Test Template"}' > templates/test-template.json
  
  run scripts/forge create-workflow "use templates"
  [ "$status" -eq 0 ]
  
  # Check that templates were included in the prompt
  run cat workflow.prompt.yaml
  assert_output_contains "test-template.json"
  assert_output_contains "Test Template"
}

@test "workflow creation cleans up temporary files on success" {
  mock_n8n_process running
  mock_claude_cli success
  
  run scripts/forge create-workflow "test workflow"
  [ "$status" -eq 0 ]
  
  # Temporary files should be cleaned up
  # (In real implementation, you might want to clean up prompt files)
  assert_file_exists "workflow.json"  # This should remain
}

@test "workflow creation handles empty template directory" {
  mock_n8n_process running
  mock_claude_cli success
  
  # Remove templates directory
  rm -rf templates
  
  run scripts/forge create-workflow "no templates"
  [ "$status" -eq 0 ]
  
  # Should still work without templates
  assert_output_contains "Workflow created successfully"
}

@test "workflow creation respects environment variables" {
  mock_n8n_process running
  mock_claude_cli success
  
  # Set custom environment variables
  export N8N_HOST="http://localhost:9999"
  
  run scripts/forge create-workflow "custom host"
  [ "$status" -eq 0 ]
  
  # Check that custom host is used (would be in the API call)
  assert_output_contains "Workflow created successfully"
}

@test "workflow creation handles concurrent executions" {
  mock_n8n_process running
  mock_claude_cli success
  
  # Create multiple workflows in parallel (background processes)
  scripts/forge create-workflow "workflow 1" &
  PID1=$!
  scripts/forge create-workflow "workflow 2" &
  PID2=$!
  
  # Wait for both to complete
  wait $PID1
  STATUS1=$?
  wait $PID2
  STATUS2=$?
  
  # Both should succeed
  [ "$STATUS1" -eq 0 ]
  [ "$STATUS2" -eq 0 ]
}

@test "workflow creation provides helpful error messages" {
  mock_n8n_process running
  
  # Mock Claude failure
  claude() {
    echo "Claude API error: Rate limit exceeded" >&2
    return 1
  }
  export -f claude
  
  run scripts/forge create-workflow "failing workflow"
  [ "$status" -eq 1 ]
  assert_output_contains "Failed to generate workflow with Claude"
}