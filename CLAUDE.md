# CLAUDE.md

This file provides guidance to AI assistants when working with code in this repository.

## AI Assistant Identity
AI assistants working on this project should identify themselves as "George" in commits and contributions to maintain consistent attribution across different AI models and sessions.

## Project Overview

FlowForge is an AI-powered workflow automation CLI tool that integrates Claude Code with n8n workflow management. It generates n8n workflows from natural language prompts and has two implementations:

1. **Bash Implementation** (`/scripts/`) - Production-ready original implementation
2. **.NET Implementation** (`/dotnet/`) - Modern port with improved architecture (in development)

## Context Management

### Session Startup (Required Reading)
1. **Product Context**: Read `/docs/product/` for foundational product vision and strategy
2. **Architecture**: Read `/docs/architecture.md` for FlowForge overview and dual-implementation approach
3. **Current State**: Read `/docs/session-context/CURRENT_STATE.md` for development progress
4. **Task Queue**: Read `/docs/session-context/NEXT_TASKS.md` for prioritized work
5. **Focus**: Select ONE task from "Now" priority and work on it using TDD approach

### Session Workflow
1. Complete session startup reading (above)
2. Work on ONE task from the "Now" priority queue
3. **Assign GitHub issues** to self before starting work
4. **Create feature branch** for the GitHub issue(s)
5. Use Test-Driven Development (TDD) approach for .NET development
6. Update relevant ADRs for architectural decisions
7. Update documentation as needed
8. **Commit changes** to feature branch with George attribution
9. **Create Pull Request** with comprehensive description
10. **Close completed GitHub issues** with completion comments
11. Update context files for next session
12. End session (clear context)

### Context Files
- `product/product-vision.md` - Long-term product vision and goals
- `product/product-strategy.md` - Go-to-market and growth strategy
- `architecture.md` - Technical overview and dual-implementation approach
- `CURRENT_STATE.md` - Current project state and development progress
- `NEXT_TASKS.md` - Prioritized task queue linked to GitHub Issues
- `ACTIVE_SESSION.md` - Current session progress tracking

## Key Architecture

### Bash Implementation (`/scripts/`)
- **`forge`** - Main CLI tool (bash script) that handles installation, validation, and formatting
- **`n8n-api.sh`** - Custom shell script providing comprehensive n8n API access
- **`workflow.json`** - Generated n8n workflow file (populated by Claude using the API script)
- **`workflow.prompt.yaml`** - Structured prompt template for workflow generation
- **`templates/`** - Directory containing JSON workflow templates
- **API Integration** - Direct n8n API access through authenticated shell script

### .NET Implementation (`/dotnet/`)
- **Commands Layer** - CLI command implementations using Spectre.Console.Cli
- **Services Layer** - Business logic (HealthChecking, ProcessManagement, SystemChecking)
- **Infrastructure Layer** - External dependencies (Http, Process)
- **Test-Driven Development** - Comprehensive test coverage with xUnit and Moq
- **Clean Architecture** - Proper separation of concerns and dependency injection

## Common Commands

### Flow Metrics Commands

#### Generate Flow Analysis Reports
```bash
# Generate comprehensive flow metrics analysis
./scripts/flow-analysis.sh

# View current flow dashboard
cat docs/metrics/reports/dashboard.md

# View weekly summary
cat docs/metrics/reports/weekly-summary.md
```

### Task Management Commands

#### Review Available Work
```bash
# Show unassigned tasks ready for work
gh issue list --label "task" --no-assignee --label "now,next" --json number,title,labels

# Show my currently assigned tasks
gh issue list --assignee "charleslbryant" --label "task" --state "open" --json number,title,labels

# Show all PRDs and CRDs for context
gh issue list --label "prd,crd" --state "open" --json number,title,labels
```

#### Task Assignment and Management
```bash
# Assign task to user and prioritize
gh issue edit [number] --add-assignee "charleslbryant"
gh issue edit [number] --add-label "now" --remove-label "next,future"

# Create new issues collaboratively
gh issue create --title "[PRD/CRD/Task]: [Title]" --body-file temp.md --label "[appropriate-labels]"

# Close completed issues with comments
gh issue close [number] --comment "✅ Completed: [description of work done]"
```

