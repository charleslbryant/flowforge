# Retrospective Topics

This file tracks topics for discussion during retrospective meetings.

## Current Topics for Discussion

### Documentation Integration in Development Process

**Context**: Documentation is critical for user adoption and developer experience, but keeping documentation in sync with development has been challenging in the past.

**Proposal**: Integrate documentation creation and maintenance directly into our development workflow:

1. **Bake Documentation into Process**: Make documentation a required deliverable for every feature, not an afterthought
2. **User Guides**: For user-facing features, commands, and workflows - create step-by-step guides with examples
3. **Developer Guides**: For APIs, SDKs, CLI tools - create technical integration guides with code examples
4. **Sync with Development**: Update documentation as part of the same PR that introduces feature changes
5. **Documentation Standards**: Establish consistent formatting, structure, and quality standards
6. **Review Process**: Include documentation review as part of PR approval process

**Questions for Discussion**:
- What documentation templates should we create to standardize our approach?
- How do we ensure documentation accuracy during rapid development cycles?
- What tooling can help us maintain documentation quality and consistency?
- Should documentation updates be part of Definition of Done for tasks?
- How do we handle documentation for experimental or beta features?

**Expected Outcome**: Clear process for documentation that ensures user and developer guides stay current with development.

### Code Review Agent Development

**Context**: Pull request reviews require significant time and expertise to ensure code quality, security, and architectural consistency. A specialized Code Review Agent could provide automated initial review feedback to help human approvers make better-informed decisions.

**Proposal**: Create a Code Review Agent (system prompt) that can:

1. **Automated Code Analysis**: Review PRs for common issues, patterns, and best practices
2. **Architecture Compliance**: Check adherence to established patterns and ADR decisions  
3. **Security Review**: Identify potential security vulnerabilities or anti-patterns
4. **Documentation Verification**: Ensure code changes include appropriate documentation updates
5. **Test Coverage Analysis**: Verify adequate test coverage for new functionality
6. **Performance Considerations**: Flag potential performance issues or resource concerns

**Questions for Discussion**:
- What specific code review criteria should the agent focus on?
- How should the agent integrate with our GitHub PR process?
- What level of autonomy should the agent have (comment-only vs. approval blocking)?
- How do we ensure the agent stays current with our coding standards and practices?
- Should the agent have different review modes for different types of changes (features vs. fixes)?
- How do we handle false positives and agent feedback quality?

**Expected Outcome**: Automated code review system that enhances human review process and maintains code quality standards.

---

## Completed Topics

*No completed topics yet*

---

## Guidelines for Adding Topics

When adding a retrospective topic:
1. **Provide Context**: Explain the current situation and why this needs discussion
2. **Be Specific**: Include concrete examples or scenarios when possible
3. **Suggest Solutions**: Propose potential approaches or solutions to consider
4. **Ask Questions**: Include specific questions to guide the discussion
5. **Define Success**: Describe what a good outcome would look like

## Topic Template

```markdown
### [Topic Title]

**Context**: [Current situation and background]

**Proposal**: [Suggested approach or solution]

**Questions for Discussion**:
- [Specific question 1]
- [Specific question 2]

**Expected Outcome**: [What success looks like]
```