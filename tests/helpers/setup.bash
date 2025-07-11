#!/bin/bash
# Test setup helpers for FlowForge

# Common setup for all tests
setup_test_environment() {
  # Create temporary directory for test
  TEST_TEMP_DIR=$(mktemp -d)
  export TEST_TEMP_DIR
  
  # Save original working directory
  export ORIGINAL_DIR=$(pwd)
  
  # Set test environment variables
  export N8N_API_KEY="test-api-key-12345"
  export N8N_HOST="http://localhost:5678"
  export N8N_PORT="5678"
  
  # Disable actual n8n operations in tests
  export FLOWFORGE_TEST_MODE=1
}

# Common teardown for all tests
teardown_test_environment() {
  # Clean up temporary directory
  if [[ -n "$TEST_TEMP_DIR" && -d "$TEST_TEMP_DIR" ]]; then
    rm -rf "$TEST_TEMP_DIR"
  fi
  
  # Return to original directory
  if [[ -n "$ORIGINAL_DIR" ]]; then
    cd "$ORIGINAL_DIR"
  fi
  
  # Unset test environment variables
  unset N8N_API_KEY N8N_HOST N8N_PORT FLOWFORGE_TEST_MODE TEST_TEMP_DIR
}

# Check if command exists
command_exists() {
  command -v "$1" >/dev/null 2>&1
}

# Skip test if dependency is missing
skip_if_missing() {
  local cmd="$1"
  local message="$2"
  
  if ! command_exists "$cmd"; then
    skip "${message:-"$cmd is not installed"}"
  fi
}

# Assert that output contains expected text
assert_output_contains() {
  local expected="$1"
  if [[ "$output" != *"$expected"* ]]; then
    echo "Expected output to contain: '$expected'"
    echo "Actual output: '$output'"
    return 1
  fi
}

# Assert that output does not contain text
assert_output_not_contains() {
  local unexpected="$1"
  if [[ "$output" == *"$unexpected"* ]]; then
    echo "Expected output to NOT contain: '$unexpected'"
    echo "Actual output: '$output'"
    return 1
  fi
}

# Assert that file exists
assert_file_exists() {
  local file="$1"
  if [[ ! -f "$file" ]]; then
    echo "Expected file to exist: '$file'"
    return 1
  fi
}

# Assert that file does not exist
assert_file_not_exists() {
  local file="$1"
  if [[ -f "$file" ]]; then
    echo "Expected file to NOT exist: '$file'"
    return 1
  fi
}

# Assert that directory exists
assert_directory_exists() {
  local dir="$1"
  if [[ ! -d "$dir" ]]; then
    echo "Expected directory to exist: '$dir'"
    return 1
  fi
}

# Create a mock executable that returns specific output
create_mock_executable() {
  local name="$1"
  local return_code="${2:-0}"
  local output="$3"
  
  cat > "$TEST_TEMP_DIR/$name" << EOF
#!/bin/bash
echo "$output"
exit $return_code
EOF
  chmod +x "$TEST_TEMP_DIR/$name"
  export PATH="$TEST_TEMP_DIR:$PATH"
}

# Create a test workflow JSON file
create_test_workflow() {
  local filename="${1:-test_workflow.json}"
  cat > "$filename" << 'EOF'
{
  "name": "Test Workflow",
  "nodes": [
    {
      "parameters": {},
      "id": "start-node",
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
EOF
}

# Wait for condition with timeout
wait_for_condition() {
  local condition="$1"
  local timeout="${2:-30}"
  local interval="${3:-1}"
  local elapsed=0
  
  while ! eval "$condition"; do
    if [[ $elapsed -ge $timeout ]]; then
      echo "Timeout waiting for condition: $condition"
      return 1
    fi
    sleep "$interval"
    elapsed=$((elapsed + interval))
  done
}

# Check if port is available
port_available() {
  local port="$1"
  ! nc -z localhost "$port" 2>/dev/null
}

# Generate random port for testing
get_random_port() {
  python3 -c "import socket; s=socket.socket(); s.bind(('', 0)); print(s.getsockname()[1]); s.close()"
}