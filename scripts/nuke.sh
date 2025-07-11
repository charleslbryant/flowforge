#!/bin/bash
set -e
killall n8n || true
rm -rf ~/.n8n ~/.claude.json workflow.json *.log
echo "🧨 Nuked all local data, logs, and config."
