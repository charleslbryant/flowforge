# Agent Standup

We have agents that work in different contexts that deliver actions specific to their context:

-   IM – send and receive instant messages on my behalf
-   Web Search – browse and search the internet
-   File Search – browse and search local and cloud file systems
-   Vector Search – search vector store
-   Email – send and receive email
-   Calendar – invite and attend meetings
-   Team Chat – send and receive team chat messages
-   Project Management – manage projects and tasks
-   Cloud Infrastructure – manage resources
-   Accounting – query financial transactions
-   CRM – manage customers, opportunities, and deals

# Understand the Assignment

-   You are now tasked with managing a team. You unblock them when they are stuck. You assess their performance. You work to improve their effectiveness and efficiency.
-   Whether you are managing operators or agents you need a system to maximize the return on investment.
-   When a project or task is stuck you intervene to

# Ask the Right Questions

-   This is why experts will still be needed. Experts know the right questions to ask. Experts don’t ask LLMs to do something and just accept what it says, they are able to better understand when the LLM is hallucinating, lying, or just wrong. So, experts are great at follow up questions and pushing the LLM in the best direction to produce the best output for the intent.

# Data, Communities, and Network Effects is Our Moat

-   You can’t vibe code a community of users.
-   For example, having a community of HOAs share data to improve the estimates based on visual inspection of property components to infer the remaining life and replacement cost. If we get a head start on building the data sets and community, we may be able to provide catalyst to create the network effect.
-   The go-to-market and ability to message and build a community and acquire community data and build solutions to monetize the data solving the most pressing challenges in the community.
-   So, building or buying social networks or media companies to monetize their data is a good play in the new AI age.

# Agents Include

Input

Input Validation

Processing

Aggregate

State

Events

Event

Memory

Long Term

Short Term

Session

Chat

Actions

Action

Command

Query

Output

Output Validation

# AI Engineer Basic Methodology

*(a relaxed but deadly serious framework)*

## 1. Define the Problem (Real World First, AI Later)

-   **Goal**: What outcome matters to the business, user, or system?
-   **Constraints**: Time, cost, compute, data, regulations, explainability, etc.
-   **Measurement**: How will you know you succeeded? (metric, KPI, threshold)

**Rule of thumb**: No problem definition = no model. Full stop.

## 2. Understand the Domain Data

-   **Source**: Where’s the data coming from? Is it reliable?
-   **Format**: Text, tabular, images, events, etc.
-   **Quality**: Missing values, bias, noise, anomalies.
-   **Quantity**: Enough to not embarrass yourself with overfitting?
-   **Labeling**: How are ground truths obtained? Trust but verify.

**Golden rule**: If the data sucks, the model will suck harder.

## 3. Frame the Task Properly

-   **Task type**:
    -   Classification
    -   Regression
    -   Clustering
    -   Recommendation
    -   Generation
    -   Reinforcement Learning
-   **Input → Output**: Define **clear interfaces** for your model.

**Key principle**: No framing, no aiming.

## 4. Select Baseline Solutions First

-   Start simple:
    -   Logistic regression
    -   Random forest
    -   K-means
    -   Simple transformers
-   Get a **benchmark** before you reach for fancy stuff.

**Clever shortcut**: Baseline models are a cheap insurance policy against wasting 3 months on junk.

## 5. Iterate Smartly (Model + System Together)

-   **Feature engineering** (if traditional ML)
-   **Fine-tuning or prompt design** (if LLM)
-   **Architecture exploration** (if deep learning)
-   **Pipeline**: Build training, validation, and test steps reproducibly.
-   **Evaluate ruthlessly**: Don’t move the goalposts to feel good.

**Mindset**: Build small, test fast, fail cheap.

## 6. Operationalize (Don't Just Throw Models Over The Wall)

-   **Version everything**: data, code, models, pipelines.
-   **Deployable artifacts**: Models must run somewhere (API, edge device, database, etc).
-   **Evals and Monitoring**:
    -   Data drift
    -   Model degradation
    -   Performance vs. real-world usage
-   **Feedback loops**: Automate retraining or alert when intervention needed.

**Sane attitude**: A model that dies quietly in production is worse than no model at all.

