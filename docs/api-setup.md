# FlowForge API Setup Guide

## What You Need

FlowForge requires an **n8n API key** to communicate with your n8n instance. This is a secure token that allows the scripts to manage workflows programmatically.

## Getting Your n8n API Key

### Step 1: Start n8n
```bash
scripts/forge start
```

### Step 2: Open n8n Web Interface
Open your browser and go to: http://localhost:5678

### Step 3: Generate API Key
1. In n8n, click on your profile (bottom left)
2. Go to **Settings** → **n8n API**
3. Click **Create an API key**
4. Copy the generated key (it looks like: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`)

### Step 4: Set Environment Variable
```bash
# Add to your environment
export N8N_API_KEY="your-api-key-here"

# Or add to your .bashrc for persistence
echo 'export N8N_API_KEY="your-api-key-here"' >> ~/.bashrc
source ~/.bashrc
```

### Step 5: Test the Connection
```bash
# Test API connectivity
scripts/n8n-api.sh list-workflows
```

## Using .env File (Recommended)

For better organization, use the `.env` file:

```bash
# Copy the example
cp .env.example .env

# Edit the file
nano .env

# Add your API key
N8N_API_KEY="your-actual-api-key-here"

# Load the environment
source .env
```

## Troubleshooting

### "API key not set" Error
```bash
# Check if the variable is set
echo $N8N_API_KEY

# If empty, set it:
export N8N_API_KEY="your-api-key-here"
```

### "Unauthorized" Error
- Your API key may be expired
- Generate a new one at: http://localhost:5678/settings/api
- Update your environment variable

### "Connection refused" Error
- n8n is not running
- Start it with: `scripts/forge start`
- Check health: `scripts/forge health`

## Security Notes

- **Never commit your API key to git** - it's already in `.gitignore`
- **API keys expire** - you may need to regenerate them periodically
- **Keep your API key private** - it has full access to your n8n instance
- **Use environment variables** - never hardcode keys in scripts

## What About Session Cookies?

You might see references to "session cookies" in older documentation. **You don't need them!** 

Session cookies were used in development but are not required for FlowForge. The API key is the proper authentication method and is all you need.