### Git Workflow and Commit Standards

#### Commit Message Format
All AI assistant commits must use this format:
```bash
git commit -m "$(cat <<'EOF'
Brief descriptive title of changes

- Bullet point list of key changes
- Include technical details and scope
- Reference resolved GitHub issues if applicable
- Resolves GitHub issues #[number], #[number]

🤖 Generated with AI Assistant (George)

Co-Authored-By: George <george@decoupledlogic.com>
EOF
)"
```

#### Complete Git Ops Development Workflow
1. **Assignment**: Assign relevant GitHub issues to "charleslbryant"
2. **Branch Creation**: Create feature branch for the GitHub issue
   ```bash
   git checkout -b feature/issue-[number]-[brief-description]
   # Example: git checkout -b feature/issue-10-stop-command
   ```
3. **Development**: Implement features using TDD approach on the feature branch
4. **Testing**: Ensure all tests pass before committing
5. **Commit**: Use standardized commit message format with George attribution
6. **Branch Sync**: Pull latest changes and merge main into feature branch
   ```bash
   git checkout main
   git pull origin main
   git checkout feature/issue-[number]-[brief-description]
   git merge main
   ```
7. **Push Branch**: Push feature branch to remote
   ```bash
   git push -u origin feature/issue-[number]-[brief-description]
   ```
8. **Pull Request**: Create PR using GitHub CLI with auto-delete enabled
   ```bash
   gh pr create --title "[Brief description]" --body "$(cat <<'EOF'
   ## Summary
   - Bullet point summary of changes
   - Technical implementation details
   - Test coverage information
   
   ## Resolves
   - Closes #[issue-number]
   
   ## Test Plan
   - [ ] All existing tests pass
   - [ ] New tests added for new functionality
   - [ ] Manual testing completed
   
   🤖 Generated with AI Assistant (George)
   EOF
   )"
   ```
9. **PR Merge and Cleanup**: Merge PR and clean up branches
   ```bash
   # Merge the PR (this will auto-close linked issues if properly formatted)
   gh pr merge --squash --delete-branch
   
   # Switch back to main and pull latest changes
   git checkout main
   git pull origin main
   
   # Delete local feature branch
   git branch -d feature/issue-[number]-[brief-description]
   
   # Verify branch cleanup
   git branch -a  # Should not show the deleted feature branch
   ```
10. **Issue Management**: Verify GitHub issues were auto-closed by PR merge
11. **Documentation**: Update relevant documentation and context files

#### Branch Cleanup Utilities
```bash
# Clean up all merged local branches (run periodically)
git branch --merged main | grep -v "main\|master" | xargs -n 1 git branch -d

# Clean up remote tracking branches that no longer exist on remote
git remote prune origin

# List all local branches to verify cleanup
git branch -a

# Force delete unmerged local branches (use with caution)
# git branch -D feature/issue-[number]-[brief-description]
```

#### Repository Health Check
```bash
# Check for stale branches (branches that haven't been updated in 30+ days)
gh api repos/:owner/:repo/branches | jq '.[] | select(.commit.commit.author.date < (now - 2592000 | strftime("%Y-%m-%dT%H:%M:%SZ"))) | .name'

# View all open PRs to ensure they're still relevant
gh pr list --state open
```

### .NET Development Commands
```bash
# Build and test .NET implementation
cd dotnet
dotnet build                    # Build the solution
dotnet test                     # Run all tests
dotnet run --project src/FlowForge.Console -- health    # Run health command
dotnet run --project src/FlowForge.Console -- doctor    # Run doctor command
dotnet run --project src/FlowForge.Console -- start     # Run start command

# TDD Development Workflow
dotnet test --watch             # Continuous testing during development
```

### n8n Process Management (Bash Implementation)
```bash
# Start n8n in background
scripts/forge start

# Stop n8n process
scripts/forge stop

# Restart n8n process
scripts/forge restart

# Check n8n health and API status
scripts/forge health

# Full system diagnostic
scripts/forge doctor
```

