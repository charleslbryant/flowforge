# FlowForge End-to-End Workflow Documentation

## Overview

FlowForge is an AI-powered workflow automation tool that integrates Claude Code with n8n workflow management. It provides a seamless experience for generating, validating, and deploying n8n workflows using natural language prompts.

## Architecture Components

### Core Components

1. **`forge`** - Main CLI tool (bash script)
   - Handles installation, validation, formatting, and workflow creation
   - Provides health checks and system diagnostics
   - Located at: `scripts/forge`

2. **`n8n-api.sh`** - n8n API Integration Layer
   - Comprehensive shell script for n8n API access
   - Handles authentication via session cookies and API keys
   - Located at: `scripts/n8n-api.sh`

3. **Template System** - Workflow Templates
   - Pre-built workflow patterns for common use cases
   - Located at: `templates/`
   - Available templates: email-workflow, web-scraper, slack-notification

4. **Claude Integration** - AI Workflow Generation
   - Uses Claude Code CLI for natural language to JSON conversion
   - Template-aware generation with contextual examples
   - Automatic cleanup and validation

## End-to-End Workflow Process

### 1. System Initialization

```bash
# Start n8n service
scripts/forge start

# Verify system health
scripts/forge doctor
```

**Process:**
- Checks for n8n process running on port 5678
- Validates API authentication using session cookies
- Verifies all dependencies (node, npm, jq, curl, nc, claude)
- Confirms n8n web interface accessibility

### 2. Workflow Generation (The Magic)

```bash
scripts/forge create-workflow "send me daily weather updates"
```

**Step-by-step process:**

#### 2.1 Pre-flight Checks
- Validates n8n health status
- Ensures API connectivity
- Checks Claude CLI availability

#### 2.2 Prompt Engineering
- Generates `workflow.prompt.yaml` with:
  - User's natural language description
  - Available template context (from `templates/`)
  - n8n-specific requirements
  - JSON formatting constraints

#### 2.3 AI Generation
- Executes: `claude --print < workflow.prompt.yaml > workflow.json`
- Claude analyzes the prompt and templates
- Generates valid n8n workflow JSON

#### 2.4 JSON Cleanup & Validation
- Removes markdown code blocks if present
- Strips non-JSON content
- Validates JSON syntax with `jq`
- Ensures required fields are present

#### 2.5 n8n Deployment
- Calls `scripts/n8n-api.sh create-workflow workflow.json`
- Handles API errors gracefully
- Returns workflow ID on success

#### 2.6 Browser Integration
- Automatically opens workflow in n8n web interface
- Provides manual link as fallback

### 3. API Layer Details

#### Authentication Flow
```bash
# API key authentication (recommended)
N8N_API_KEY='...' # JWT token from n8n settings

# Generate your API key at: http://localhost:5678/settings/api
```

#### API Operations Available
- `list-workflows` - List all workflows
- `get-workflow [id]` - Get workflow details
- `create-workflow [file]` - Create new workflow
- `update-workflow [id] [file]` - Update existing workflow
- `delete-workflow [id]` - Delete workflow
- `activate-workflow [id]` - Activate workflow
- `deactivate-workflow [id]` - Deactivate workflow
- `list-executions [id]` - List workflow executions
- `get-execution [id]` - Get execution details

### 4. Template System Integration

#### Template Discovery
- `templates/` directory contains JSON workflow templates
- Templates are automatically included in Claude prompts
- Claude uses templates as reference patterns

#### Available Templates
1. **Email Workflow** (`templates/email-workflow.json`)
   - Schedule trigger every 30 minutes
   - IMAP email reading
   - Unread email filtering

2. **Web Scraper** (`templates/web-scraper.json`)
   - Daily schedule trigger
   - HTTP request to target URL
   - Data processing with JavaScript

3. **Slack Notification** (`templates/slack-notification.json`)
   - Daily morning schedule
   - Message generation
   - Slack integration

### 5. Error Handling & Recovery

#### Common Error Scenarios
1. **n8n Not Running**
   - Detection: Health check fails
   - Recovery: `scripts/forge start`

