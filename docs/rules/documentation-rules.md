# Documentation Rules

## When Documentation is Required

### User-Facing Features
- **Requirement**: Any feature users interact with directly
- **Location**: `/docs/user-guides/`
- **Content**: Step-by-step instructions, examples, troubleshooting

### Developer/Technical Features  
- **Requirement**: APIs, SDKs, CLI tools, technical integrations
- **Location**: `/docs/developer-guides/`
- **Content**: Code examples, API reference, integration guides

### Feature Updates
- **Requirement**: Every new feature or significant change
- **Actions**: 
  - Update existing relevant guides
  - Create new guides for new user workflows
  - Update CLI help text and command descriptions

## Documentation Standards

### Content Requirements
- Clear, concise language for target audience
- Practical examples and real-world usage scenarios
- Consistent formatting and structure
- Links to related documentation
- Clear navigation paths

### Integration with Development
- Write documentation alongside code development
- Test documentation accuracy during feature testing
- Include documentation review in PR process
- Keep documentation in version control with code

### Maintenance
- Review and update during each development cycle
- Mark outdated documentation during retrospectives
- Treat documentation as first-class citizen alongside code

## TodoWrite Integration
For features requiring documentation:
- [ ] Identify if feature needs user guides or developer guides
- [ ] Create documentation draft alongside code development
- [ ] Test documentation accuracy during feature testing
- [ ] Include documentation review in PR checklist
- [ ] Update existing guides that reference the feature