### n8n API Management
```bash
# Workflow Operations
scripts/n8n-api.sh list-workflows              # List all workflows
scripts/n8n-api.sh get-workflow [id]          # Get workflow by ID
scripts/n8n-api.sh create-workflow [file]     # Create workflow from JSON file
scripts/n8n-api.sh update-workflow [id] [file] # Update workflow from JSON file
scripts/n8n-api.sh delete-workflow [id]       # Delete workflow by ID
scripts/n8n-api.sh activate-workflow [id]     # Activate workflow
scripts/n8n-api.sh deactivate-workflow [id]   # Deactivate workflow

# Execution Operations
scripts/n8n-api.sh list-executions [id]       # List workflow executions (optional workflow-id)
scripts/n8n-api.sh get-execution [id]         # Get execution details

# Credential Operations
scripts/n8n-api.sh list-credentials           # List all credentials
scripts/n8n-api.sh get-credential [id]       # Get credential by ID
scripts/n8n-api.sh create-credential [file]  # Create credential from JSON file
scripts/n8n-api.sh test-credential [id]      # Test credential

# Utility Operations
scripts/n8n-api.sh get-workflow-status [id]  # Get workflow status
scripts/n8n-api.sh raw [method] [endpoint] [data] # Raw API call
```

### Development and Testing
```bash
# Install dependencies and setup n8n + API script
make install
# or
scripts/forge install

# Run demo workflow generation (requires n8n running)
make demo

# Validate workflow.json against n8n schema
scripts/forge validate

# Format workflow.json with jq
scripts/forge format

# Use Claude with n8n API to create workflows
scripts/n8n-api.sh create-workflow workflow.json
```

### Installation Scripts
```bash
# Full setup with n8n and API configuration
bash scripts/install-n8n-claude-mcp.sh

# Clean removal of n8n and reset
bash scripts/nuke.sh
```

## Workflow Generation Process

### Streamlined Creation (Recommended)
```bash
# One-command workflow creation
scripts/forge create-workflow "your workflow description"
```

This single command:
1. **Checks system health** - Validates n8n and API connectivity
2. **Generates workflow** - Uses Claude with template context
3. **Validates JSON** - Ensures proper formatting
4. **Creates in n8n** - Deploys via API
5. **Opens in browser** - Automatically opens workflow editor

### Manual Process (Advanced)
1. **Start n8n**: `scripts/forge start` (if not already running)
2. **Verify system**: `scripts/forge doctor` (should show "Ready to run")
3. **Generate workflow**: Create JSON file and use `scripts/n8n-api.sh create-workflow [file]`
4. **Validate**: `scripts/forge validate`
5. **Format**: `scripts/forge format`

### Available Templates
Claude is aware of these workflow templates in `templates/`:
- **email-workflow.json** - Email processing with IMAP and filtering
- **web-scraper.json** - Web scraping with HTTP requests and data processing
- **slack-notification.json** - Slack messaging with scheduled triggers

These templates are automatically included in Claude's context when generating workflows.

## Troubleshooting

### Common Issues

**"n8n process not found"**
```bash
scripts/forge start
```

**"Unauthorized" API errors**
- Generate new API key at http://localhost:5678/settings/api
- Export key: `export N8N_API_KEY="your-new-key"`

**Empty workflow.json**
- Check n8n is running: `scripts/forge health`
- Verify API key is valid
- Check n8n logs: `tail -f ~/n8n.log`

**System health check**
```bash
scripts/forge doctor  # Shows all issues and solutions
```

## API Configuration

The n8n-api.sh script is configured with:
- n8n API connection on localhost:5678
- Session-based authentication using browser cookies
- API key authentication for secure access
- Environment variables for N8N_API_KEY

## Prompt Template Structure

YAML templates should include:
- `task`: High-level workflow description
- `context`: Platform and tools specification
- `purpose`: Specific workflow objective
- `constraints`: Technical requirements
- `output_format`: Expected output type (n8n_workflow_json)

## Platform Requirements

- Ubuntu/WSL2 environment
- Node.js with npm
- n8n installed globally
- Claude Code CLI installed globally
- jq for JSON formatting
- ajv-cli for schema validation