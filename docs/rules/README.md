# Rules Overview

This directory defines the core rule files that all assistants and contributors must follow.
Each rule file supports a specific workflow area and is automatically loaded during the appropriate mode.

---

## 📦 Rule Files

### `session-workflow.md`
Defines the overall flow of a development session, including startup, task focus, and checklist compliance.
- **Used by**: `/begin`, `/dev`

### `task-management.md`
Explains how PRDs, CRDs, and tasks are structured and managed via GitHub Issues.
- **Used by**: `/begin`, `/plan`, `/dev`

### `git-workflow.md`
Covers branching strategy, commit standards, PR process, and delivery conventions.
- **Used by**: `/begin`, `/dev`, `/deliver`

### `check-in-formats.md`
Defines standard check-in templates for assistant communication during mode transitions and approvals.
- **Used by**: All modes

### `documentation-rules.md`
Establishes when and how to create or update user guides, developer guides, CLI help, and ADRs.
- **Used by**: `/plan`, `/dev`, `/deliver`, `/qa`

### `tdd-rules.md`
Details the red-green-refactor cycle and assistant behavior for test-first development in `/dev` mode.
- **Used by**: `/dev`

### `todowrite-templates.md`
Provides raw checklist templates for TodoWrite. Used to seed checklists in each mode. 
- **Used by**: All modes (implicitly)

---

## 📌 Rule Enforcement

- Rules are re-read at the start of each mode
- Rules support checklist generation and assistant behavior
- Session context is updated based on rule execution

---

## ✍️ How to Add a Rule
1. Create a new `.md` file in this folder
2. Keep it atomic — one topic per file
3. Reference it from a mode in `modes/index.yaml`
4. Add it here to keep this summary up to date

---
*Rules are structured for precision, context efficiency, and automation compliance.*
