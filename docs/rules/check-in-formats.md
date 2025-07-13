# Check-in Format Rules

## When to Check-in
1. **Before Starting Major Work** - Before CRD, major task, or significant development
2. **Before Push/PR Creation** - After development but before pushing to remote
3. **After Major Completion** - After completing significant work
4. **End of Session** - When all planned work is complete

## Check-in Templates

### 1. Starting Work Check-in
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

### 2. Ready to Push Check-in
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

### 3. Major Work Completion Check-in
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

### 4. End of Session Check-in
```
## Session Completion
**Session summary**: [What was accomplished]
**Current state**: [Where the project stands now]
**Recommended next session focus**: [Specific next steps]
**Operator next steps**: [What you should do - usually /clear context]
```

## Check-in Rules
- **Always wait for operator response** before proceeding after check-in
- **Provide clear options** rather than open-ended questions
- **Include recommendations** but make it clear operator decides
- **Keep check-ins brief** but informative
- **Use consistent formatting** for easy scanning

## TodoWrite Integration
Add check-in reminders to todos:
- [ ] Check-in before starting major development work
- [ ] Check-in before pushing and creating PR
- [ ] Check-in after completing major milestones
- [ ] End session with completion check-in