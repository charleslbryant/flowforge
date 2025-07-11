#!/bin/bash

echo "🔑 Testing n8n API Key..."
echo "API Key: ${N8N_API_KEY:0:50}..."

if [ -z "$N8N_API_KEY" ]; then
    echo "❌ N8N_API_KEY environment variable not set"
    exit 1
fi

echo "📡 Testing API connection..."
RESPONSE=$(curl -s "http://localhost:5678/api/v1/workflows" -H "X-N8N-API-KEY: $N8N_API_KEY")

if echo "$RESPONSE" | grep -q "unauthorized"; then
    echo "❌ API Key is invalid or expired"
    echo "Response: $RESPONSE"
    echo ""
    echo "📝 To fix:"
    echo "1. Go to http://localhost:5678"
    echo "2. Settings → n8n API" 
    echo "3. Generate new API key"
    echo "4. export N8N_API_KEY=\"your-new-key\""
    exit 1
elif echo "$RESPONSE" | grep -q "data"; then
    echo "✅ API Key is working!"
    echo "Response: $RESPONSE"
    exit 0
else
    echo "⚠️  Unexpected response:"
    echo "$RESPONSE"
    exit 1
fi