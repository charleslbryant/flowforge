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
7. **Create/update documentation** (see Documentation Requirements below)
8. **Commit changes** to feature branch with George attribution
9. **Pull latest main and merge** to feature branch before pushing
10. **Create Pull Request** with comprehensive description
11. **Close completed GitHub issues** with completion comments
12. Update context files for next session
13. **Instruct operator to /clear context** when all work is complete
14. End session (clear context)

### Operator Check-in Process

To maintain collaboration and transparency during development, AI assistants must provide check-ins at key transition points:

#### 1. Before Starting Major Work
**When**: Before beginning a new CRD, major task, or significant development work
**Format**:
```
## Starting Work Check-in
**About to start**: [Brief description of the work]
**Estimated scope**: [How much work this represents]
**Your options**:
A) Proceed with this approach
B) Focus on [alternative approach]
C) Let me break this down further first
**Recommendation**: [Which option I suggest and why]
```

#### 2. Before Push/PR Creation
**When**: After completing development work but before pushing to remote and creating PR
**Format**:
```
## Ready to Push Check-in
**Completed**: [Summary of what was implemented]
**Changes**: [Key files/areas modified]
**Test status**: [All tests passing, coverage maintained, etc.]
**Your options**:
A) Proceed with push and PR creation
B) Make additional changes first: [what changes]
C) Review the changes before proceeding
**Recommendation**: [Which option I suggest]
```

#### 3. After Major Completion
**When**: After completing a significant piece of work (CRD, major feature, architectural change)
**Format**:
```
## Major Work Completion Check-in
**Just completed**: [What was accomplished]
**Impact**: [How this changes the project state]
**What this enables**: [New capabilities or next logical steps]
**Your options**:
A) Continue with [next logical task]
B) Take a break and review what was done
C) Switch focus to [different area/priority]
D) Conduct retrospective on this work
**Recommendation**: [Which option I suggest and why]
```

#### 4. End of Session
**When**: When all planned work is complete and ready to end the session
**Format**:
```
## Session Completion
**Session summary**: [What was accomplished]
**Current state**: [Where the project stands now]
**Recommended next session focus**: [Specific next steps]
**Operator next steps**: [What you should do - usually /clear context]
```

#### Check-in Rules
- **Always wait for operator response** before proceeding after a check-in
- **Provide clear options** rather than open-ended questions when possible
- **Include recommendations** but make it clear the operator decides
- **Keep check-ins brief** but informative
- **Use consistent formatting** for easy scanning

### Handling PRDs and CRDs

#### PRD (Product Requirement Document)
PRDs are high-level feature specifications. When working on a PRD:
1. **Do NOT implement directly** - PRDs are too broad for a single work session
2. **Break down into CRDs** - Create specific user stories as CRD issues
3. **Link CRDs to parent PRD** - Reference the parent PRD in each CRD

#### CRD (Change Request Document)
CRDs are specific user stories with acceptance criteria. When working on a CRD:
1. **Review task breakdown** - CRDs include a "Tasks Breakdown" section
2. **Create GitHub Task Issues** - Each task in the breakdown should become a separate GitHub issue:
   ```bash
   gh issue create --title "Task: [specific task description]" \
     --body "Part of CRD #[CRD-number] - [CRD title]
   
   ## Summary
   [Task description]
   
   ## Acceptance Criteria
   - [ ] [Specific criteria for this task]
   
   ## Parent Issues
   - Resolves part of #[CRD-number]
   - Depends on #[previous-task-number] (if applicable)
   
   ## Implementation Notes
   [Technical details]" \
     --label "task,[priority]"
   ```
3. **Prioritize tasks**:
   - First task: Label as "now"
   - Dependent tasks: Label as "next"
   - Future tasks: Label as "future"
4. **Work on ONE task at a time**:
   - Assign the "now" task to yourself
   - Complete it before moving to the next
   - Update priority labels as you progress
