using FlowForge.Console.Commands;
using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowForge.Console.Tests.Commands;

public class StopCommandTests
{
    [Fact]
    public async Task ExecuteAsync_WhenN8nNotRunning_ReturnsZero()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockLogger = new Mock<ILogger<StopCommand>>();
        
        var successResult = ProcessOperationResult.CreateSuccess(
            ProcessOperationType.Stop,
            "n8n process is not running");
        
        mockProcessManager.Setup(x => x.StopN8nAsyncEnhanced(It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);
        
        var stopCommand = new StopCommand(mockLogger.Object, mockProcessManager.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "stop", null);
        
        // Act
        var result = await stopCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StopN8nAsyncEnhanced(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nRunningAndStopsSuccessfully_ReturnsZero()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockLogger = new Mock<ILogger<StopCommand>>();
        
        var successResult = ProcessOperationResult.CreateSuccess(
            ProcessOperationType.Stop,
            "n8n process stopped successfully");
        
        mockProcessManager.Setup(x => x.StopN8nAsyncEnhanced(It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);
        
        var stopCommand = new StopCommand(mockLogger.Object, mockProcessManager.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "stop", null);
        
        // Act
        var result = await stopCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StopN8nAsyncEnhanced(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nRunningButFailsToStop_ReturnsOne()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockLogger = new Mock<ILogger<StopCommand>>();
        
        var failureResult = ProcessOperationResult.CreateFailure(
            ProcessOperationType.Stop,
            "Failed to stop n8n process",
            "The process could not be terminated. This may be due to insufficient permissions.",
            "Try running with elevated permissions (sudo on Linux/macOS)",
            "Use 'pkill -f n8n' or 'taskkill /F /IM n8n.exe' manually");
        
        mockProcessManager.Setup(x => x.StopN8nAsyncEnhanced(It.IsAny<CancellationToken>()))
            .ReturnsAsync(failureResult);
        
        var stopCommand = new StopCommand(mockLogger.Object, mockProcessManager.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "stop", null);
        
        // Act
        var result = await stopCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
        mockProcessManager.Verify(x => x.StopN8nAsyncEnhanced(It.IsAny<CancellationToken>()), Times.Once);
    }
}