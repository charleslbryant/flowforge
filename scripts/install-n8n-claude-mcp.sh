#!/bin/bash
set -e

N8N_PORT=5678
N8N_HOST="http://localhost:$N8N_PORT"

echo "📦 Installing n8n and Claude Code..."
npm install -g n8n @anthropic-ai/claude-code bats

echo "🚀 Starting n8n on port $N8N_PORT..."
nohup n8n > ~/n8n.log 2>&1 &
sleep 5

echo "🔐 Create your n8n API key via $N8N_HOST → Settings → n8n API"
read -p "Paste your API key: " N8N_API_KEY

echo "🧠 Configuring n8n API shell script..."
# Store API key in environment for current session
export N8N_API_KEY="$N8N_API_KEY"

# Add to bashrc for persistence
if ! grep -q "export N8N_API_KEY" ~/.bashrc; then
    echo "" >> ~/.bashrc
    echo "# FlowForge n8n API Configuration" >> ~/.bashrc
    echo "export N8N_API_KEY=\"$N8N_API_KEY\"" >> ~/.bashrc
fi

echo "✅ n8n API shell script configured"
echo "🧹 Testing API connection..."
# Get the directory where this script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
"$SCRIPT_DIR/n8n-api.sh" list-workflows

echo "🎯 Setup complete! You can now use:"
echo "   scripts/n8n-api.sh help     # Show available commands"
echo "   scripts/forge doctor        # Check system health"
