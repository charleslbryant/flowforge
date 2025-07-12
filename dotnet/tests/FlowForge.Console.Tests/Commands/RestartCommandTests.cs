using FlowForge.Console.Commands;
using FlowForge.Console.Services.HealthChecking;
using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowForge.Console.Tests.Commands;

public class RestartCommandTests
{
    [Fact]
    public async Task ExecuteAsync_WhenN8nNotRunning_StartsSuccessfully_ReturnsZero()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<RestartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        mockProcessManager.Setup(x => x.StartN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProcessStartResult { Success = true });
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = true });
        
        var restartCommand = new RestartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "restart", null);
        
        // Act
        var result = await restartCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StopN8nAsync(It.IsAny<CancellationToken>()), Times.Never);
        mockProcessManager.Verify(x => x.StartN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nRunning_StopsAndStartsSuccessfully_ReturnsZero()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<RestartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockProcessManager.Setup(x => x.StopN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockProcessManager.Setup(x => x.StartN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProcessStartResult { Success = true });
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = true });
        
        var restartCommand = new RestartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "restart", null);
        
        // Act
        var result = await restartCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StopN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockProcessManager.Verify(x => x.StartN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenStopFails_ReturnsOne()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<RestartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockProcessManager.Setup(x => x.StopN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var restartCommand = new RestartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "restart", null);
        
        // Act
        var result = await restartCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
        mockProcessManager.Verify(x => x.StopN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockProcessManager.Verify(x => x.StartN8nAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenStartFails_ReturnsOne()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<RestartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockProcessManager.Setup(x => x.StopN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockProcessManager.Setup(x => x.StartN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProcessStartResult { Success = false, ErrorMessage = "Failed to start" });
        
        var restartCommand = new RestartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "restart", null);
        
        // Act
        var result = await restartCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
        mockProcessManager.Verify(x => x.StopN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockProcessManager.Verify(x => x.StartN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}