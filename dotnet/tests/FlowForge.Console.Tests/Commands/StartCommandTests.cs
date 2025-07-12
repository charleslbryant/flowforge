using FlowForge.Console.Commands;
using FlowForge.Console.Services.HealthChecking;
using FlowForge.Console.Services.ProcessManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowForge.Console.Tests.Commands;

public class StartCommandTests
{
    [Fact]
    public async Task ExecuteAsync_WhenN8nAlreadyRunning_ReturnsZero()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<StartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = true, Message = "n8n is running" });
        
        var startCommand = new StartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "start", null);
        
        // Act
        var result = await startCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StartN8nAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nNotRunningAndStartsSuccessfully_ReturnsZero()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<StartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        mockProcessManager.Setup(x => x.StartN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProcessStartResult { Success = true });
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = true, Message = "n8n started successfully" });
        
        var startCommand = new StartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "start", null);
        
        // Act
        var result = await startCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
        mockProcessManager.Verify(x => x.StartN8nAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nFailsToStart_ReturnsOne()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<StartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        mockProcessManager.Setup(x => x.StartN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProcessStartResult { Success = true });
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = false, Message = "n8n failed to start" });
        
        var startCommand = new StartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "start", null);
        
        // Act
        var result = await startCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenProcessStartFails_ReturnsOne()
    {
        // Arrange
        var mockProcessManager = new Mock<IProcessManager>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<StartCommand>>();
        
        mockProcessManager.Setup(x => x.IsN8nRunningAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        mockProcessManager.Setup(x => x.StartN8nAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProcessStartResult { Success = false, ErrorMessage = "Failed to start process" });
        
        var startCommand = new StartCommand(mockLogger.Object, mockProcessManager.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "start", null);
        
        // Act
        var result = await startCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
        mockHealthChecker.Verify(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}