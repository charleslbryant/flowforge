using FlowForge.Console.Models.Workflows;
using Xunit;

namespace FlowForge.Console.Tests.Models.Workflows;

public class WorkflowSummaryTests
{
    [Fact]
    public void WorkflowSummary_ShouldInitializeWithDefaultValues()
    {
        // Act
        var summary = new WorkflowSummary();

        // Assert
        Assert.Equal(string.Empty, summary.Id);
        Assert.Equal(string.Empty, summary.Name);
        Assert.False(summary.Active);
        Assert.Equal(default(DateTime), summary.CreatedAt);
        Assert.Equal(default(DateTime), summary.UpdatedAt);
        Assert.Empty(summary.Tags);
        Assert.Equal(0, summary.NodeCount);
        Assert.Null(summary.Description);
    }

    [Fact]
    public void WorkflowSummary_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = "workflow-123";
        var name = "Test Workflow";
        var active = true;
        var createdAt = DateTime.Now.AddDays(-5);
        var updatedAt = DateTime.Now;
        var tags = new[] { "tag1", "tag2" };
        var nodeCount = 5;
        var description = "Test description";

        // Act
        var summary = new WorkflowSummary
        {
            Id = id,
            Name = name,
            Active = active,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            Tags = tags,
            NodeCount = nodeCount,
            Description = description
        };

        // Assert
        Assert.Equal(id, summary.Id);
        Assert.Equal(name, summary.Name);
        Assert.Equal(active, summary.Active);
        Assert.Equal(createdAt, summary.CreatedAt);
        Assert.Equal(updatedAt, summary.UpdatedAt);
        Assert.Equal(tags, summary.Tags);
        Assert.Equal(nodeCount, summary.NodeCount);
        Assert.Equal(description, summary.Description);
    }
}