2. **API Key Expired**
   - Detection: 401 unauthorized responses
   - Recovery: Generate new key at http://localhost:5678/settings/api

3. **Invalid JSON Generation**
   - Detection: jq validation fails
   - Recovery: Automatic cleanup, re-validation

4. **Workflow Creation Errors**
   - Detection: n8n API error responses
   - Recovery: Clear error messages, workflow.json preserved for debugging

### 6. File Structure & Data Flow

```
flowforge/
├── scripts/
│   ├── forge                 # Main CLI entry point
│   └── n8n-api.sh           # API integration layer
├── templates/
│   ├── email-workflow.json   # Email processing template
│   ├── web-scraper.json     # Web scraping template
│   └── slack-notification.json # Notification template
├── docs/
│   └── end-to-end-workflow.md # This document
├── workflow.json            # Generated workflow (temp)
├── workflow.prompt.yaml     # Generated prompt (temp)
└── CLAUDE.md               # Claude Code instructions
```

### 7. Integration Points

#### Claude Code Integration
- Natural language processing via `claude --print`
- Template-aware context injection
- Automatic JSON cleanup and validation

#### n8n Integration
- RESTful API communication
- Session and API key authentication
- Real-time workflow deployment

#### Browser Integration
- Automatic workflow opening
- Direct link generation
- Cross-platform compatibility (xdg-open)

## Performance Characteristics

### Typical Workflow Creation Time
- **Total**: 10-30 seconds
- **AI Generation**: 5-15 seconds
- **API Deployment**: 1-3 seconds
- **Validation**: <1 second

### Resource Requirements
- **Memory**: ~100MB (n8n process)
- **CPU**: Minimal (bash scripts)
- **Network**: Localhost API calls only
- **Storage**: <10MB templates and cache

## Security Considerations

### Authentication Security
- Session cookies stored in script (local environment)
- API keys with limited scope (n8n workflows only)
- No external network access for sensitive operations

### Data Privacy
- All processing happens locally
- No workflow data sent to external services
- Claude prompts contain only workflow descriptions

## Extensibility

### Adding New Templates
1. Create JSON file in `templates/`
2. Templates automatically included in Claude context
3. No code changes required

### Custom API Operations
1. Add new case to `n8n-api.sh`
2. Follow existing pattern for error handling
3. Use `n8n_api_call` function for consistency

### Additional CLI Commands
1. Add new case to `scripts/forge`
2. Follow existing pattern for validation
3. Include in help text

## Troubleshooting Guide

### Common Issues

1. **"n8n process not found"**
   ```bash
   scripts/forge start
   ```

2. **"API authentication failed"**
   - Generate new API key at http://localhost:5678/settings/api
   - Update `N8N_API_KEY` in `scripts/n8n-api.sh`

3. **"Invalid JSON generated"**
   - Check `workflow.json` for syntax errors
   - Verify Claude CLI is working: `claude --version`

4. **"Workflow creation failed"**
   - Run `scripts/forge health` to check system status
   - Check n8n logs: `tail -f ~/n8n.log`

### Debug Commands
```bash
# Full system diagnostic
scripts/forge doctor

# Check n8n health only
scripts/forge health

# Test API connectivity
scripts/n8n-api.sh list-workflows

# Validate workflow JSON
scripts/forge validate

# Format workflow JSON
scripts/forge format
```

## Future Enhancements

### Planned Features
1. **Workflow Templates Library** - Expandable template system
2. **Batch Operations** - Multiple workflow creation
3. **Workflow Versioning** - Git-based workflow management
4. **Advanced Error Recovery** - Automatic retry mechanisms
5. **Workflow Testing** - Automated validation and testing

### Integration Opportunities
1. **CI/CD Pipeline** - GitHub Actions integration
2. **Monitoring** - Workflow execution monitoring
3. **Backup & Restore** - Workflow backup automation
4. **Multi-Environment** - Development/staging/production workflows

---

*This documentation reflects the current state of FlowForge as of the latest implementation. For updates and changes, refer to the project's git history and CLAUDE.md file.*