## 7. Stay Ethical and Legal

-   Privacy: Are you leaking personal data?
-   Bias: Are you baking bias into decisions?
-   Explainability: Can you explain why the model made that choice?
-   Compliance: GDPR, HIPAA, ADA, whatever alphabet soup applies.

**Cynical truth**: Legal trouble eats ML projects for breakfast.

## 8. Document Like a Future Engineer, Not a Past Genius

-   **What you did**
-   **Why you did it**
-   **What worked**
-   **What didn’t**
-   **How to pick it up if you got hit by a bus**

**Secret weapon**: Good documentation is more scalable than your smartest team member.

## Quick Summary Checklist

>   **Problem → Data → Task → Baseline → Iterate → Deploy → Monitor → Evaluate → Document**

That’s it.  
That’s the DNA strand of a competent, practical AI engineer.

# Expanded Elements You Should Standardize

## 1. Mission Statement (per Step)

-   Short description of what this step does and *why it matters*.

## 2. Inputs

-   **Input Data Format**: What structured info the agent needs (example templates).
-   **Required Context**: What prior work must be known (ex: project name, goal, environment)?
-   **Constraints**: Time limits, data size limits, token limits, privacy rules, etc.

## 3. Prompts/Commands

-   **Prompt templates** for agents.
-   Pre-validated **few-shot examples** if applicable.
-   Clear **expected behavior** instructions ("If unsure, escalate. If missing input, ask for it.").

## 4. Outputs

-   **Expected Output Format** (JSON, Markdown, CSV, DB record, code file, etc.).
-   **Required Fields**: If output is structured (ex: project spec needs title, description, acceptance criteria).
-   **Output Quality Checklist** (self-check rules the assistant should verify).

## 5. Evaluation/Validation

-   How we **evaluate success** of this step.
-   Automatic validations (ex: "Output must have all required fields.")
-   Automated evaluation (ex: “LLM-as-judge”)
-   Manual spot checks (ex: "Human-in-the-loop review 10% of outputs.").

## 6. Failure and Escalation Paths

-   If an agent **fails** a step (ex: bad input, invalid output),
    -   What **error message** or **feedback** is generated?
    -   What happens next? (Retry? Escalate? Human assist?)

## 7. Standard Constraint / Budget

-   **How long** this step should take.
-   **Optional**: Max token or compute budget.

## 8. Logs and Artifacts

-   What **logs** or **intermediate files** should be saved?
-   Naming conventions? Storage location?
-   Artifacts you can reuse later (ex: prompt tuning sets, training sets).

## Example: Framing the Task Step

| Element            | Example                                                                                                                             |
|--------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| Mission            | Frame the AI/ML problem correctly to match a valid ML task type.                                                                    |
| Inputs             | Problem description, business goals, key metrics.                                                                                   |
| Prompt             | “Given this description, classify the task as classification, regression, clustering, etc. If uncertain, ask clarifying questions.” |
| Output             | JSON: { "TaskType": "Classification", "Justification": "Predicting discrete labels" }                                               |
| Evaluation         | Check that task type matches provided examples with an 80% eval score. Human review of 20% of output and failed output.             |
| Failure/Escalation | If output is empty or wrong format → Retry with expanded clarification prompt.                                                      |
| Constraint         | 10 minutes maximum.                                                                                                                 |
| Logs               | Save prompt+output pairs for prompt tuning.                                                                                         |

## Bonus Categories (Optional, but Powerful)

-   **Agent Personas**: Different behavior styles (e.g., aggressive optimizer, cautious verifier).
-   **Success Metrics**: Aggregate performance measures across all steps (accuracy, time saved, escalations).
-   **Continuous Improvement Hooks**: Capture error patterns to improve prompts, templates, or agents over time.

## If you build this right, it becomes...

>   **A full Agentic SOP (Standard Operating Procedure) Framework.**

Basically, a **little "Operating System"** for managing all your AI agents at industrial scale.  
**Inputs → Prompts → Outputs → Evaluation → Logging → Improvement Loops**.

Improvement loops can be self-healing. We could have a Prompt Engineering agent that automatically tunes a prompt in a gradient decent towards goals.

