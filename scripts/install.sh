#!/bin/bash
set -e

LOG="$HOME/.flowforge-install.log"
exec > >(tee -a "$LOG") 2>&1

N8N_PORT=5678
N8N_HOST="http://localhost:$N8N_PORT"
HEADLESS=false

# Parse flags
while [[ $# -gt 0 ]]; do
  case "$1" in
    --headless)
      HEADLESS=true
      export N8N_API_KEY="${N8N_API_KEY:-}"
      shift
      ;;
    *)
      echo "❌ Unknown option: $1"
      exit 1
      ;;
  esac
done

echo "📦 Installing n8n, Claude Code, and dependencies..."
npm install -g n8n @anthropic-ai/claude-code ajv-cli bats > /dev/null

# Kill any running n8n process on that port
if lsof -i:$N8N_PORT -t > /dev/null 2>&1; then
  echo "🔁 Stopping existing n8n on port $N8N_PORT..."
  kill -9 $(lsof -i:$N8N_PORT -t) 2>/dev/null
fi

echo "🚀 Starting n8n on $N8N_HOST..."
nohup n8n > "$HOME/n8n.log" 2>&1 &
sleep 5

if [ "$HEADLESS" = false ]; then
  echo "🌐 Opening n8n settings page in your browser..."
  if command -v xdg-open >/dev/null 2>&1; then
    xdg-open "$N8N_HOST/settings/api" 2>/dev/null &
  elif command -v open >/dev/null 2>&1; then
    open "$N8N_HOST/settings/api" 2>/dev/null &
  else
    echo "📝 Please open: $N8N_HOST/settings/api"
  fi
  
  echo ""
  echo "🔐 Generate an API key in n8n and paste it below:"
  read -p "API Key: " N8N_API_KEY
else
  if [ -z "$N8N_API_KEY" ]; then
    echo "❌ N8N_API_KEY environment variable required in headless mode"
    exit 1
  fi
fi

echo "🧠 Configuring n8n API shell script..."
# Store API key in environment for current session
export N8N_API_KEY="$N8N_API_KEY"

# Add to bashrc for persistence
if ! grep -q "export N8N_API_KEY" ~/.bashrc 2>/dev/null; then
    echo "" >> ~/.bashrc
    echo "# FlowForge n8n API Configuration" >> ~/.bashrc
    echo "export N8N_API_KEY=\"$N8N_API_KEY\"" >> ~/.bashrc
fi

echo "✅ FlowForge installation complete!"
echo ""
echo "🎯 Next steps:"
echo "   scripts/forge doctor        # Check system health"
echo "   make test               # Run test suite to verify installation"
echo "   scripts/n8n-api.sh help     # Show available API commands"
echo "   make demo             # Run demo workflow"
echo ""
echo "📚 Documentation:"
echo "   cat CLAUDE.md           # Main documentation"
echo "   cat docs/testing.md     # Testing guide"