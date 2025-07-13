using FlowForge.Console.Infrastructure.Http;
using FlowForge.Console.Models.Workflows;
using FlowForge.Console.Services.WorkflowManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlowForge.Console.Tests.Services.WorkflowManagement;

public class WorkflowServiceTests
{
    private readonly Mock<ILogger<WorkflowService>> _mockLogger;
    private readonly Mock<IN8nHttpClient> _mockHttpClient;
    private readonly WorkflowService _workflowService;

    public WorkflowServiceTests()
    {
        _mockLogger = new Mock<ILogger<WorkflowService>>();
        _mockHttpClient = new Mock<IN8nHttpClient>();
        _workflowService = new WorkflowService(_mockLogger.Object, _mockHttpClient.Object);
    }

    [Fact]
    public async Task GetWorkflowsAsync_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var workflows = new[]
        {
            new WorkflowSummary { Id = "1", Name = "Workflow 1", Active = true },
            new WorkflowSummary { Id = "2", Name = "Workflow 2", Active = false }
        };

        _mockHttpClient
            .Setup(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(workflows);

        // Act
        var result = await _workflowService.GetWorkflowsAsync(CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(workflows, result.Workflows);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task GetWorkflowsAsync_WhenEmpty_ReturnsEmptySuccessResult()
    {
        // Arrange
        _mockHttpClient
            .Setup(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<WorkflowSummary>());

        // Act
        var result = await _workflowService.GetWorkflowsAsync(CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Workflows);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task GetWorkflowsAsync_WhenExceptionThrown_ReturnsFailureResult()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        _mockHttpClient
            .Setup(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _workflowService.GetWorkflowsAsync(CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Workflows);
        Assert.Contains("Failed to retrieve workflows: Test exception", result.ErrorMessage);
    }
}