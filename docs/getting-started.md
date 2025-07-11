# Getting Started with FlowForge

Welcome to FlowForge, your terminal-first, Claude-powered automation engine built on top of n8n with a comprehensive shell script API.

This guide walks you through setup, configuration, and usage to get you from zero to automated workflows in minutes.

---

## 🧰 Prerequisites

Before starting, ensure you have these system requirements:

### System Requirements
- **Ubuntu on WSL2** or native Ubuntu Linux
- **Node.js** (v16+) - recommend using `nvm` for version management
- **npm** (v8 or later)
- **Git** for cloning the repository
- **jq** for JSON processing
- **bash** (v4+)

### Required Tools
- **Claude Code CLI** - Install globally:
  ```bash
  npm install -g @anthropic-ai/claude-code
  ```
- **n8n** - Will be installed automatically during setup

### Optional Tools
- **GitHub CLI** - For repository operations
- **Docker Desktop** - If you prefer containerized n8n

---

## 🚀 Quick Installation

### Option 1: One-Command Setup
```bash
git clone https://github.com/charleslbryant/flowforge.git
cd flowforge
make install
```

### Option 2: Step-by-Step Setup
```bash
git clone https://github.com/charleslbryant/flowforge.git
cd flowforge
scripts/forge install
```

### What Happens During Installation

1. **📦 Package Installation**
   - Installs n8n, Claude Code, and dependencies
   - Sets up ajv-cli for JSON schema validation

2. **🚀 n8n Startup**
   - Starts n8n on localhost:5678
   - Creates log files for monitoring

3. **🔐 API Key Setup**
   - Opens n8n settings page in your browser
   - Guides you through API key generation
   - Configures shell script authentication

4. **✅ Validation**
   - Tests API connection
   - Verifies all components are working

---

## 🎮 First Steps

### 1. Verify Installation
```bash
scripts/forge doctor
```
This runs a comprehensive health check and shows any issues.

### 2. Check n8n Status
```bash
scripts/forge health
```
Verifies n8n is running and API is accessible.

### 3. Explore Available Commands
```bash
scripts/n8n-api.sh help
scripts/n8n-help.sh
```
Shows all available API operations and examples.

### 4. Run Demo Workflow
```bash
make demo
```
Generates and tests a sample workflow to verify everything works.

---

## 🔧 Core Tools Overview

FlowForge provides several shell scripts for different purposes:

### `scripts/forge` - System Management
Main CLI tool for managing the FlowForge environment:

```bash
scripts/forge start          # Start n8n in background
scripts/forge stop           # Stop n8n process
scripts/forge restart        # Restart n8n
scripts/forge health         # Check n8n status and API
scripts/forge doctor         # Full system diagnostic
scripts/forge validate      # Validate workflow JSON
scripts/forge format        # Format workflow JSON
```

### `scripts/n8n-api.sh` - Workflow Management
Complete n8n API wrapper for all workflow operations:

```bash
# Workflow CRUD
scripts/n8n-api.sh list-workflows
scripts/n8n-api.sh create-workflow workflow.json
scripts/n8n-api.sh get-workflow [workflow-id]
scripts/n8n-api.sh update-workflow [id] workflow.json
scripts/n8n-api.sh delete-workflow [workflow-id]

# Workflow Control
scripts/n8n-api.sh activate-workflow [workflow-id]
scripts/n8n-api.sh deactivate-workflow [workflow-id]

# Execution Monitoring
scripts/n8n-api.sh list-executions [workflow-id]
scripts/n8n-api.sh get-execution [execution-id]

# Credential Management
scripts/n8n-api.sh list-credentials
scripts/n8n-api.sh create-credential credential.json
scripts/n8n-api.sh test-credential [credential-id]
```

### `scripts/n8n-help.sh` - Quick Reference
Displays comprehensive usage examples and common patterns.

### `scripts/test-api.sh` - Connection Testing
Tests API connectivity and authentication.

---

## 📝 Creating Your First Workflow

### Method 1: Using Claude Code (Recommended)

1. **Start Claude Code in your FlowForge directory**
2. **Ask Claude to create a workflow**, for example:
   ```
   Create an n8n workflow that fetches weather data for Jacksonville, FL 
   and sends a Slack message if rain is expected. Save it as weather-alert.json
   ```
