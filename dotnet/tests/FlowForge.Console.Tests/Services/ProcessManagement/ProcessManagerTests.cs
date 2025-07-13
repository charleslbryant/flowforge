using FlowForge.Console.Infrastructure.Process;
using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlowForge.Console.Tests.Services.ProcessManagement;

public class ProcessManagerTests
{
    private readonly Mock<ILogger<ProcessManager>> _mockLogger;
    private readonly Mock<IProcessExecutor> _mockProcessExecutor;
    private readonly ProcessManager _processManager;

    public ProcessManagerTests()
    {
        _mockLogger = new Mock<ILogger<ProcessManager>>();
        _mockProcessExecutor = new Mock<IProcessExecutor>();
        _processManager = new ProcessManager(_mockLogger.Object, _mockProcessExecutor.Object);
    }

    [Fact]
    public async Task StopN8nAsyncEnhanced_WhenProcessNotRunning_ReturnsSuccessWithMessage()
    {
        // Arrange
        _mockProcessExecutor.Setup(x => x.IsProcessRunningAsync("n8n", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _processManager.StopN8nAsyncEnhanced(CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("n8n process is not running", result.Message);
        Assert.Equal(ProcessOperationType.Stop, result.OperationType);
        Assert.Empty(result.SuggestedActions);
        _mockProcessExecutor.Verify(x => x.KillProcessAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task StopN8nAsyncEnhanced_WhenProcessRunningAndStopsSuccessfully_ReturnsSuccess()
    {
        // Arrange
        _mockProcessExecutor.Setup(x => x.IsProcessRunningAsync("n8n", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockProcessExecutor.Setup(x => x.KillProcessAsync("n8n", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _processManager.StopN8nAsyncEnhanced(CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("n8n process stopped successfully", result.Message);
        Assert.Equal(ProcessOperationType.Stop, result.OperationType);
        Assert.Empty(result.SuggestedActions);
    }

    [Fact]
    public async Task StopN8nAsyncEnhanced_WhenKillFails_ReturnsFailureWithSuggestions()
    {
        // Arrange
        _mockProcessExecutor.Setup(x => x.IsProcessRunningAsync("n8n", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockProcessExecutor.Setup(x => x.KillProcessAsync("n8n", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _processManager.StopN8nAsyncEnhanced(CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to stop n8n process", result.Message);
        Assert.Equal(ProcessOperationType.Stop, result.OperationType);
        Assert.Contains("insufficient permissions", result.ErrorDetails);
        Assert.NotEmpty(result.SuggestedActions);
        Assert.Contains(result.SuggestedActions, s => s.Contains("elevated permissions"));
        Assert.Contains(result.SuggestedActions, s => s.Contains("pkill"));
    }

    [Fact]
    public async Task StopN8nAsyncEnhanced_WhenExceptionOccurs_ReturnsFailureWithErrorDetails()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Process executor failed");
        _mockProcessExecutor.Setup(x => x.IsProcessRunningAsync("n8n", It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result = await _processManager.StopN8nAsyncEnhanced(CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Unexpected error while stopping n8n process", result.Message);
        Assert.Equal("Process executor failed", result.ErrorDetails);
        Assert.Equal(ProcessOperationType.Stop, result.OperationType);
        Assert.NotEmpty(result.SuggestedActions);
        Assert.Contains(result.SuggestedActions, s => s.Contains("system logs"));
    }

    [Fact]
    public async Task IsN8nRunningAsync_DelegatesToProcessExecutor()
    {
        // Arrange
        _mockProcessExecutor.Setup(x => x.IsProcessRunningAsync("n8n", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _processManager.IsN8nRunningAsync(CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockProcessExecutor.Verify(x => x.IsProcessRunningAsync("n8n", It.IsAny<CancellationToken>()), Times.Once);
    }

    // Skip testing StartN8nAsync success case because System.Diagnostics.Process.Id is not mockable
    // This is tested through integration tests instead

    [Fact]
    public async Task StartN8nAsync_WhenStartFails_ReturnsFailureResult()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Failed to start process");
        _mockProcessExecutor.Setup(x => x.StartBackgroundProcessAsync("n8n", "", It.IsAny<string>()))
            .ThrowsAsync(expectedException);

        // Act
        var result = await _processManager.StartN8nAsync(CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to start process", result.ErrorMessage);
        Assert.Null(result.ProcessId);
    }
}