5. **Create ONE feature branch per CRD** - All tasks for a CRD share the same feature branch

### Context Files
- `product/product-vision.md` - Long-term product vision and goals
- `product/product-strategy.md` - Go-to-market and growth strategy
- `architecture.md` - Technical overview and dual-implementation approach
- `CURRENT_STATE.md` - Current project state and development progress
- `NEXT_TASKS.md` - Prioritized task queue linked to GitHub Issues
- `ACTIVE_SESSION.md` - Current session progress tracking

## Documentation Requirements

Documentation must be created and maintained alongside feature development to ensure consistency and usability.

### Documentation Types and Requirements

#### User Guides (`/docs/user-guides/`)
**When Required**: For any user-facing features, commands, or workflows
**Content Requirements**:
- Step-by-step instructions with examples
- Screenshots or CLI output examples where helpful
- Common troubleshooting scenarios
- Prerequisites and setup requirements

#### Developer Guides (`/docs/developer-guides/`)
**When Required**: For APIs, SDKs, CLI tools, or technical integrations
**Content Requirements**:
- Code examples and usage patterns
- API reference documentation
- SDK/library integration examples
- Technical architecture explanations
- Troubleshooting and debugging guides

#### Feature Documentation Updates
**When Required**: For every new feature or significant change
**Update Requirements**:
- Update relevant existing user/developer guides
- Create new guides if feature introduces new user workflows
- Update CLI help text and command descriptions
- Update API documentation if applicable

### Documentation Workflow Integration

#### During Development
1. **Identify Documentation Needs**: Determine if feature requires new or updated user/developer guides
2. **Create Documentation Draft**: Write documentation alongside code development
3. **Include in Testing**: Test documentation accuracy during feature testing
4. **Review Documentation**: Include documentation review in PR process

#### Documentation Standards
- Use clear, concise language appropriate for target audience
- Include practical examples and real-world usage scenarios
- Maintain consistent formatting and structure across guides
- Link related documentation and provide clear navigation
- Keep documentation files in version control alongside code

### Documentation Maintenance
- Review and update documentation during each feature development cycle
- Mark outdated documentation for update during retrospectives
- Maintain documentation as a first-class citizen alongside code

## Retrospective Process

### When to Conduct Retrospectives

Retrospectives should be conducted:
1. **End of major development cycles** - After completing significant features or CRDs
2. **Monthly cadence** - Regular retrospectives to capture ongoing learnings
3. **After significant issues** - Process improvements following problems or blockers
4. **Before major decisions** - Gather team insights before architectural changes

### Retrospective Workflow

#### 1. Preparation Phase
```bash
# Review current retrospective topics
cat docs/retrospective-topics.md

# List current retrospective GitHub issues
gh issue list --label "retro" --state "open" --json number,title,createdAt
```

#### 2. Topic Management
**Adding New Retrospective Topics**:
```bash
# Create GitHub issue for new retrospective topic
gh issue create --title "Retro: [Topic Title]" --body "$(cat <<'EOF'
**Context**: [Current situation and background]

**Proposal**: [Suggested approach or solution]

**Questions for Discussion**:
- [Specific question 1]
- [Specific question 2]

**Expected Outcome**: [What success looks like]

🤖 Generated with AI Assistant (George)
EOF
)" --label "retro"
```

**Updating retrospective-topics.md**:
- Add new topics to the "Current Topics for Discussion" section
- Move completed topics to the "Completed Topics" section
- Include GitHub issue references for traceability

#### 3. Retrospective Session Execution
1. **Review Topics**: Go through each topic in retrospective-topics.md and related GitHub issues
2. **Discuss Solutions**: Focus on actionable outcomes and concrete next steps
3. **Create Action Items**: Convert decisions into GitHub issues with appropriate labels
4. **Update Documentation**: Record outcomes and update relevant process documentation