3. **Deploy the workflow:**
   ```bash
   scripts/n8n-api.sh create-workflow weather-alert.json
   scripts/n8n-api.sh activate-workflow [returned-workflow-id]
   ```

### Method 2: Manual JSON Creation

1. **Create a workflow JSON file** (see `sample-workflow.json` for template)
2. **Validate the JSON:**
   ```bash
   scripts/forge validate
   ```
3. **Deploy to n8n:**
   ```bash
   scripts/n8n-api.sh create-workflow my-workflow.json
   ```

### Method 3: Import from n8n UI

1. **Create workflow in n8n web interface** (http://localhost:5678)
2. **Export and manage via API:**
   ```bash
   scripts/n8n-api.sh list-workflows
   scripts/n8n-api.sh get-workflow [workflow-id] > exported-workflow.json
   ```

---

## 🔍 Monitoring and Debugging

### Check Workflow Status
```bash
scripts/n8n-api.sh list-workflows | jq '.data[] | {id, name, active}'
```

### Monitor Executions
```bash
scripts/n8n-api.sh list-executions [workflow-id] | jq '.data[0:5]'
```

### Debug API Issues
```bash
scripts/test-api.sh           # Test API connection
scripts/forge health          # Check n8n status
tail -f ~/n8n.log            # View n8n logs
```

### Validate Workflow JSON
```bash
scripts/forge validate       # Check against n8n schema
scripts/forge format         # Fix JSON formatting
jq . workflow.json           # Validate JSON syntax
```

---

## 🛠️ Advanced Configuration

### Environment Variables
FlowForge uses these environment variables:

- `N8N_API_KEY` - Your n8n API key (set automatically during install)
- `N8N_PORT` - n8n port (default: 5678)
- `N8N_HOST` - n8n host URL (default: http://localhost:5678)

### Custom API Calls
For advanced operations not covered by the wrapper:

```bash
scripts/n8n-api.sh raw GET "workflows/[id]/executions"
scripts/n8n-api.sh raw POST "credentials" '{"name":"my-cred","type":"httpBasicAuth"}'
```

### Shell Script Customization
The API scripts are located in `scripts/` and can be customized:

- Modify `scripts/n8n-api.sh` to add new API operations
- Update `scripts/forge` to add new system management commands
- Extend `scripts/n8n-help.sh` with custom examples

---

## 🔧 Troubleshooting

### Common Issues and Solutions

**"n8n process not found"**
```bash
scripts/forge start
scripts/forge doctor
```

**"API key invalid or expired"**
```bash
# Generate new key at http://localhost:5678/settings/api
export N8N_API_KEY="your-new-key"
echo 'export N8N_API_KEY="your-new-key"' >> ~/.bashrc
scripts/test-api.sh
```

**"Workflow validation failed"**
```bash
scripts/forge validate        # Check schema errors
scripts/forge format          # Fix JSON formatting
jq . workflow.json            # Check JSON syntax
```

**"Connection refused"**
```bash
scripts/forge health          # Check if n8n is running
scripts/forge restart         # Restart n8n
ps aux | grep n8n            # Check process status
```

### Getting Help

- Run `scripts/forge doctor` for comprehensive diagnostics
- Check `scripts/n8n-help.sh` for API usage examples
- View n8n logs: `tail -f ~/n8n.log`
- Consult [CLAUDE.md](../CLAUDE.md) for Claude Code integration details

---

## 🎯 Next Steps

Once you have FlowForge running:

1. **Explore the API** with `scripts/n8n-help.sh`
2. **Create your first automation** using Claude Code
3. **Set up credentials** for external services (Slack, APIs, etc.)
4. **Build complex workflows** with multiple nodes and logic
5. **Monitor executions** and optimize performance

### Useful Resources

- [n8n Documentation](https://docs.n8n.io/)
- [Claude Code Documentation](https://docs.anthropic.com/en/docs/claude-code)
- [FlowForge API Reference](../claude-n8n-tools.md)
- [Sample Workflows](../prompts/)

---

**Happy automating! 🚀**

Need help? Check our [troubleshooting guide](../README.md#troubleshooting) or open an issue on GitHub.