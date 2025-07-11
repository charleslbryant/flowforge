# FlowForge Testing Guide

## Overview

FlowForge uses [Bats (Bash Automated Testing System)](https://github.com/bats-core/bats-core) for comprehensive shell script testing. Our test suite ensures reliability across all core functionality including CLI operations, API interactions, and end-to-end workflow creation.

## Test Architecture

### Test Structure
```
tests/
├── test_forge.bats              # Main CLI tests (35+ tests)
├── test_n8n_api.bats           # API wrapper tests (25+ tests)
├── test_workflow_creation.bats  # End-to-end tests (15+ tests)
├── helpers/
│   ├── setup.bash              # Test environment & utilities
│   └── mocks.bash              # Mock functions for dependencies
└── fixtures/
    ├── sample_workflows.json   # Test workflow data
    ├── mock_responses.json     # API response mocks
    └── test_credentials.json   # Credential test data
```

### Test Categories

1. **Unit Tests** - Individual script functionality
2. **Integration Tests** - Script interactions
3. **End-to-End Tests** - Complete workflow scenarios
4. **Mock Tests** - External dependency simulation

## Running Tests

### Quick Start
```bash
# Install test dependencies
make test-install

# Run all tests
make test

# Run with verbose output
make test-verbose
```

### Specific Test Suites
```bash
# Test main CLI functionality
make test-forge

# Test n8n API wrapper
make test-api

# Test workflow creation flow
make test-workflow
```

### Development Workflow
```bash
# Watch mode - runs tests on file changes
make test-watch

# Clean test artifacts
make test-clean
```

## Test Environment

### Setup
Each test runs in an isolated environment:
- **Temporary directories** for file operations
- **Environment variables** for configuration
- **Mock functions** to simulate external dependencies
- **Cleanup routines** to prevent test pollution

### Mock System
Our comprehensive mock system simulates:

#### External Commands
- `curl` - n8n API responses
- `claude` - AI workflow generation
- `jq` - JSON processing
- `pgrep/pkill` - Process management

#### System Operations
- File system operations (`mktemp`, file creation)
- Network checks (`nc`, port availability)
- Browser opening (`xdg-open`)

#### n8n API Responses
- Workflow CRUD operations
- Credential management
- Execution monitoring
- Error scenarios

## Writing Tests

### Basic Test Structure
```bash
#!/usr/bin/env bats

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

@test "descriptive test name" {
  # Arrange
  mock_n8n_process running
  
  # Act
  run scripts/forge health
  
  # Assert
  [ "$status" -eq 0 ]
  assert_output_contains "n8n process running"
}
```

### Test Patterns

#### Testing Success Scenarios
```bash
@test "forge create-workflow generates workflow successfully" {
  mock_n8n_process running
  mock_claude_cli success
  
  run scripts/forge create-workflow "test workflow"
  
  [ "$status" -eq 0 ]
  assert_output_contains "Workflow created successfully"
  assert_file_exists "workflow.json"
}
```

#### Testing Error Scenarios
```bash
@test "forge create-workflow handles API errors" {
  mock_n8n_process running
  mock_claude_cli success
  
  # Mock API returning error
  curl() {
    echo '{"message": "Workflow name exists"}'
  }
  export -f curl
  
  run scripts/forge create-workflow "test"
  
  [ "$status" -eq 1 ]
  assert_output_contains "Workflow name exists"
}
```

#### Testing File Operations
```bash
@test "script validates required files exist" {
  create_test_workflow "test.json"
  
  run scripts/n8n-api.sh create-workflow "test.json"
  
  [ "$status" -eq 0 ]
  assert_file_exists "test.json"
}
```

### Helper Functions

#### Assertions
```bash
assert_output_contains "expected text"      # Output contains text
assert_output_not_contains "unwanted"      # Output doesn't contain text
assert_file_exists "path/to/file"          # File exists
assert_file_not_exists "path/to/file"      # File doesn't exist
assert_directory_exists "path/to/dir"      # Directory exists
```

#### Test Data
```bash
create_test_workflow "filename.json"       # Creates sample workflow
create_mock_executable "name" 0 "output"   # Creates mock command
wait_for_condition "condition" 30          # Waits for condition
```

#### Environment
```bash
setup_test_environment()                   # Sets up isolated test env
teardown_test_environment()                # Cleans up test env
setup_common_mocks()                       # Sets up standard mocks
reset_mocks()                              # Removes all mocks
```

## Test Coverage

### Current Coverage

#### scripts/forge (35+ tests)
- ✅ Help and usage display
- ✅ System health checks
- ✅ n8n process management (start/stop/restart)
- ✅ System diagnostics
- ✅ Workflow validation and formatting
- ✅ Workflow creation from prompts
- ✅ Error handling and edge cases

#### scripts/n8n-api.sh (25+ tests)
- ✅ API key validation
- ✅ Workflow CRUD operations
- ✅ Credential management
- ✅ Execution monitoring
- ✅ Error handling and validation
- ✅ JSON processing and formatting

#### End-to-End Workflows (15+ tests)
- ✅ Complete workflow creation flow
- ✅ Template system integration
- ✅ Claude AI integration
- ✅ JSON cleanup and validation
- ✅ Browser integration
- ✅ Concurrent operation handling

### Coverage Goals
- **Unit Test Coverage**: >90% of script functions
- **Integration Coverage**: All script interactions
- **Error Coverage**: All error conditions
- **Edge Case Coverage**: Boundary conditions and invalid inputs

## Continuous Integration

### GitHub Actions Integration
```yaml
name: Test Suite
on: [push, pull_request]
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Install Dependencies
        run: make test-install
      - name: Run Tests
        run: make test
      - name: Upload Results
        uses: actions/upload-artifact@v3
        with:
          name: test-results
          path: test-results/
```

### Pre-commit Hooks
```bash
#!/bin/bash
# .git/hooks/pre-commit
make test
if [ $? -ne 0 ]; then
  echo "Tests failed. Commit aborted."
  exit 1
fi
```

## Debugging Tests

### Verbose Output
```bash
# Run single test with full output
bats --verbose-run tests/test_forge.bats

# Debug specific test
bats --verbose-run -f "test name pattern" tests/
```

### Test Isolation
```bash
# Run single test function
bats -f "specific test name" tests/test_forge.bats

# Skip specific tests
bats -f "!skip_this_test" tests/
```

### Manual Debugging
```bash
# Set debug mode in test
setup() {
  set -x  # Enable debug output
  setup_test_environment
}

# Check test artifacts
ls -la test_temp_*/
cat workflow.json
echo "$output"
```

## Performance Testing

### Test Execution Time
```bash
# Time test suite
time make test

# Profile individual tests
time bats tests/test_forge.bats
```

### Resource Usage
```bash
# Monitor during tests
htop
iostat 1

# Check for resource leaks
ps aux | grep -E "(n8n|claude|bats)"
```

## Best Practices

### Test Design
1. **Isolation** - Each test should be independent
2. **Deterministic** - Tests should produce consistent results
3. **Fast** - Mock external dependencies
4. **Readable** - Clear test names and structure
5. **Maintainable** - Use helper functions and shared fixtures

### Naming Conventions
```bash
# Good test names
@test "forge health checks n8n status"
@test "forge create-workflow requires description"
@test "n8n-api.sh validates JSON before sending"

# Avoid generic names
@test "test forge"
@test "check api"
```

### Mock Strategy
1. **Mock external dependencies** (APIs, processes, files)
2. **Don't mock the code under test**
3. **Use realistic mock responses**
4. **Test both success and failure scenarios**

### Error Testing
```bash
# Test all error conditions
@test "handles missing API key"
@test "handles invalid JSON"
@test "handles network failures"
@test "handles permission errors"
```

## Troubleshooting

### Common Issues

#### Tests Fail in CI but Pass Locally
- Check environment differences
- Verify all dependencies are installed
- Check file permissions
- Review path differences

#### Mock Functions Not Working
```bash
# Verify function export
export -f mock_function

# Check if mock is being called
mock_function() {
  echo "Mock called with: $*" >&2
  # ... rest of mock
}
```

#### Test Cleanup Issues
```bash
# Manual cleanup
make test-clean
rm -rf test_temp_* mock_temp_*

# Check for leftover processes
ps aux | grep -E "(n8n|claude)"
```

#### Permission Errors
```bash
# Fix test file permissions
chmod +x tests/*.bats
chmod +x tests/helpers/*.bash
```

### Getting Help

1. **Check test output** - Bats provides detailed failure information
2. **Use verbose mode** - `make test-verbose` for more details
3. **Isolate failing tests** - Run individual test files
4. **Check mock setup** - Verify mocks are properly configured
5. **Review test logs** - Check for error messages and stack traces

## Contributing

### Adding New Tests
1. Choose appropriate test file (`test_forge.bats`, `test_n8n_api.bats`, etc.)
2. Follow existing patterns and naming conventions
3. Add necessary mocks and fixtures
4. Test both success and failure scenarios
5. Update documentation if needed

### Test Review Checklist
- [ ] Tests are isolated and independent
- [ ] Both success and failure cases are covered
- [ ] Mocks are realistic and appropriate
- [ ] Test names are descriptive
- [ ] Cleanup is properly handled
- [ ] Performance impact is minimal

---

*This testing guide ensures FlowForge maintains high quality and reliability across all functionality. For questions or contributions, please refer to the project's contribution guidelines.*