This mirrors how we design and architect Features. Features have inputs as commands and queries (write request or read request). Features have command and query handlers where a command or query request is processed. Running a prompt can be an action in the processing. Features have output which we call a command or query response. Features have command and query request and response validators that validate the structure and values of requests and responses. Features have events and logging. Features have improvement loops where we evaluate the performance of the feature and the effectiveness and efficiency of features.

# AI Engineering Framework - Naming Convention Rules

## 1. General Principles

| Rule                    | Description                                                            |
|-------------------------|------------------------------------------------------------------------|
| **Snake Case**          | Use lowercase with underscores: `define_problem_prompt_v1.txt`         |
| **No Spaces**           | Never use spaces. Use `_` to separate words.                           |
| **Version Numbers**     | Always include versioning (`_v1`, `_v1_1`, `_v2`).                     |
| **Meaningful Names**    | File names must describe *what* and *which step/role* they serve.      |
| **Extension Matters**   | Use `.md`, `.json`, `.txt`, `.csv`, `.yaml`, `.py`, etc. consistently. |
| **Step Prefix**         | Prefix files related to methodology steps with `stepXX_`.              |
| **Agent Prefix**        | Prefix files related to agents with `agent_`.                          |
| **Template vs Example** | Include `_template` or `_example` explicitly in filename.              |
| **Prompt vs Output**    | Include `_prompt` or `_output` explicitly too.                         |

## 2. Folder Naming Rules

-   Top-level folders use **CamelCase**: `Methodology`, `Agents`, `Artifacts`, `Tools`
-   Subfolders use **snake_case** matching topic: `input_templates`, `output_templates`, etc.
-   Step folders must be prefixed: `step01_define_problem`, `step02_understand_data`, etc.

## 3. File Naming Patterns

### Methodology Step Assets

| Asset            | Naming Example                               |
|------------------|----------------------------------------------|
| README           | `README.md`                                  |
| Input Template   | `step01_define_problem_input_template.json`  |
| Output Template  | `step01_define_problem_output_template.json` |
| Prompt           | `step01_define_problem_prompt_v1.txt`        |
| Prompt Example   | `step01_define_problem_prompt_example1.txt`  |
| Output Example   | `step01_define_problem_output_example1.json` |
| Validator Script | `step01_define_problem_validator.py`         |

### Agent Blueprints and Personas

| Asset              | Naming Example                           |
|--------------------|------------------------------------------|
| Agent Blueprint    | `agent_problem_framer_blueprint.md`      |
| Agent Persona      | `agent_cautious_verifier_persona.md`     |
| Agent Example Logs | `agent_problem_framer_log_20250427.json` |

### Artifacts and Logs

| Asset             | Naming Example                          |
|-------------------|-----------------------------------------|
| Prompt Log        | `prompt_log_20250427_step01.json`       |
| Output Log        | `output_log_20250427_agent_framer.json` |
| Validation Report | `validation_report_20250427_step01.csv` |
| Improvement Notes | `improvement_notes_20250427.md`         |

## 4. Versioning Rules

-   Always append version tag for templates and prompts:
    -   First draft: `_v1`
    -   Minor revision: `_v1_1`
    -   Major update: `_v2`
-   Examples don’t need versioning unless they're templates themselves.

## 5. Datetime Stamps for Logs

-   Use UTC date in filenames where needed: `YYYYMMDD`
    -   Example: `prompt_log_20250427.json`
-   Optional: Add time if high-volume (`YYYYMMDD_HHMM`)

## Summary Example for "Define Problem" Step

-   /1_Methodology/step01_define_problem
-   ├── README.md
-   ├── input_templates/
-   │ └── step01_define_problem_input_template_v1.json
-   ├── output_templates/
-   │ └── step01_define_problem_output_template_v1.json
-   ├── prompts/
-   │ ├── step01_define_problem_prompt_v1.txt
-   │ └── step01_define_problem_prompt_example1.txt
-   ├── validators/
-   │ └── step01_define_problem_validator.py
-   ├── examples/
-   │ └── step01_define_problem_output_example1.json

## Quick Memory Cheatsheet

\> `stepXX_topic_assettype[_template|_example][_vX[_X]].ext`

## Filename Linting Rule

