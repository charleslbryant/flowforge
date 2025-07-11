#!/bin/bash
# Mock functions for FlowForge tests

# Mock curl to simulate n8n API responses
curl() {
  # Parse curl arguments to determine what to mock
  local url=""
  local method="GET"
  local data=""
  local headers=()
  
  # Parse arguments
  while [[ $# -gt 0 ]]; do
    case $1 in
      -X)
        method="$2"
        shift 2
        ;;
      -H)
        headers+=("$2")
        shift 2
        ;;
      -d)
        data="$2"
        shift 2
        ;;
      http*)
        url="$1"
        shift
        ;;
      *)
        shift
        ;;
    esac
  done
  
  # Mock different API endpoints
  case "$url" in
    */api/v1/workflows)
      if [[ "$method" == "GET" ]]; then
        echo '{"data": [{"id": "test-workflow-1", "name": "Test Workflow", "active": false}]}'
      elif [[ "$method" == "POST" ]]; then
        echo '{"id": "new-workflow-123", "name": "New Workflow", "active": false}'
      fi
      ;;
    */api/v1/workflows/*)
      if [[ "$method" == "GET" ]]; then
        echo '{"id": "test-workflow-1", "name": "Test Workflow", "active": false, "nodes": []}'
      elif [[ "$method" == "DELETE" ]]; then
        echo '{"success": true}'
      fi
      ;;
    */api/v1/credentials)
      echo '{"data": [{"id": "test-cred-1", "name": "Test Credential", "type": "httpBasicAuth"}]}'
      ;;
    */healthz)
      echo "OK"
      ;;
    *)
      echo "Mock curl: unknown URL $url" >&2
      return 1
      ;;
  esac
}

# Mock n8n process management functions
pgrep() {
  if [[ "${N8N_PROCESS_RUNNING:-true}" == "true" ]]; then
    if [[ "$1" == "-f" && "$2" == "n8n" ]]; then
      echo "12345"
      return 0
    fi
  fi
  return 1
}

pkill() {
  if [[ "$1" == "-f" && "$2" == "n8n" ]]; then
    return 0
  fi
  return 1
}

nohup() {
  if [[ -n "$TEST_TEMP_DIR" ]]; then
    echo "n8n started successfully" > "$TEST_TEMP_DIR/n8n.log"
  fi
  return 0
}

# Mock network tools
nc() {
  if [[ "$1" == "-z" ]]; then
    # Mock port check - return success (port is open)
    return 0
  fi
}

# Mock Claude CLI
claude() {
  if [[ "$1" == "--print" ]]; then
    case "${CLAUDE_RESPONSE_TYPE:-success}" in
      success)
        create_test_workflow
        ;;
      error)
        echo "Error: Claude API failed" >&2
        return 1
        ;;
      invalid_json)
        echo "This is not valid JSON"
        ;;
    esac
  else
    echo "Claude CLI mock"
  fi
}

# Mock jq for JSON processing
jq() {
  local behavior="${JQ_BEHAVIOR:-success}"
  
  case "$behavior" in
    success)
      if [[ "$1" == "empty" ]]; then
        return 0  # Valid JSON
      elif [[ "$1" == "." ]]; then
        cat  # Pretty print
      elif [[ "$1" == "-r" && "$2" == ".id" ]]; then
        echo "mock-id-123"
      elif [[ "$1" == "-e" && "$2" == ".id" ]]; then
        return 0  # ID exists
      else
        echo "mock-jq-output"
      fi
      ;;
    invalid_json)
      if [[ "$1" == "empty" ]]; then
        return 4  # Invalid JSON
      fi
      ;;
  esac
}

# Mock file system operations
mktemp() {
  if [[ "$1" == "-d" ]]; then
    local dir="$TEST_TEMP_DIR/mock_temp_dir_$$"
    mkdir -p "$dir"
    echo "$dir"
  else
    local file="$TEST_TEMP_DIR/mock_temp_file_$$"
    touch "$file"
    echo "$file"
  fi
}

# Mock system commands
command() {
  if [[ "$1" == "-v" ]]; then
    case "$2" in
      node|npm|jq|curl|nc|claude|bats)
        echo "/usr/bin/$2"
        return 0
        ;;
      *)
        return 1
        ;;
    esac
  fi
}

xdg-open() {
  echo "Opening: $1"
  return 0
}

# Reset all mocks
reset_mocks() {
  unset -f curl pgrep nohup pkill nc claude jq mktemp command xdg-open
}

# Setup common mocks for most tests
setup_common_mocks() {
  export -f curl
  export -f jq
  export -f claude
  export -f pgrep
  export -f pkill
  export -f nc
  export -f command
  export -f xdg-open
  export -f mktemp
}

# Setup mocks for API tests
setup_api_mocks() {
  setup_common_mocks
  export N8N_PROCESS_RUNNING=true
}

# Setup mocks for failing scenarios
setup_failing_mocks() {
  export N8N_PROCESS_RUNNING=false
  export CLAUDE_RESPONSE_TYPE=error
  export JQ_BEHAVIOR=invalid_json
}