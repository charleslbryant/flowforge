# Flow Metrics System

## Overview
Statistical Process Control (SPC) system for tracking flow metrics across our GitHub Issues workflow to enable continuous process improvement.

## Flow States and Transitions

### Issue Lifecycle States
```
Created → Backlog → Ready → In Progress → Review → Delivered → Closed
   ↓        ↓        ↓         ↓          ↓         ↓        ↓
 created  future   next      now     assigned  completed  closed
```

### State Definitions
- **Created**: Issue created in GitHub
- **Backlog**: Labeled `future` (not prioritized yet)
- **Ready**: Labeled `next` (ready for sprint planning)
- **In Progress**: Labeled `now` and assigned to developer
- **Review**: Work completed, under review (optional state)
- **Delivered**: Labeled `completed` (work finished)
- **Closed**: GitHub issue closed

## Flow Metrics Captured

### 1. Throughput Metrics
- **Issues Completed Per Day/Week/Sprint**
  - Total count
  - By type (PRD/CRD/Task)
  - By assignee
  - By priority level

### 2. Flow Time Metrics
- **Total Lead Time**: Created → Closed
- **Work Time**: In Progress → Delivered (active work time)
- **Wait Time**: Created → In Progress (time in backlog)
- **Planning Time**: Created → Ready (grooming/planning)
- **Queue Time**: Ready → In Progress (ready but not started)

### 3. Flow Velocity Metrics
- **Planning Velocity**: Issues per period moving to `next`
- **Development Velocity**: Issues per period moving to `completed`
- **Delivery Rate**: Issues per period moving to `closed`

### 4. Flow Efficiency
- **Flow Efficiency %**: Work Time / Total Lead Time
- **Batch Size**: Average tasks per CRD, CRDs per PRD
- **Rework Rate**: Issues reopened / total completed

## Data Collection Strategy

### GitHub Issue Events Tracked
```json
{
  "issue_number": 123,
  "type": "task|crd|prd",
  "created_at": "2025-01-15T10:00:00Z",
  "state_transitions": [
    {
      "from_state": "created",
      "to_state": "future", 
      "timestamp": "2025-01-15T10:00:00Z",
      "triggered_by": "label_added"
    },
    {
      "from_state": "future",
      "to_state": "next",
      "timestamp": "2025-01-16T09:00:00Z", 
      "triggered_by": "label_changed"
    },
    {
      "from_state": "next",
      "to_state": "now",
      "timestamp": "2025-01-17T08:00:00Z",
      "triggered_by": "assigned"
    },
    {
      "from_state": "now", 
      "to_state": "completed",
      "timestamp": "2025-01-18T17:00:00Z",
      "triggered_by": "label_added"
    },
    {
      "from_state": "completed",
      "to_state": "closed",
      "timestamp": "2025-01-18T17:30:00Z",
      "triggered_by": "issue_closed"
    }
  ],
  "flow_times": {
    "total_lead_time_hours": 31.5,
    "work_time_hours": 9.0,
    "wait_time_hours": 22.0,
    "planning_time_hours": 0.5,
    "queue_time_hours": 22.0
  },
  "assignee": "charleslbryant",
  "parent_issues": ["#121"],
  "sprint": "2025-01-week3"
}
```

### Automated Collection via GitHub Actions

#### Trigger Events
- Issue created
- Issue labeled/unlabeled  
- Issue assigned/unassigned
- Issue closed/reopened
- Issue commented (for status updates)

#### Collection Script
```bash
# Extract flow data on every issue event
gh api repos/:owner/:repo/issues/:issue-number/events \
  --jq '.[] | select(.event | test("labeled|assigned|closed")) | {
    event: .event,
    created_at: .created_at,
    label: .label.name // null,
    assignee: .assignee.login // null
  }'
```

## Flow Metrics Reporting

### 1. Real-time Dashboard
```
Current Flow Status:
- Backlog (future): 12 issues
- Ready (next): 5 issues  
- In Progress (now): 3 issues
- Completed: 2 issues today

Today's Throughput: 2 issues
Weekly Throughput: 8 issues
Average Lead Time: 3.2 days
Flow Efficiency: 35%
```

### 2. Statistical Process Control Charts

#### Control Charts to Generate
- **Throughput Control Chart**: Issues completed per day with control limits
- **Lead Time Control Chart**: Individual issue lead times with control limits
- **Flow Efficiency Trend**: Weekly flow efficiency percentage
- **Cumulative Flow Diagram**: Stacked area chart showing work in each state

#### SPC Analysis
- **Control Limits**: ±3 sigma from mean
- **Special Cause Detection**: Points outside control limits
- **Trend Analysis**: 7+ consecutive points above/below centerline
- **Process Capability**: Cpk calculation for lead time predictability

### 3. Weekly Flow Metrics Report
```markdown
# Weekly Flow Metrics Report - Week of Jan 15, 2025

## Throughput
- Issues Completed: 12 (vs 10 last week) ↗️
- Tasks: 8, CRDs: 3, PRDs: 1
- Assignee Breakdown: charl(8), team(4)

## Flow Times (Average)
- Total Lead Time: 3.4 days (vs 4.1 last week) ↗️
- Work Time: 1.2 days (stable)
- Wait Time: 2.2 days (improved)

## Flow Efficiency
- This Week: 35% (1.2/3.4)
- Last Week: 29% 
- Trend: Improving ↗️

## SPC Signals
- ✅ Process in control - no special causes detected
- 🎯 Lead time within control limits (0.5-6.5 days)
- 📈 7-day improvement trend in wait time

## Process Improvements
- Reduced queue time by prioritizing `next` → `now` transitions
- Batch size optimization: smaller tasks showing faster flow
```

## Implementation Plan

### Phase 1: Data Collection Infrastructure
1. **GitHub Actions Workflow**: Capture all issue events
2. **Flow Data Storage**: JSON files in `/docs/metrics/data/`
3. **State Transition Logic**: Map GitHub events to flow states

### Phase 2: Metrics Calculation
1. **Flow Time Calculations**: Duration between state transitions
2. **Throughput Calculations**: Count per time period
3. **SPC Calculations**: Control limits, process capability

### Phase 3: Reporting & Visualization  
1. **Daily Dashboard**: Real-time flow status
2. **Control Charts**: SPC charts for throughput and lead time
3. **Weekly Reports**: Automated flow metrics summary

### Phase 4: Process Optimization
1. **Special Cause Investigation**: Identify and address outliers
2. **Process Adjustments**: Optimize based on flow metrics trends
3. **Capability Improvement**: Reduce variation in lead times

## Key Benefits

### For Process Management
- **Predictability**: Know how long work takes on average
- **Bottleneck Identification**: See where work gets stuck
- **Capacity Planning**: Understand team throughput capability
- **Process Stability**: SPC charts show when process is in control

### for Continuous Improvement
- **Data-Driven Decisions**: Optimize based on metrics, not intuition
- **Trend Analysis**: See if changes actually improve flow
- **Special Cause Detection**: Quickly identify and address problems
- **Process Capability**: Measure and improve consistency

### For Stakeholder Communication
- **Predictable Delivery**: "Work typically completes in 2-4 days"
- **Transparent Progress**: Real-time visibility into work flow
- **Performance Trends**: Show improvement over time
- **Objective Metrics**: Remove subjectivity from process discussions

This system will give you the statistical foundation needed for true process control and continuous improvement!