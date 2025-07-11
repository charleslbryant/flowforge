#!/bin/bash
set -e

N8N_HOST="http://localhost:5678"
CLAUDE_PROMPT="./workflow.prompt.yaml"

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

error_exit() {
  echo -e "${RED}❌ Error: $1${NC}" >&2
  exit 1
}

success_msg() {
  echo -e "${GREEN}✅ $1${NC}"
}

warn_msg() {
  echo -e "${YELLOW}⚠️  $1${NC}"
}

# Check if n8n is healthy before proceeding
echo "🔍 Checking n8n status..."
# Get the directory where this script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if ! "$SCRIPT_DIR/forge" health; then
  error_exit "n8n is not healthy. Run '$SCRIPT_DIR/forge doctor' for diagnosis."
fi

cat > $CLAUDE_PROMPT <<EOF
Generate a clean n8n workflow JSON for fetching Jacksonville weather and sending Slack notifications when rain is expected.

Requirements:
- Use n8n platform
- Include HTTP Request, Function, and Slack nodes
- Use JSON-based weather API
- Only send Slack message when rain > 50%
- Output ONLY valid JSON, no markdown, no explanations, no code blocks

Generate the complete n8n workflow JSON object.
EOF

echo "🤖 Prompting Claude..."
if ! claude --print < $CLAUDE_PROMPT > workflow.json; then
  error_exit "Failed to generate workflow with Claude"
fi

# Check if Claude actually generated content
if [ ! -s workflow.json ]; then
  error_exit "Claude generated empty workflow"
fi

success_msg "Claude generated workflow"

echo "🧹 Cleaning workflow JSON..."
# Extract JSON if it's wrapped in markdown code blocks
if grep -q "\`\`\`json" workflow.json; then
    sed -n '/\`\`\`json/,/\`\`\`/p' workflow.json | sed '1d;$d' > workflow_clean.json
    mv workflow_clean.json workflow.json
fi

# Remove any markdown text before the first {
sed -i '/^[^{]*$/d' workflow.json

# Validate the JSON is properly formatted
if ! jq empty workflow.json 2>/dev/null; then
  error_exit "Generated workflow is not valid JSON"
fi

success_msg "Workflow JSON cleaned and validated"

echo "📡 Sending workflow to n8n..."
# Store the API response for error checking
API_RESPONSE=$(curl -s -X POST "$N8N_HOST/rest/workflows" \
  -H "Authorization: Bearer $N8N_API_KEY" \
  -H "Content-Type: application/json" \
  -d @workflow.json)

# Check if API call was successful
if echo "$API_RESPONSE" | jq -e '.id' >/dev/null 2>&1; then
  WORKFLOW_ID=$(echo "$API_RESPONSE" | jq -r '.id')
  success_msg "Workflow created successfully with ID: $WORKFLOW_ID"
  
  echo "🌐 Opening n8n workflow..."
  if command -v xdg-open &> /dev/null; then
    xdg-open "$N8N_HOST/workflow/$WORKFLOW_ID"
  else
    echo "Open manually: $N8N_HOST/workflow/$WORKFLOW_ID"
  fi
else
  error_exit "Failed to create workflow. API response: $API_RESPONSE"
fi
