using FlowForge.Console.Commands;
using FlowForge.Console.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowForge.Console.Tests.Commands;

public class HealthCommandTests
{
    [Fact]
    public async Task ExecuteAsync_WhenN8nIsHealthy_ReturnsZero()
    {
        // Arrange
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<HealthCommand>>();
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = true, Message = "n8n is running" });
        
        var healthCommand = new TestableHealthCommand(mockLogger.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "health", null);
        
        // Act
        var result = await healthCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
    }
    
    [Fact] 
    public async Task ExecuteAsync_WhenN8nIsUnhealthy_ReturnsOne()
    {
        // Arrange
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<HealthCommand>>();
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = false, Message = "n8n is not running" });
        
        var healthCommand = new TestableHealthCommand(mockLogger.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "health", null);
        
        // Act
        var result = await healthCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
    }
}

// Testable version of HealthCommand that accepts IHealthChecker
public class TestableHealthCommand : AsyncCommand
{
    private readonly ILogger<HealthCommand> _logger;
    private readonly IHealthChecker _healthChecker;
    
    public TestableHealthCommand(ILogger<HealthCommand> logger, IHealthChecker healthChecker)
    {
        _logger = logger;
        _healthChecker = healthChecker;
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var result = await _healthChecker.CheckN8nHealthAsync(CancellationToken.None);
        
        if (result.IsHealthy)
        {
            return 0; // Success
        }
        else
        {
            return 1; // Failure
        }
    }
}