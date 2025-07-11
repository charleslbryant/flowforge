#!/bin/bash

# n8n API wrapper using session-based authentication
# Usage: scripts/n8n-api.sh [command] [args...]
# Commands:
#   list-workflows              - List all workflows
#   get-workflow [id]          - Get workflow by ID
#   create-workflow [file]     - Create workflow from JSON file
#   update-workflow [id] [file] - Update workflow from JSON file
#   delete-workflow [id]       - Delete workflow by ID
#   activate-workflow [id]     - Activate workflow
#   deactivate-workflow [id]   - Deactivate workflow
#   execute-workflow [id]      - Execute workflow manually
#   list-executions [id]       - List workflow executions
#   get-execution [id]         - Get execution details
#   list-nodes                 - List available node types
#   get-node [type]            - Get node type details
#   import-workflow [file]     - Import workflow from export file
#   export-workflow [id]       - Export workflow to file
#   duplicate-workflow [id]    - Duplicate workflow
#   raw [method] [endpoint] [data] - Raw API call

COMMAND=${1:-list-workflows}
shift

# n8n API key (from environment variable)
# To use this script, you must set this environment variable:
# export N8N_API_KEY="your-api-key-here"
# Generate your API key at: http://localhost:5678/settings/api

N8N_API_KEY="${N8N_API_KEY:-}"

# Check if API key is set
if [ -z "$N8N_API_KEY" ]; then
  echo "❌ N8N_API_KEY environment variable not set"
  echo "Generate your API key at: http://localhost:5678/settings/api"
  echo "Then run: export N8N_API_KEY=\"your-api-key-here\""
  exit 1
fi

# Core API call function
n8n_api_call() {
  local method=$1
  local endpoint=$2
  local data=$3
  
  if [ -n "$data" ]; then
    curl -s "http://localhost:5678/api/v1/${endpoint}" \
      -X "$method" \
      -H "X-N8N-API-KEY: $N8N_API_KEY" \
      -H "accept: application/json" \
      -H "Content-Type: application/json" \
      -d "$data"
  else
    curl -s "http://localhost:5678/api/v1/${endpoint}" \
      -X "$method" \
      -H "X-N8N-API-KEY: $N8N_API_KEY" \
      -H "accept: application/json" \
      -H "Content-Type: application/json"
  fi
}