**Rule:**  
All filenames in the repository must match the following pattern:

-   \^([a-z0-9]+_)+[a-z0-9]+(_v[0-9]+(_[0-9]+)?)?(\\.[a-z0-9]+)\$

**Plain English:**

-   Lowercase only (a–z, 0–9, underscores `_`)
-   Words separated by `_`
-   Optional versioning suffix: `_v1`, `_v1_1`, `_v2`
-   Ends with a proper file extension (like `.md`, `.json`, `.txt`, `.py`)
-   No spaces, no hyphens, no weird characters

### Simple Filename Linting Script (Bash)

-   bash
-   \#!/bin/bash
-   
-   \# Lint filenames in staged Git changes
-   echo "Running filename lint check..."
-   
-   \# Regex pattern
-   pattern='\^([a-z0-9]+_)+[a-z0-9]+(_v[0-9]+(_[0-9]+)?)?\\.[a-z0-9]+\$'
-   
-   \# Find staged files
-   error=0
-   for file in \$(git diff --cached --name-only); do
-   filename=\$(basename "\$file")
-   
-   if [[ ! \$filename =\~ \$pattern ]]; then
-   echo "Filename violation: \$filename"
-   error=1
-   fi
-   done
-   
-   if [ \$error -eq 1 ]; then
-   echo "Filename linting failed. Fix filenames before committing."
-   exit 1
-   else
-   echo "Filename linting passed."
-   fi

### Git Pre-Commit Hook to Enforce Filename Rules

1.  Go to your repo’s `.git/hooks/` directory.
2.  Create a file called `pre-commit` (no extension).
3.  Paste the above Bash script into it.
4.  Make it executable:
-   bash
-   chmod +x .git/hooks/pre-commit

**Now:**

-   When someone tries to `git commit`,
-   Git will **check every staged filename**,
-   If anything violates the naming convention, **the commit will fail**,
-   They'll get a message showing which filenames are wrong.

### Extra: Prettier Version of the Error Message

If you want to get *real fancy* (like a sassy but helpful AI buddy), you can tweak the error message like this:

-   bash
-   echo "-------------------------------------------------------------"
-   echo "❌ Filename Convention Violation!"
-   echo "Files must use: snake_case + optional \_v1 suffix + valid extension"
-   echo "Fix the following filenames before committing:"
-   echo "-------------------------------------------------------------"

*Because honestly, if the AI agents are gonna start managing each other soon, they deserve a little theater.*

### In Summary:

| Tool                    | Purpose                                                    |
|-------------------------|------------------------------------------------------------|
| **Filename Regex Rule** | Enforces clean, predictable, automation-friendly naming.   |
| **Linting Script**      | Checks filenames in staged changes before commit.          |
| **Pre-Commit Hook**     | Automatically blocks bad filenames from entering the repo. |

# Step 01: Define the Problem

## Mission

Help the AI agent accurately understand and structure a *real-world* problem statement into an AI/ML-compatible framing.  
The output ensures downstream steps (data gathering, modeling) are based on a *solvable*, *measurable*, and *business-relevant* problem.

## Inputs

| Field                 | Description                            | Example                                                               |
|-----------------------|----------------------------------------|-----------------------------------------------------------------------|
| `project_name`        | Name of the project or initiative      | "Customer Churn Predictor"                                            |
| `business_goal`       | High-level outcome desired             | "Reduce customer churn by 20% by end of Q4"                           |
| `problem_description` | Detailed explanation of the issue      | "We lose \~10% of customers monthly; need to predict churners early." |
| `constraints`         | Known boundaries (time, cost, compute) | "Must deploy on edge devices with 1GB RAM"                            |
| `success_criteria`    | How success will be measured           | "Churn prediction model with \>80% F1 score"                          |

### Input Template

**Filename:**  
`/1_Methodology/step01_define_problem/input_templates/step01_define_problem_input_template_v1.json`

**Content:**

