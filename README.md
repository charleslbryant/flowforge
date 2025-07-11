# FlowForge

![Validate workflow.json](https://github.com/charleslbryant/flowforge/actions/workflows/validate-workflow.yml/badge.svg)

**🔥 Get your flow up!**

## About

FlowForge is your copilot for automating the creation and management of AI workflows with prompts. It's a terminal-first starter kit for AI-powered workflow automation that connects Claude Code directly to n8n through a comprehensive shell script API.

## Why FlowForge

Tired of manually building workflows? FlowForge flips the script by using Claude Code and a custom n8n API shell script to generate entire workflows from a single prompt. It's not just a toolkit, it's liberation.

* ⚡ **Automate tedious tasks instantly** - From prompt to production in minutes
* 🛡️ **Validate your workflows** before they break production
* 🎯 **Focus on logic, not clicking** around endless UI forms
* 🤖 **Make your automation AI-native** from the jump
* 🔒 **Run it locally, privately, and offline** if needed
* 🧠 **Direct Claude integration** with comprehensive n8n API access

Whether you're an ops ninja, indie hacker, or enterprise AI tinkerer, FlowForge gets your flows up faster and smarter.

## Architecture

FlowForge provides Claude Code with direct n8n API access through:

- 🔧 **`scripts/forge`** - Main CLI for n8n management (start, stop, health checks)
- 🌊 **`scripts/n8n-api.sh`** - Complete n8n API wrapper (workflows, executions, credentials)
- 📚 **`scripts/n8n-help.sh`** - Quick reference guide for all API operations
- 🧪 **`scripts/test-api.sh`** - API connection testing and validation

## Features

* 🎨 **Claude Code prompt templates** for workflow generation
* 🔄 **Complete n8n workflow management** via shell script API
* 📊 **Workflow validation and formatting** with JSON schema
* 🚀 **One-command installation and setup**
* 🧰 **Comprehensive tooling** for install, demo, and maintenance
* 📋 **Makefile shortcuts** for quick operations

## Quickstart

Clone the repo inside Ubuntu/WSL and run:

```bash
git clone https://github.com/charleslbryant/flowforge.git
cd flowforge
make install
make demo
```

### Environment Setup

FlowForge uses environment variables for secure configuration:

```bash
# Copy the example file
cp .env.example .env

# Edit with your values
# You'll need to generate an n8n API key at http://localhost:5678/settings/api
```

## Requirements

To run FlowForge, make sure you have:

### System Requirements
* **Ubuntu on WSL2** (Windows Subsystem for Linux) or native Ubuntu
* **Node.js** (v16+, use `nvm` recommended)
* **npm** (v8 or later)
* **jq** (for JSON formatting)
* **bash** (v4+)

### Tools
* **Claude Code CLI** installed globally:
  ```bash
  npm install -g @anthropic-ai/claude-code
  ```
* **GitHub CLI** (optional, for repo operations)
* **Docker Desktop** (optional, if you want to run n8n via Docker)

> 💡 **Tip**: If you're unsure about dependencies, just run `scripts/forge doctor` — it'll tell you what's missing.

## Installation

### Option 1: Quick Install (Recommended)
```bash
make install
```

### Option 2: Manual Install
```bash
scripts/forge install
```

### What happens during installation:
1. 📦 Install required packages (n8n, Claude Code, dependencies)
2. 🚀 Start n8n locally on port 5678
3. 🔐 Guide you through n8n API key generation
4. 🧠 Configure the n8n API shell script with authentication
5. ✅ Test API connection and validate setup

## Usage

### Core Commands

```bash
# System Management
scripts/forge start          # Start n8n in background
scripts/forge stop           # Stop n8n process  
scripts/forge restart        # Restart n8n process
scripts/forge health         # Check n8n status and API
scripts/forge doctor         # Full system diagnostic

# Workflow Operations  
scripts/n8n-api.sh list-workflows           # List all workflows
scripts/n8n-api.sh create-workflow [file]   # Create from JSON
scripts/n8n-api.sh activate-workflow [id]   # Activate workflow
scripts/n8n-api.sh list-executions [id]     # Check execution history

# Development Tools
scripts/forge validate      # Validate workflow.json schema
scripts/forge format        # Format workflow.json with jq
scripts/n8n-api.sh help     # Show all API commands
scripts/n8n-help.sh         # Quick reference guide
```

### Quick Operations

```bash
make demo                    # Generate and test demo workflow
make install                 # Run full installation
make nuke                    # Clean uninstall everything
```

### Workflow Development Process

1. **Start n8n**: `scripts/forge start`
2. **Check health**: `scripts/forge doctor` 
3. **Create workflow**: Use Claude Code with our API tools
4. **Validate**: `scripts/forge validate`
5. **Deploy**: `scripts/n8n-api.sh activate-workflow [id]`

## API Reference

Our shell script provides comprehensive n8n API access:

```bash
# Workflow Management
scripts/n8n-api.sh list-workflows              # List all workflows
scripts/n8n-api.sh get-workflow [id]           # Get workflow details
scripts/n8n-api.sh create-workflow [file]      # Create from JSON file
scripts/n8n-api.sh update-workflow [id] [file] # Update existing
scripts/n8n-api.sh delete-workflow [id]        # Delete workflow
scripts/n8n-api.sh activate-workflow [id]      # Activate
scripts/n8n-api.sh deactivate-workflow [id]    # Deactivate

# Execution Management  
scripts/n8n-api.sh list-executions [id]        # List executions
scripts/n8n-api.sh get-execution [id]          # Get execution details

# Credential Management
scripts/n8n-api.sh list-credentials            # List credentials
scripts/n8n-api.sh create-credential [file]    # Create credential
scripts/n8n-api.sh test-credential [id]        # Test connection

# Advanced
scripts/n8n-api.sh raw [method] [endpoint] [data] # Direct API access
```

## Troubleshooting

### Common Issues

**n8n not starting:**
```bash
scripts/forge doctor  # Full diagnostic
scripts/forge start   # Start n8n
```

**API authentication failed:**
```bash
# Generate new API key at http://localhost:5678/settings/api
export N8N_API_KEY="your-new-key"
scripts/test-api.sh   # Test connection
```

**Workflow validation errors:**
```bash
scripts/forge validate  # Check JSON schema
scripts/forge format    # Fix formatting
```

## Contributing

Want to help forge the future of AI workflow automation?

1. **Fork the repo** and create a feature branch
2. **Make your changes** (new features, bug fixes, docs improvements)
3. **Test thoroughly** with `make demo` and `scripts/forge doctor`
4. **Submit a pull request** with clear description

### Development Guidelines
- Follow shell script best practices
- Update documentation for new features  
- Test all API operations before submitting
- Keep scripts portable across Ubuntu/WSL environments

Ideas welcome. Bugs doubly so. Let's build something that saves us all from clicking through endless workflow UIs ever again.

## Origin Story

I created FlowForge on my cracked and tired old Android phone with "George," my AI assistant. After talking through the pain of painstakingly building n8n workflows by hand, we landed on the idea of automating the entire process using Claude Code. 

We wanted to give Claude direct access to the n8n API, so we built a comprehensive shell script that handles all workflow operations — from creation and validation to execution monitoring and credential management.

Many thumb taps later, we had a GitHub repo and the core tools that make up FlowForge. It was built for Windows running WSL, but it should work anywhere Ubuntu does.

Hope it brings you as much joy and efficiency as it brought us.

**~ Charles and George** 🤖

---

## 📚 Documentation

- [Getting Started Guide](docs/getting-started.md)
- [Claude Code Integration](CLAUDE.md) 
- [API Reference](claude-n8n-tools.md)

## 🏷️ Tags

`n8n` `claude-code` `workflow-automation` `shell-script` `ai` `api` `ubuntu` `wsl`