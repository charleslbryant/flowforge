#!/bin/bash

# Script to sync local GitHub Issue files with actual GitHub Issues
# Enhanced for Context-Managed Development System (CMDS)
# Run this from the root directory

echo "🔄 Syncing GitHub Issues for CMDS..."

# Check if we're in the right directory
if [ ! -f "LICENSE" ]; then
    echo "Error: Please run this script from the root directory"
    exit 1
fi

# Check if gh is authenticated
if ! gh auth status >/dev/null 2>&1; then
    echo "Error: GitHub CLI not authenticated. Please run 'gh auth login' first"
    exit 1
fi

# Create sync directory
mkdir -p docs/github-issues/sync

echo "Fetching GitHub Issues..."

# Fetch all issues and save to local files
gh issue list --limit 100 --json number,title,body,labels,assignees,state,createdAt,updatedAt --jq '.[] | "\(.number) \(.title) \(.state)"' | while read number title state; do
    echo "Syncing Issue #$number: $title"
    
    # Get full issue content
    gh issue view $number --json number,title,body,labels,assignees,state,createdAt,updatedAt > "docs/github-issues/sync/issue-$number.json"
    
    # Create markdown version
    gh issue view $number --json number,title,body,labels,assignees,state,createdAt,updatedAt --jq '
    "# Issue #\(.number): \(.title)\n\n" +
    "## Status\n" +
    "- **State**: \(.state)\n" +
    "- **Created**: \(.createdAt)\n" +
    "- **Updated**: \(.updatedAt)\n" +
    "- **Labels**: \(.labels | join(", "))\n" +
    "- **Assignees**: \(.assignees | join(", "))\n\n" +
    "## Content\n\n" +
    (.body // "No content")
    ' > "docs/github-issues/sync/issue-$number.md"
done

echo "Creating sync summary..."
cat > docs/github-issues/sync/README.md << 'EOF'
# GitHub Issues Sync

This directory contains automatically synced copies of GitHub Issues.

## Last Sync
$(date)

## Files
- `issue-*.json`: Raw JSON data from GitHub API
- `issue-*.md`: Markdown formatted issue content
- `README.md`: This file

## Usage
- These files are automatically generated and should not be edited manually
- Use them for offline reference or documentation
- The source of truth is always GitHub Issues

## Sync Command
Run `./scripts/sync-github-issues.sh` to update these files.
EOF

# Generate CMDS task queue from 'now' priority issues
echo "📋 Updating CMDS task queue..."
mkdir -p docs/session-context

# Create/update NEXT_TASKS.md with current 'now' priority issues
cat > docs/session-context/NEXT_TASKS_GENERATED.md << 'EOF'
# Next Tasks Queue (Auto-Generated)

## Now (Active Sprint)
EOF

# Add 'now' priority issues
gh issue list --label "now" --limit 10 --json number,title,labels,assignees --jq '.[] | "1. [ ] \(.title) - [#\(.number)](https://github.com/charleslbryant/flowforge/issues/\(.number))"' >> docs/session-context/NEXT_TASKS_GENERATED.md

cat >> docs/session-context/NEXT_TASKS_GENERATED.md << 'EOF'

## Next (Backlog)
EOF

# Add 'next' priority issues
gh issue list --label "next" --limit 10 --json number,title,labels,assignees --jq '.[] | "1. [ ] \(.title) - [#\(.number)](https://github.com/charleslbryant/flowforge/issues/\(.number))"' >> docs/session-context/NEXT_TASKS_GENERATED.md

cat >> docs/session-context/NEXT_TASKS_GENERATED.md << 'EOF'

## Future (Icebox)
EOF

# Add 'future' priority issues
gh issue list --label "future" --limit 5 --json number,title,labels,assignees --jq '.[] | "1. [ ] \(.title) - [#\(.number)](https://github.com/charleslbryant/flowforge/issues/\(.number))"' >> docs/session-context/NEXT_TASKS_GENERATED.md

cat >> docs/session-context/NEXT_TASKS_GENERATED.md << 'EOF'

---
*Auto-generated from GitHub Issues. Last updated: $(date)*
*Manual edits should be made in NEXT_TASKS.md*
EOF

# Update current development focus from 'now' issues
CURRENT_ISSUE=$(gh issue list --label "now" --assignee "@me" --limit 1 --json number,title --jq 'if length > 0 then .[0] | "Issue #\(.number): \(.title)" else "No assigned tasks" end')

echo "📊 Current development focus: $CURRENT_ISSUE"

echo ""
echo "✅ CMDS Sync completed successfully!"
echo ""
echo "📁 Files updated:"
echo "   • docs/github-issues/sync/issue-*.json (raw data)"
echo "   • docs/github-issues/sync/issue-*.md (markdown)"
echo "   • docs/github-issues/sync/README.md (summary)"
echo "   • docs/session-context/NEXT_TASKS_GENERATED.md (task queue)"
echo ""
echo "🎯 Development Context:"
echo "   • Current focus: $CURRENT_ISSUE"
echo "   • Ready for AI session startup"
echo ""
echo "💡 Note: GitHub Issues are source of truth. Use generated files for AI context." 