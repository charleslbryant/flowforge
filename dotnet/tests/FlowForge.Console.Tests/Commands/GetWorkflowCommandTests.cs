using FlowForge.Console.Commands;
using FlowForge.Console.Models.Workflows;
using FlowForge.Console.Services.WorkflowManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowForge.Console.Tests.Commands;

public class GetWorkflowCommandTests
{
    private readonly Mock<ILogger<GetWorkflowCommand>> _mockLogger;
    private readonly Mock<IWorkflowService> _mockWorkflowService;
    private readonly GetWorkflowCommand _command;

    public GetWorkflowCommandTests()
    {
        _mockLogger = new Mock<ILogger<GetWorkflowCommand>>();
        _mockWorkflowService = new Mock<IWorkflowService>();
        _command = new GetWorkflowCommand(_mockLogger.Object, _mockWorkflowService.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidId_ReturnsZero()
    {
        // Arrange
        var workflowDetails = new WorkflowDetails
        {
            Id = "123",
            Name = "Test Workflow",
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Tags = new[] { "test" },
            NodeCount = 5,
            Description = "Test workflow",
            Nodes = new[] { new NodeDefinition { Id = "node1", Name = "Node 1", Type = "webhook" } }
        };

        var result = new WorkflowDetailsResult
        {
            Success = true,
            WorkflowDetails = workflowDetails
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "get-workflow", null);
        var settings = new GetWorkflowSettings { Id = "123" };

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(0, exitCode);
        _mockWorkflowService.Verify(x => x.GetWorkflowAsync("123", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidId_ReturnsOne()
    {
        // Arrange
        var result = new WorkflowDetailsResult
        {
            Success = false,
            ErrorMessage = "Workflow not found"
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowAsync("invalid", It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "get-workflow", null);
        var settings = new GetWorkflowSettings { Id = "invalid" };

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(1, exitCode);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceFails_ReturnsOne()
    {
        // Arrange
        var result = new WorkflowDetailsResult
        {
            Success = false,
            ErrorMessage = "Connection failed"
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "get-workflow", null);
        var settings = new GetWorkflowSettings { Id = "123" };

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(1, exitCode);
    }

    [Fact]
    public async Task ExecuteAsync_WithJsonFlag_ReturnsZero()
    {
        // Arrange
        var workflowDetails = new WorkflowDetails
        {
            Id = "123",
            Name = "Test Workflow",
            Active = true,
            CreatedAt = new DateTime(2025, 1, 1),
            UpdatedAt = new DateTime(2025, 1, 2),
            Tags = new[] { "test" },
            NodeCount = 5,
            Description = "Test workflow",
            Nodes = new[] { new NodeDefinition { Id = "node1", Name = "Test Node", Type = "webhook" } }
        };

        var result = new WorkflowDetailsResult
        {
            Success = true,
            WorkflowDetails = workflowDetails
        };

        _mockWorkflowService
            .Setup(x => x.GetWorkflowAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "get-workflow", null);
        var settings = new GetWorkflowSettings { Id = "123", Json = true };

        // Act
        var exitCode = await _command.ExecuteAsync(context, settings);

        // Assert
        Assert.Equal(0, exitCode);
        _mockWorkflowService.Verify(x => x.GetWorkflowAsync("123", It.IsAny<CancellationToken>()), Times.Once);
    }
}