#### 4. Post-Retrospective Actions
```bash
# Close completed retrospective topics
gh issue close [issue-number] --comment "✅ Discussed in retrospective: [summary of outcome]"

# Create action items from retrospective outcomes
gh issue create --title "[Action]: [Description]" --body "[Details from retrospective]" --label "process-improvement"

# Update retrospective-topics.md
# Move completed topics to "Completed Topics" section
```

### Retrospective Topic Categories

#### Process Improvements
- Development workflow enhancements
- Testing and quality assurance improvements
- Documentation process refinements
- Git workflow optimizations

#### Technical Decisions
- Architecture changes and ADR updates
- Technology adoption or migration decisions
- Performance optimization opportunities
- Security enhancement proposals

#### Team Collaboration
- Communication improvements
- Tool adoption and usage
- Role clarification and responsibilities
- Cross-team integration topics

### Integration with Session Workflow

Retrospectives are integrated into the session workflow:
1. **Session Preparation**: Review open retrospective issues during session startup
2. **Development Work**: Add retrospective topics as they arise during development
3. **Session Completion**: Update retrospective topics with learnings from the session
4. **Documentation Updates**: Include retrospective outcomes in session documentation

### Retrospective Commands

#### Review and Management
```bash
# Show all retrospective topics and their status
gh issue list --label "retro" --json number,title,state,createdAt

# View specific retrospective topic details
gh issue view [retro-issue-number]

# Add retrospective topic discussion to session notes
echo "## Retrospective Discussion - Issue #[number]" >> docs/session-context/ACTIVE_SESSION.md
```

#### Topic Lifecycle Management
```bash
# Mark retrospective topic as discussed
gh issue edit [issue-number] --add-label "discussed"

# Convert retrospective outcome to action item
gh issue create --title "Action: [Action Description]" --body "From retro #[retro-issue]: [details]" --label "process-improvement"

# Archive completed retrospective topic
gh issue close [issue-number] --comment "Retrospective completed. Action items: #[action-issue-1], #[action-issue-2]"
```

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

#### CRITICAL Branch Management and Task Scope Rules

**These rules are MANDATORY and must be added to session todos to ensure compliance:**

1. **One Branch Per Task/CRD Rule**:
   - Each GitHub task gets its own feature branch: `feature/issue-[number]-[description]`
   - CRDs get their own branch: `feature/crd-[number]-[description]`
   - NEVER work on multiple unrelated tasks in the same branch
   - If work expands beyond original task scope, STOP and check with operator

2. **Task Scope Verification (MANDATORY CHECK)**:
   - Before making ANY code changes, verify current branch matches the task
   - If current task requires changes to files beyond the original scope, check:
     - Is this truly part of the same logical change?
     - Should this be a separate task/branch?
     - Should this wait for the current branch to be merged first?

3. **Session Startup Branch Check (MANDATORY)**:
   - ALWAYS add to session todos: "Check current branch and ensure it matches the work to be done"
   - If on wrong branch: switch or create correct branch BEFORE any changes
   - If unsure about branch scope: ask operator before proceeding

4. **Multi-File Change Protocol**:
   - When changes span multiple commands/files (like RestartCommand + StopCommand):
     - If they're part of the same logical feature/CRD: same branch OK
     - If they're separate concerns: create separate tasks and branches
     - When in doubt: ask operator

5. **Branch Completion Protocol**:
   - Before closing any GitHub issue, ensure ALL changes are committed
   - NEVER close issues if there are uncommitted changes
   - Check `git status` before issue closure
   - If unexpected changes exist: commit them or ask operator

6. **Emergency Branch Recovery**:
   - If wrong changes are on wrong branch: stash changes, switch to correct branch, apply stash
   - If multiple tasks got mixed: create separate commits and cherry-pick to appropriate branches

**Session Todo Template (ALWAYS add these at session start):**
```
- [ ] Verify current git branch matches assigned task
- [ ] Check git status for uncommitted changes  
- [ ] Confirm task scope before making changes beyond original files
- [ ] Commit all changes before closing any GitHub issues
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