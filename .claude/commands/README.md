# FlowForge Mode System

FlowForge uses a mode-based workflow system to ensure structured, context-aware development across assistants and contributors. Each mode has its own responsibilities, tools, and transition logic.

---

## 🚀 Available Modes

### `/begin` — Session Startup Mode
Prepares environment and selects a task to work on.
- **Checklist**: `begin`
- **Transitions**: `/dev`, `/plan`
- **Responsibilities**:
  - Load all rule and session context files
  - Select a single "now" task
  - Create feature branch
  - Write session state to `ACTIVE_SESSION.md`

---

### `/plan` — Task Planning Mode
Breaks down PRDs/CRDs into tasks and prioritizes them.
- **Checklist**: `plan`
- **Transitions**: `/dev`, `/deliver`
- **Responsibilities**:
  - Read all rule and context files
  - Break down work into CRDs and tasks
  - Set priorities and labels
  - Update session context before exit

---

### `/dev` — Development Mode (TDD)
Implements a single task using strict test-driven development.
- **Checklist**: `dev`
- **Transitions**: `/plan`, `/deliver`
- **Responsibilities**:
  - Follow red-green-refactor cycle
  - Commit code with proper attribution
  - Update context and documentation
  - Prepare for PR and handoff

---

### `/deliver` — Delivery Mode
Prepares work for review, creates PRs, and finalizes documentation.
- **Checklist**: `deliver`
- **Transitions**: `/begin`, `/dev`, `/qa`
- **Responsibilities**:
  - Final testing and documentation
  - PR creation and Git workflow
  - Session wrap-up and state update

---

### `/qa` — QA Mode
Verifies feature functionality, tests, and documentation.
- **Checklist**: `qa`
- **Transitions**: `/begin`, `/dev`
- **Responsibilities**:
  - Confirm all acceptance criteria and documentation are met
  - Validate PR merged and system state is clean
  - Raise issues for any gaps or defects

---

## 🔁 Mode Transition Overview
| From Mode | Can Transition To         |
|-----------|---------------------------|
| `/begin`  | `/dev`, `/plan`           |
| `/plan`   | `/dev`, `/deliver`        |
| `/dev`    | `/plan`, `/deliver`       |
| `/deliver`| `/begin`, `/dev`, `/qa`   |
| `/qa`     | `/begin`, `/dev`          |

---

## 🧠 Mode Context Files
Each mode starts by re-reading the following:
- Rule files (specific to mode)
- Product and architecture context
- Session files:
  - `CURRENT_STATE.md`
  - `NEXT_TASKS.md`
  - `ACTIVE_SESSION.md`

This ensures clean context boundaries between modes and reduces memory overload.

---

## ✅ Compliance Automation
- All modes use TodoWrite checklists
- Mode transitions require `/clear` to reset context
- Session state must be written before switching
- Rule reading is mandatory at each entry

---
*Generated from `modes/index.yaml`*
