#!/bin/bash
# Mock functions for FlowForge tests

# Mock curl to simulate n8n API responses
mock_curl() {
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

# Mock n8n process management
mock_n8n_process() {
  local action="$1"
  
  case "$action" in
    running)
      # Mock pgrep to return a fake PID
      pgrep() {
        if [[ "$1" == "-f" && "$2" == "n8n" ]]; then
          echo "12345"
        fi
      }
      export -f pgrep
      ;;
    not_running)
      # Mock pgrep to return nothing
      pgrep() {
        return 1
      }
      export -f pgrep
      ;;
    start)
      # Mock successful n8n start
      nohup() {
        echo "n8n started successfully" > "$TEST_TEMP_DIR/n8n.log"
        return 0
      }
      export -f nohup
      ;;
    stop)
      # Mock successful n8n stop
      pkill() {
        if [[ "$1" == "-f" && "$2" == "n8n" ]]; then
          return 0
        fi
      }
      export -f pkill
      ;;
  esac
}

# Mock network tools
mock_network_tools() {
  # Mock netcat (nc) for port checking
  nc() {
    if [[ "$1" == "-z" ]]; then
      # Mock port check - return success (port is open)
      return 0
    fi
  }
  export -f nc
  
  # Mock curl for health checks
  curl() {
    mock_curl "$@"
  }
  export -f curl
}

# Mock Claude CLI
mock_claude_cli() {
  local response_type="${1:-success}"
  
  claude() {
    if [[ "$1" == "--print" ]]; then
      case "$response_type" in
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
  export -f claude
}

# Mock jq for JSON processing
mock_jq() {
  local behavior="${1:-success}"
  
  jq() {
    case "$behavior" in
      success)
        if [[ "$1" == "empty" ]]; then
          return 0  # Valid JSON
        elif [[ "$1" == "." ]]; then
          cat  # Pretty print
        elif [[ "$1" == "-r" && "$2" == ".id" ]]; then
          echo "mock-id-123"
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
  export -f jq
}

# Mock file system operations
mock_filesystem() {
  # Mock mktemp
  mktemp() {
    if [[ "$1" == "-d" ]]; then
      echo "$TEST_TEMP_DIR/mock_temp_dir"
      mkdir -p "$TEST_TEMP_DIR/mock_temp_dir"
    else
      echo "$TEST_TEMP_DIR/mock_temp_file"
      touch "$TEST_TEMP_DIR/mock_temp_file"
    fi
  }
  export -f mktemp
}

# Mock system commands
mock_system_commands() {
  # Mock command existence checks
  command() {
    if [[ "$1" == "-v" ]]; then
      case "$2" in
        node|npm|jq|curl|nc|claude|bats)
          echo "/usr/bin/$2"
          ;;
        *)
          return 1
          ;;
      esac
    fi
  }
  export -f command
  
  # Mock xdg-open for browser opening
  xdg-open() {
    echo "Opening: $1"
  }
  export -f xdg-open
}

# Reset all mocks
reset_mocks() {
  unset -f curl pgrep nohup pkill nc claude jq mktemp command xdg-open
}

# Setup common mocks for most tests
setup_common_mocks() {
  mock_network_tools
  mock_claude_cli
  mock_jq
  mock_filesystem
  mock_system_commands
}

# Setup mocks for API tests
setup_api_mocks() {
  setup_common_mocks
  mock_n8n_process running
}

# Setup mocks for failing scenarios
setup_failing_mocks() {
  mock_n8n_process not_running
  mock_claude_cli error
  mock_jq invalid_json
}