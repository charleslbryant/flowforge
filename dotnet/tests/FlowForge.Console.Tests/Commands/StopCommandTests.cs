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
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var stopCommand = new StopCommand(mockLogger.Object, mockProcessManager.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "stop", null);
        
        // Act
        var result = await stopCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StopN8nAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nRunningAndStopsSuccessfully_ReturnsZero()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockLogger = new Mock<ILogger<StopCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockProcessManager.Setup(x => x.StopN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var stopCommand = new StopCommand(mockLogger.Object, mockProcessManager.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "stop", null);
        
        // Act
        var result = await stopCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StopN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nRunningButFailsToStop_ReturnsOne()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockLogger = new Mock<ILogger<StopCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockProcessManager.Setup(x => x.StopN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var stopCommand = new StopCommand(mockLogger.Object, mockProcessManager.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "stop", null);
        
        // Act
        var result = await stopCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
        mockProcessManager.Verify(x => x.StopN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}