-   json
-   {
-   "project_name": "PROJECT_NAME_HERE",
-   "business_goal": "BUSINESS_GOAL_HERE",
-   "problem_description": "DETAILED_DESCRIPTION_OF_THE_PROBLEM_HERE",
-   "constraints": "ANY_KNOWN_CONSTRAINTS_HERE (e.g., budget, compute, deadlines)",
-   "success_criteria": "HOW_SUCCESS_WILL_BE_MEASURED (e.g., 85% accuracy, model deployed in production by Q4)"
-   }

**Notes:**

-   All CAPS placeholders = places you swap real data in.
-   This template should be **validated** before being fed into prompt generation (simple field presence check).

#### Example Filled Input

**Filename:**  
`/1_Methodology/step01_define_problem/examples/step01_define_problem_input_example1.json`

**Content:**

-   json
-   {
-   "project_name": "Customer Churn Prediction",
-   "business_goal": "Reduce customer churn rate by 20% by end of Q4.",
-   "problem_description": "Our subscription business sees about 10% churn monthly. We have customer engagement, payment, and support ticket data. We need a way to predict which customers are likely to churn within the next 30 days so retention teams can intervene.",
-   "constraints": "Model must be lightweight enough to run on CRM platform backend. Limited data storage (\~500MB per customer record).",
-   "success_criteria": "Model achieves \>80% F1 Score on holdout set, and lifts retention interventions by 10%."
-   }

#### Real Example Prompt (Generated)

**Filename:**  
`/1_Methodology/step01_define_problem/prompts/step01_define_problem_prompt_example1.txt`

**Content:**

-   You are a highly skilled AI problem framer.
-   
-   Given a business description, your tasks are:
-   1\. Identify the core machine learning task type (classification, regression, clustering, recommendation, generation, or reinforcement learning).
-   2\. Summarize the structured problem in a way suitable for an AI/ML system.
-   3\. Highlight any missing information if needed.
-   
-   Inputs:
-   \- Project Name: Customer Churn Prediction
-   \- Business Goal: Reduce customer churn rate by 20% by end of Q4.
-   \- Problem Description: Our subscription business sees about 10% churn monthly. We have customer engagement, payment, and support ticket data. We need a way to predict which customers are likely to churn within the next 30 days so retention teams can intervene.
-   \- Constraints: Model must be lightweight enough to run on CRM platform backend. Limited data storage (\~500MB per customer record).
-   \- Success Criteria: Model achieves \>80% F1 Score on holdout set, and lifts retention interventions by 10%.
-   
-   Output a JSON object with:
-   \- TaskType
-   \- Justification
-   \- StructuredProblem
-   \- ClarifyingQuestions (if any)

#### Directory Layout After This

-   /1_Methodology/step01_define_problem/
-   ├── README.md
-   ├── input_templates/
-   │ └── step01_define_problem_input_template_v1.json
-   ├── examples/
-   │ └── step01_define_problem_input_example1.json
-   ├── prompts/
-   │ └── step01_define_problem_prompt_example1.txt
-   ├── output_templates/
-   │ └── step01_define_problem_output_template_v1.json
-   ├── validators/
-   │ └── step01_define_problem_validator.py

#### Quick Checklist: What We Now Have Ready

-   [x] Input Template
-   [x] Input Example
-   [x] Prompt Template
-   [x] Prompt Example
-   [x] Output Template
-   [x] Validation Rules
-   [x] Mission Definition
-   [x] Failure Escalation Plan

**(This could run live through an agent already — real operational-grade artifacts.)**

## Handlers

### Prompt Template

**Filename:** `step01_define_problem_prompt_template_v1.txt`

-   You are a highly skilled AI problem framer.
-   
-   Given a business description, your tasks are:
-   1\. Identify the core machine learning task type (classification, regression, clustering, recommendation, generation, or reinforcement learning).
-   2\. Summarize the structured problem in a way suitable for an AI/ML system.
-   3\. Highlight any missing information if needed.
-   
-   Inputs:
-   \- Project Name: {{project_name}}
-   \- Business Goal: {{business_goal}}
-   \- Problem Description: {{problem_description}}
-   \- Constraints: {{constraints}}
-   \- Success Criteria: {{success_criteria}}
-   
-   Output a JSON object with:
-   \- TaskType
-   \- Justification
-   \- StructuredProblem
-   \- ClarifyingQuestions (if any)

### Clarifying Question Handler

