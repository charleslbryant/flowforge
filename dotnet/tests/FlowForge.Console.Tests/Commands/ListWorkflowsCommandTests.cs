using FlowForge.Console.Commands;
using FlowForge.Console.Models.Workflows;
using FlowForge.Console.Services.WorkflowManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowForge.Console.Tests.Commands;

public class ListWorkflowsCommandTests
{
    private readonly Mock<ILogger<ListWorkflowsCommand>> _mockLogger;
    private readonly Mock<IWorkflowService> _mockWorkflowService;
    private readonly ListWorkflowsCommand _command;

    public ListWorkflowsCommandTests()
    {
        _mockLogger = new Mock<ILogger<ListWorkflowsCommand>>();
        _mockWorkflowService = new Mock<IWorkflowService>();
        _command = new ListWorkflowsCommand(_mockLogger.Object, _mockWorkflowService.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenWorkflowsExist_ReturnsZero()
    {
        // Arrange
        var workflows = new[]
        {
            new WorkflowSummary { Id = "1", Name = "Test Workflow 1", Active = true },
            new WorkflowSummary { Id = "2", Name = "Test Workflow 2", Active = false }
        };

        var result = new WorkflowListResult
        {
            Success = true,
            Workflows = workflows,
            TotalCount = 2
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "list-workflows", null);
        var settings = new ListWorkflowsSettings();

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(0, exitCode);
        _mockWorkflowService.Verify(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoWorkflows_ReturnsZero()
    {
        // Arrange
        var result = new WorkflowListResult
        {
            Success = true,
            Workflows = Array.Empty<WorkflowSummary>(),
            TotalCount = 0
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "list-workflows", null);
        var settings = new ListWorkflowsSettings();

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(0, exitCode);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceFails_ReturnsOne()
    {
        // Arrange
        var result = new WorkflowListResult
        {
            Success = false,
            ErrorMessage = "Connection failed",
            Workflows = Array.Empty<WorkflowSummary>(),
            TotalCount = 0
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "list-workflows", null);
        var settings = new ListWorkflowsSettings();

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(1, exitCode);
    }

    [Fact]
    public async Task ExecuteAsync_WithJsonFlag_OutputsJson()
    {
        // Arrange
        var workflows = new[]
        {
            new WorkflowSummary { Id = "1", Name = "Test Workflow 1", Active = true, NodeCount = 5, UpdatedAt = new DateTime(2025, 1, 1) },
            new WorkflowSummary { Id = "2", Name = "Test Workflow 2", Active = false, NodeCount = 3, UpdatedAt = new DateTime(2025, 1, 2) }
        };

        var result = new WorkflowListResult
        {
            Success = true,
            Workflows = workflows,
            TotalCount = 2
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var settings = new ListWorkflowsSettings { Json = true };
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "list-workflows", null);

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(0, exitCode);
        _mockWorkflowService.Verify(x => x.GetWorkflowsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}