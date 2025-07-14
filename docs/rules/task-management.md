# Task Management Rules (Concise)

## Overview

All work is tracked via GitHub issues. Work flows from PRD → CRDs → Tasks. The assistant delivers one CRD at a time, one Task at a time.

When in plan mode process PRDs, then CRDs, then tasks before changing to new mode.

## Labels

* **Priority**

  * `now`: Current active work
  * `next`: Ready to start after current
  * `future`: Planned but not ready
* **Status**

  * `created`, `in progress`, `on hold`, `blocked`, `to do`, `done`, `cancelled`
* **Scope**

  * `prd`, `crd`, `task`
* **Type**

  * `feature`, `bug`, `refactor`, `documentation`, `test`, `review`, `release`, `deploy`

## PRD: Product Requirement Document

* **Scope**: High-level feature vision
* **Action**: Do NOT implement directly
* **Process**:

  1. Create PRD issue with label `prd`
  2. Define vision and value with operator
  3. Work with operator to prioritize PRDs (`now`, `next`, `future`)
  4. Break down now PRD into CRDs
  5. Prioritize CRDs (`now`, `next`, `future`)
  6. Mark PRD `done` when all CRDs complete
* **Template**: `.github/ISSUE_TEMPLATE/prd-user-story.md`

## CRD: Change Request Document

* **Scope**: Single user story
* **Action**: Implement in a session
* **Process**:

  1. Select `now` priority CRD
  2. Break into tasks (if not already done)
  3. Work with operator to prioritize tasks (`now`, `next`, `future`)
  4. Use one branch for entire CRD
  5. Mark CRD `done` when all tasks are done and move to PRD process
* **Template**: `.github/ISSUE_TEMPLATE/crd-user-story.md`

## Task

* **Scope**: Specific implementation step
* **Action**: Deliver as a step in one-piece flow
* **Process**:

  1. Select `now` task for current CRD
  2. Implement using TDD
  3. Commit and push to CRD branch
  4. Mark task `done`
* **Template**: `.github/ISSUE_TEMPLATE/task-user-story.md`

## Branch Strategy

* One branch per CRD: `feature/crd-[#]-[desc]`
* All tasks in CRD use same branch
* CRD is complete before merge

## Active Work Rules

* One `now` PRD per operator
* One `now` CRD per session
* One `now` Task at a time

## Completion Rules

* When Task is `done`, prioritize next task in CRD
* When all tasks in CRD are `done`, mark CRD `done` and select next CRD (across PRDs)
* When all CRDs in PRD are `done`, mark PRD `done` and select next PRD

## Priority Decisions

* Product manager decides PRD and CRD priority
* `now` does not mean “do all children” — prioritize explicitly

## Scope Control

* If scope expands: STOP and ask operator
* If unrelated concern found: create new issue
* When unsure: confirm with operator

## Task Planning Checklist

```
- [ ] Review selected PRD or CRD
- [ ] Create missing CRDs or tasks as needed
- [ ] Label all new issues with scope, type, status, priority
- [ ] Confirm priority with operator
- [ ] Assign current "now" issue to self
- [ ] Ensure one-piece flow: only one active task
- [ ] Verify git branch matches CRD
- [ ] Begin work using TDD
```