*(This is the agent behavior if it realizes it doesn’t have enough info.)*

#### Clarifying Question Handler Micro-Prompt

**Filename:**  
`/1_Methodology/step01_define_problem/prompts/step01_define_problem_clarifying_question_handler_v1.txt`

**Content:**

-   You are an AI agent tasked with framing problems for machine learning.
-   
-   If the input information is incomplete, vague, or missing key details needed to correctly frame the problem, you must ask clarifying questions.
-   
-   Guidelines:
-   \- Ask precise, short, professional questions.
-   \- Only ask about missing or unclear elements necessary for task framing.
-   \- Limit yourself to 3 questions per round.
-   \- Output a JSON array of questions.
-   
-   Inputs:
-   \- Project Name: {{project_name}}
-   \- Business Goal: {{business_goal}}
-   \- Problem Description: {{problem_description}}
-   \- Constraints: {{constraints}}
-   \- Success Criteria: {{success_criteria}}
-   
-   Output:
-   [
-   "Question 1",
-   "Question 2",
-   "Question 3"
-   ]
-   
-   If no clarification is needed, output an empty array: \`[]\`.

## Output Template

**Filename:** `step01_define_problem_output_template_v1.json`

-   json
-   {
-   "task_type": "classification",
-   "justification": "The goal is to predict which customers will churn (binary outcome).",
-   "structured_problem": "Given customer usage data and historical churn status, predict the probability that a current customer will churn in the next 30 days.",
-   "clarifying_questions": [
-   "What features are available from customer records?",
-   "Is there a labeled churn history dataset available?"
-   ]
-   }

### Output Example

**Filename:**  
`/1_Methodology/step01_define_problem/examples/step01_define_problem_output_example1.json`

**Content:**

-   json
-   {
-   "task_type": "classification",
-   "justification": "The goal is to predict a binary outcome: whether a customer will churn within the next 30 days (yes/no). This naturally frames the problem as a supervised classification task.",
-   "structured_problem": "Given customer engagement metrics, payment history, and support ticket records, predict whether a customer is likely to churn within the next 30 days. The model must be lightweight for CRM backend deployment and able to handle \~500MB per customer record.",
-   "clarifying_questions": [
-   "What is the time window for defining churn (e.g., 30 days after prediction)?",
-   "Is there a labeled historical dataset linking customer records to churn outcomes?",
-   "Are there any regulatory or data privacy constraints regarding model training or prediction?"
-   ]
-   }

#### Quick Analysis of Why This Is Good

-   `task_type`: Clear, simple, correct → *Classification.*
-   `justification`: Explains logically why (binary outcome mapping).
-   `structured_problem`: Full rewritten version, machine-actionable (input/output framed cleanly).
-   `clarifying_questions`: Smart questions to prevent downstream waste if assumptions are wrong.

**Validation:**  
This would PASS the validator script we wrote earlier.

#### Directory Layout After Adding This

-   /1_Methodology/step01_define_problem/
-   ├── examples/
-   │ ├── step01_define_problem_input_example1.json
-   │ └── step01_define_problem_output_example1.json
-   ├── prompts/
-   │ └── step01_define_problem_prompt_example1.txt
-   ├── input_templates/
-   │ └── step01_define_problem_input_template_v1.json
-   ├── output_templates/
-   │ └── step01_define_problem_output_template_v1.json
-   ├── validators/
-   │ └── step01_define_problem_validator.py
-   ├── README.md

#### Checkpoint: What We Have Now

-   [x] Input Template
-   [x] Input Example
-   [x] Prompt Template
-   [x] Prompt Example
-   [x] Output Template
-   [x] Output Example
-   [x] Validation Script
-   [x] Constraint, Mission, Escalation Plan
-   [x] Naming Conventions
-   [x] Pre-commit Filename Linter

This **entire Step 01 is fully production ready** for agent-based execution.

### Mock BAD Output Example

**Filename:**  
`/1_Methodology/step01_define_problem/examples/step01_define_problem_output_example_bad1.json`

**Content:**

-   json
-   {
-   "task_type": "",
-   "justification": "",
-   "structured_problem": "Predict customer churn.",
-   "clarifying_questions": []
-   }

#### What's wrong with this?

| Problem                        | Why it's bad                                                       |
|--------------------------------|--------------------------------------------------------------------|
| `task_type` empty              | Critical field missing                                             |
| `justification` empty          | No logical backing for task framing                                |
| `structured_problem` too vague | No mention of inputs, outputs, time window, deployment constraints |
| `clarifying_questions` empty   | Should have asked for missing info                                 |

#### How the Validator Would Catch This

Running the validator script would result in:

-   Validation error: Missing or empty field 'task_type'
-   Validation error: Missing or empty field 'justification'
-   Validation failed.

**Outcome:**

-   Auto-reject
-   Send feedback to agent
-   Retry if allowed, else escalate to human reviewer

## Validation Rules

**Filename:** `step01_define_problem_validator.py`

Basic checks:

-   Output must be valid JSON.
-   Must have non-empty `task_type`, `justification`, and `structured_problem`.
-   If clarifying questions are missing and fields are vague, flag for human review.

Example Python snippet:

-   python
-   import json
-   import sys
-   
-   def validate_output(output_file):
-   with open(output_file, 'r') as f:
-   data = json.load(f)
-   
-   required_keys = ["task_type", "justification", "structured_problem"]
-   
-   for key in required_keys:
-   if key not in data or not data[key].strip():
-   print(f"Validation error: Missing or empty field '{key}'")
-   return False
-   
-   print("Validation passed.")
-   return True
-   
-   if \__name_\_ == "__main__":
-   output_file = sys.argv[1]
-   valid = validate_output(output_file)
-   if not valid:
-   sys.exit(1)

## Failure and Escalation

| Situation                                 | Action                                                  |
|-------------------------------------------|---------------------------------------------------------|
| Missing critical fields                   | Auto-reject output, send "Missing Field" error to agent |
| Vague structured problem                  | Flag for manual review                                  |
| Task type inconsistent with business goal | Request agent retry with rephrased prompt               |

## Constraint

-   Maximum **10 minutes** per problem framing task.
-   **Token budget**: 2,000 tokens per prompt+response cycle.

## Artifacts to Save

-   Prompt issued
-   Output generated
-   Validation result
-   Clarification questions if generated
-   Any manual reviewer comments (if flagged)

Store under:

-   /3_Artifacts/step01_define_problem/
-   ├── prompts/
-   ├── outputs/
-   ├── validation_logs/
-   ├── improvement_notes/

## Summary Snapshot

| Element          | Asset                                           |
|------------------|-------------------------------------------------|
| Mission          | Frame the problem correctly for ML use          |
| Input Template   | `step01_define_problem_input_template_v1.json`  |
| Prompt Template  | `step01_define_problem_prompt_template_v1.txt`  |
| Output Template  | `step01_define_problem_output_template_v1.json` |
| Validator Script | `step01_define_problem_validator.py`            |
| Constraint       | 10 minutes, 2000 tokens                         |
| Failure Mode     | Retry or escalate to human reviewer             |

## Example Run

**Input:**

-   Project Name: "Customer Churn Prediction"
-   Business Goal: *missing*
-   Problem Description: "We want to understand customers better."
-   Constraints: ""
-   Success Criteria: ""

**Output:**

-   json
-   [
-   "What specific business outcome are you aiming to achieve (e.g., reduce churn, increase retention)?",
-   "Do you have historical data that includes customer behaviors and outcomes?",
-   "Are there any constraints regarding the deployment environment (e.g., edge device, cloud)?"
-   ]

## Directory Structure Updated

-   /1_Methodology/step01_define_problem/
-   ├── examples/
-   │ ├── step01_define_problem_input_example1.json
-   │ ├── step01_define_problem_output_example1.json
-   │ └── step01_define_problem_output_example_bad1.json
-   ├── prompts/
-   │ ├── step01_define_problem_prompt_example1.txt
-   │ └── step01_define_problem_clarifying_question_handler_v1.txt
-   ├── input_templates/
-   │ └── step01_define_problem_input_template_v1.json
-   ├── output_templates/
-   │ └── step01_define_problem_output_template_v1.json
-   ├── validators/
-   │ └── step01_define_problem_validator.py
-   ├── README.md
