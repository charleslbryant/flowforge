#!/bin/bash

# Flow Metrics Analysis Script
# Statistical Process Control (SPC) analysis for GitHub Issues flow

set -e

METRICS_DIR="docs/metrics/data"
REPORTS_DIR="docs/metrics/reports"

# Ensure directories exist
mkdir -p "$REPORTS_DIR"

echo "📊 FlowForge Flow Metrics Analysis"
echo "=================================="

# Function to calculate flow times from issue events
calculate_flow_times() {
    local issue_file="$1"
    
    if [ ! -f "$issue_file" ]; then
        echo "Issue file not found: $issue_file"
        return 1
    fi
    
    # Extract key timestamps
    local created_at=$(jq -r '.created_at' "$issue_file")
    local closed_at=$(jq -r '.closed_at // empty' "$issue_file")
    
    if [ -z "$closed_at" ]; then
        echo "Issue not closed yet"
        return 1
    fi
    
    # Calculate total lead time in hours
    local lead_time_seconds=$(( $(date -d "$closed_at" +%s) - $(date -d "$created_at" +%s) ))
    local lead_time_hours=$(echo "scale=2; $lead_time_seconds / 3600" | bc)
    
    echo "$lead_time_hours"
}

# Function to generate throughput report
generate_throughput_report() {
    echo "📈 Generating Throughput Analysis..."
    
    # Get last 30 days of daily metrics
    local start_date=$(date -d "30 days ago" +%Y-%m-%d)
    local throughput_data=""
    
    for i in {0..29}; do
        local check_date=$(date -d "$start_date + $i days" +%Y-%m-%d)
        local daily_file="$METRICS_DIR/daily-${check_date}.json"
        
        if [ -f "$daily_file" ]; then
            local throughput=$(jq -r '.throughput.total' "$daily_file")
            throughput_data="$throughput_data $throughput"
        else
            throughput_data="$throughput_data 0"
        fi
    done
    
    # Calculate throughput statistics
    local values=($throughput_data)
    local count=${#values[@]}
    local sum=0
    
    for value in "${values[@]}"; do
        sum=$((sum + value))
    done
    
    local mean=$(echo "scale=2; $sum / $count" | bc)
    
    # Calculate standard deviation
    local variance_sum=0
    for value in "${values[@]}"; do
        local diff=$(echo "scale=4; $value - $mean" | bc)
        local squared=$(echo "scale=4; $diff * $diff" | bc)
        variance_sum=$(echo "scale=4; $variance_sum + $squared" | bc)
    done
    
    local variance=$(echo "scale=4; $variance_sum / $count" | bc)
    local std_dev=$(echo "scale=2; sqrt($variance)" | bc -l)
    
    # Control limits (mean ± 3σ)
    local ucl=$(echo "scale=2; $mean + (3 * $std_dev)" | bc)
    local lcl=$(echo "scale=2; $mean - (3 * $std_dev)" | bc)
    
    # Ensure LCL is not negative
    lcl=$(echo "$lcl < 0" | bc -l)
    if [ "$lcl" -eq 1 ]; then
        lcl="0.00"
    else
        lcl=$(echo "scale=2; $mean - (3 * $std_dev)" | bc)
    fi
    
    cat > "$REPORTS_DIR/throughput-analysis.md" << EOF
# Throughput Analysis (Last 30 Days)

## Statistical Summary
- **Mean**: $mean issues/day
- **Standard Deviation**: $std_dev
- **Upper Control Limit (UCL)**: $ucl
- **Lower Control Limit (LCL)**: $lcl

## Control Chart Data
\`\`\`
Date,Throughput,Mean,UCL,LCL
EOF
    
    # Add daily data points
    for i in {0..29}; do
        local check_date=$(date -d "$start_date + $i days" +%Y-%m-%d)
        local value=${values[$i]}
        echo "$check_date,$value,$mean,$ucl,$lcl" >> "$REPORTS_DIR/throughput-analysis.md"
    done
    
    cat >> "$REPORTS_DIR/throughput-analysis.md" << EOF
\`\`\`

## Process Control Assessment
EOF
    
    # Check for special causes
    local special_causes=""
    for i in {0..29}; do
        local value=${values[$i]}
        local check_date=$(date -d "$start_date + $i days" +%Y-%m-%d)
        
        if (( $(echo "$value > $ucl" | bc -l) )); then
            special_causes="$special_causes\n- $check_date: $value (above UCL)"
        elif (( $(echo "$value < $lcl" | bc -l) )); then
            special_causes="$special_causes\n- $check_date: $value (below LCL)"
        fi
    done
    
    if [ -z "$special_causes" ]; then
        echo "✅ **Process is in statistical control** - no special causes detected" >> "$REPORTS_DIR/throughput-analysis.md"
    else
        echo "⚠️ **Special causes detected:**" >> "$REPORTS_DIR/throughput-analysis.md"
        echo -e "$special_causes" >> "$REPORTS_DIR/throughput-analysis.md"
    fi
    
    echo "✓ Throughput analysis complete"
}

# Function to generate lead time analysis
generate_leadtime_report() {
    echo "⏱️ Generating Lead Time Analysis..."
    
    # Collect lead times from closed issues
    local lead_times=""
    local count=0
    
    for issue_file in "$METRICS_DIR"/issue-*.json; do
        if [ -f "$issue_file" ]; then
            local lead_time=$(calculate_flow_times "$issue_file" 2>/dev/null)
            if [ -n "$lead_time" ] && [ "$lead_time" != "Issue not closed yet" ]; then
                lead_times="$lead_times $lead_time"
                count=$((count + 1))
            fi
        fi
    done
    
    if [ $count -eq 0 ]; then
        echo "No completed issues found for lead time analysis"
        return 1
    fi
    
    # Calculate lead time statistics
    local values=($lead_times)
    local sum=0
    
    for value in "${values[@]}"; do
        sum=$(echo "scale=4; $sum + $value" | bc)
    done
    
    local mean=$(echo "scale=2; $sum / $count" | bc)
    
    # Calculate percentiles (approximate)
    IFS=$'\n' sorted=($(sort -n <<<"${values[*]}"))
    unset IFS
    
    local p50_index=$(( count / 2 ))
    local p85_index=$(( count * 85 / 100 ))
    local p95_index=$(( count * 95 / 100 ))
    
    local p50=${sorted[$p50_index]}
    local p85=${sorted[$p85_index]}
    local p95=${sorted[$p95_index]}
    
    cat > "$REPORTS_DIR/leadtime-analysis.md" << EOF
# Lead Time Analysis

## Summary Statistics
- **Count**: $count completed issues
- **Mean**: $mean hours
- **Median (P50)**: $p50 hours
- **85th Percentile**: $p85 hours  
- **95th Percentile**: $p95 hours

## Lead Time Distribution
\`\`\`
Issue,Lead Time (hours)
EOF
    
    # Add individual data points
    local issue_num=1
    for value in "${values[@]}"; do
        echo "Issue-$issue_num,$value" >> "$REPORTS_DIR/leadtime-analysis.md"
        issue_num=$((issue_num + 1))
    done
    
    cat >> "$REPORTS_DIR/leadtime-analysis.md" << EOF
\`\`\`

## Service Level Targets
- **50% of work completes within**: $p50 hours
- **85% of work completes within**: $p85 hours
- **95% of work completes within**: $p95 hours

## Process Capability
- **Predictability**: $(if (( $(echo "$p95 / $p50 < 3" | bc -l) )); then echo "✅ Good (P95/P50 < 3x)"; else echo "⚠️ High variation (P95/P50 ≥ 3x)"; fi)
EOF
    
    echo "✓ Lead time analysis complete"
}

# Function to generate flow efficiency report
generate_flow_efficiency_report() {
    echo "🎯 Generating Flow Efficiency Analysis..."
    
    # This would require work time vs total time analysis
    # For now, create placeholder structure
    
    cat > "$REPORTS_DIR/flow-efficiency.md" << EOF
# Flow Efficiency Analysis

## Definition
Flow Efficiency = Work Time / Total Lead Time

## Current Metrics
- **Average Flow Efficiency**: TBD (requires work time tracking)
- **Target**: > 25% (industry benchmark)

## Improvement Opportunities
1. **Reduce Wait Time**: Move items from backlog to active work faster
2. **Minimize Context Switching**: Limit work in progress
3. **Remove Blockers**: Identify and eliminate common delays

## Next Steps
- Implement work time tracking (time in 'now' state)
- Track blocked time separately
- Calculate true flow efficiency metrics
EOF
    
    echo "✓ Flow efficiency analysis complete"
}

# Function to generate weekly summary
generate_weekly_summary() {
    echo "📋 Generating Weekly Summary..."
    
    local week_start=$(date -d "monday" +%Y-%m-%d)
    local today=$(date +%Y-%m-%d)
    
    # Calculate weekly throughput
    local weekly_throughput=0
    for i in {0..6}; do
        local check_date=$(date -d "$week_start + $i days" +%Y-%m-%d)
        local daily_file="$METRICS_DIR/daily-${check_date}.json"
        
        if [ -f "$daily_file" ]; then
            local daily_throughput=$(jq -r '.throughput.total' "$daily_file")
            weekly_throughput=$((weekly_throughput + daily_throughput))
        fi
    done
    
    # Get current WIP
    local current_wip=""
    if [ -f "$METRICS_DIR/daily-${today}.json" ]; then
        current_wip=$(jq -r '.work_in_progress' "$METRICS_DIR/daily-${today}.json")
    fi
    
    cat > "$REPORTS_DIR/weekly-summary.md" << EOF
# Weekly Flow Summary - Week of $week_start

## Key Metrics
- **Weekly Throughput**: $weekly_throughput issues completed
- **Current Work in Progress**: $(echo "$current_wip" | jq -r '.total // "N/A"')
  - Active (now): $(echo "$current_wip" | jq -r '.now // "N/A"')
  - Ready (next): $(echo "$current_wip" | jq -r '.next // "N/A"')
  - Backlog (future): $(echo "$current_wip" | jq -r '.future // "N/A"')

## Reports Generated
- [Throughput Analysis](throughput-analysis.md)
- [Lead Time Analysis](leadtime-analysis.md)  
- [Flow Efficiency Analysis](flow-efficiency.md)

## Process Health
- **Throughput Trend**: $(if [ $weekly_throughput -gt 5 ]; then echo "✅ Healthy"; else echo "⚠️ Below target"; fi)
- **WIP Management**: $(if [ "$(echo "$current_wip" | jq -r '.now // 999')" -le 5 ]; then echo "✅ Under control"; else echo "⚠️ High WIP"; fi)

*Generated on $(date)*
EOF
    
    echo "✓ Weekly summary complete"
}

# Main execution
main() {
    if [ ! -d "$METRICS_DIR" ]; then
        echo "❌ Metrics directory not found: $METRICS_DIR"
        echo "Run GitHub Actions to collect flow data first"
        exit 1
    fi
    
    generate_throughput_report
    generate_leadtime_report  
    generate_flow_efficiency_report
    generate_weekly_summary
    
    echo ""
    echo "✅ Flow metrics analysis complete!"
    echo "📁 Reports available in: $REPORTS_DIR"
    echo ""
    echo "Available reports:"
    ls -la "$REPORTS_DIR"/*.md 2>/dev/null || echo "No reports generated"
}

# Run main function
main "$@"