# Command implementations
case "$COMMAND" in
  list-workflows)
    n8n_api_call GET "workflows" | jq '.'
    ;;
  get-workflow)
    if [ -z "$1" ]; then
      echo "Usage: $0 get-workflow [workflow-id]"
      exit 1
    fi
    n8n_api_call GET "workflows/$1" | jq '.'
    ;;
  create-workflow)
    if [ -z "$1" ]; then
      echo "Usage: $0 create-workflow [json-file]"
      exit 1
    fi
    if [ ! -f "$1" ]; then
      echo "Error: File $1 not found"
      exit 1
    fi
    
    # Validate JSON before sending
    if ! jq empty "$1" 2>/dev/null; then
      echo "Error: Invalid JSON in file $1"
      exit 1
    fi
    
    # Make API call and handle response
    RESPONSE=$(n8n_api_call POST "workflows" "$(cat "$1")")
    
    # Check if response contains error message
    if echo "$RESPONSE" | jq -e '.message' >/dev/null 2>&1; then
      echo "Error: $(echo "$RESPONSE" | jq -r '.message')"
      exit 1
    fi
    
    # Pretty print successful response
    echo "$RESPONSE" | jq '.'
    ;;
  update-workflow)
    if [ -z "$1" ] || [ -z "$2" ]; then
      echo "Usage: $0 update-workflow [workflow-id] [json-file]"
      exit 1
    fi
    if [ ! -f "$2" ]; then
      echo "Error: File $2 not found"
      exit 1
    fi
    n8n_api_call PUT "workflows/$1" "$(cat "$2")" | jq '.'
    ;;
  delete-workflow)
    if [ -z "$1" ]; then
      echo "Usage: $0 delete-workflow [workflow-id]"
      exit 1
    fi
    n8n_api_call DELETE "workflows/$1" | jq '.'
    ;;
  activate-workflow)
    if [ -z "$1" ]; then
      echo "Usage: $0 activate-workflow [workflow-id]"
      exit 1
    fi
    n8n_api_call POST "workflows/$1/activate" | jq '.'
    ;;
  deactivate-workflow)
    if [ -z "$1" ]; then
      echo "Usage: $0 deactivate-workflow [workflow-id]"
      exit 1
    fi
    n8n_api_call POST "workflows/$1/deactivate" | jq '.'
    ;;
  list-executions)
    if [ -z "$1" ]; then
      n8n_api_call GET "executions" | jq '.'
    else
      n8n_api_call GET "executions?workflowId=$1" | jq '.'
    fi
    ;;
  get-execution)
    if [ -z "$1" ]; then
      echo "Usage: $0 get-execution [execution-id]"
      exit 1
    fi
    n8n_api_call GET "executions/$1" | jq '.'
    ;;
  list-credentials)
    n8n_api_call GET "credentials" | jq '.'
    ;;
  get-credential)
    if [ -z "$1" ]; then
      echo "Usage: $0 get-credential [credential-id]"
      exit 1
    fi
    n8n_api_call GET "credentials/$1" | jq '.'
    ;;
  create-credential)
    if [ -z "$1" ]; then
      echo "Usage: $0 create-credential [json-file]"
      exit 1
    fi
    if [ ! -f "$1" ]; then
      echo "Error: File $1 not found"  
      exit 1
    fi
    n8n_api_call POST "credentials" "$(cat "$1")" | jq '.'
    ;;
  test-credential)
    if [ -z "$1" ]; then
      echo "Usage: $0 test-credential [credential-id]"
      exit 1
    fi
    n8n_api_call POST "credentials/$1/test" | jq '.'
    ;;
  get-workflow-status)
    if [ -z "$1" ]; then
      echo "Usage: $0 get-workflow-status [workflow-id]"
      exit 1
    fi
    n8n_api_call GET "workflows/$1/status" | jq '.'
    ;;
  raw)
    if [ -z "$1" ] || [ -z "$2" ]; then
      echo "Usage: $0 raw [method] [endpoint] [data]"
      exit 1
    fi
    n8n_api_call "$1" "$2" "$3" | jq '.'
    ;;
  help|*)
    echo "n8n API Wrapper"
    echo "Usage: $0 [command] [args...]"
    echo ""
    echo "Commands:"
    echo "  📋 WORKFLOW OPERATIONS:"
    echo "    list-workflows              - List all workflows"
    echo "    get-workflow [id]          - Get workflow by ID"  
    echo "    create-workflow [file]     - Create workflow from JSON file"
    echo "    update-workflow [id] [file] - Update workflow from JSON file"
    echo "    delete-workflow [id]       - Delete workflow by ID"
    echo "    activate-workflow [id]     - Activate workflow"
    echo "    deactivate-workflow [id]   - Deactivate workflow"
    echo ""
    echo "  📊 EXECUTION OPERATIONS:"
    echo "    list-executions [id]       - List workflow executions (optional workflow-id)"
    echo "    get-execution [id]         - Get execution details"
    echo ""
    echo "  🔐 CREDENTIAL OPERATIONS:"
    echo "    list-credentials           - List all credentials"
    echo "    get-credential [id]       - Get credential by ID"
    echo "    create-credential [file]  - Create credential from JSON file"
    echo "    test-credential [id]      - Test credential connection"
    echo ""
    echo "  🔧 UTILITY OPERATIONS:"
    echo "    get-workflow-status [id]  - Get workflow status"
    echo "    raw [method] [endpoint] [data] - Raw API call"
    echo "    help                       - Show this help"
    echo ""
    echo "  💡 All commands return JSON. Use | jq for parsing"
    ;;
esac