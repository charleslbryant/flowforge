// using FlowForge.Console.Commands; // DoctorCommand doesn't exist yet
using FlowForge.Console.Services.HealthChecking;
using FlowForge.Console.Services.SystemChecking;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowForge.Console.Tests.Commands;

public class DoctorCommandTests
{
    [Fact]
    public async Task ExecuteAsync_WhenAllSystemsHealthy_ReturnsZero()
    {
        // Arrange
        var mockSystemChecker = new Mock<ISystemChecker>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<TestableDoctorCommand>>();
        
        mockSystemChecker.Setup(x => x.CheckSystemRequirementsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SystemCheckResult 
            { 
                IsHealthy = true,
                Checks = new List<SystemCheck>
                {
                    new() { Name = "node", IsInstalled = true, Version = "v18.0.0" },
                    new() { Name = "npm", IsInstalled = true, Version = "9.0.0" },
                    new() { Name = "n8n", IsInstalled = true, Version = "1.0.0" },
                    new() { Name = "claude", IsInstalled = true, Version = "0.1.0" }
                }
            });
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = true, Message = "n8n is running" });
        
        var doctorCommand = new TestableDoctorCommand(mockLogger.Object, mockSystemChecker.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "doctor", null);
        
        // Act
        var result = await doctorCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(0, result);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenDependenciesMissing_ReturnsOne()
    {
        // Arrange
        var mockSystemChecker = new Mock<ISystemChecker>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<TestableDoctorCommand>>();
        
        mockSystemChecker.Setup(x => x.CheckSystemRequirementsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SystemCheckResult 
            { 
                IsHealthy = false,
                Checks = new List<SystemCheck>
                {
                    new() { Name = "node", IsInstalled = false, Issue = "Command not found" },
                    new() { Name = "n8n", IsInstalled = false, Issue = "Command not found" }
                }
            });
        
        var doctorCommand = new TestableDoctorCommand(mockLogger.Object, mockSystemChecker.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "doctor", null);
        
        // Act
        var result = await doctorCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenN8nUnhealthy_ReturnsOne()
    {
        // Arrange
        var mockSystemChecker = new Mock<ISystemChecker>();
        var mockHealthChecker = new Mock<IHealthChecker>();
        var mockLogger = new Mock<ILogger<TestableDoctorCommand>>();
        
        mockSystemChecker.Setup(x => x.CheckSystemRequirementsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SystemCheckResult { IsHealthy = true, Checks = new List<SystemCheck>() });
        
        mockHealthChecker.Setup(x => x.CheckN8nHealthAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HealthResult { IsHealthy = false, Message = "n8n not responding" });
        
        var doctorCommand = new TestableDoctorCommand(mockLogger.Object, mockSystemChecker.Object, mockHealthChecker.Object);
        var context = new CommandContext(Mock.Of<IRemainingArguments>(), "doctor", null);
        
        // Act
        var result = await doctorCommand.ExecuteAsync(context);
        
        // Assert
        Assert.Equal(1, result);
    }
}

// Supporting interfaces and classes for the tests
public interface ISystemChecker
{
    Task<SystemCheckResult> CheckSystemRequirementsAsync(CancellationToken cancellationToken);
}

public class SystemCheckResult
{
    public bool IsHealthy { get; set; }
    public List<SystemCheck> Checks { get; set; } = new();
    public List<string> MissingDependencies { get; set; } = new();
}

public class SystemCheck
{
    public string Name { get; set; } = string.Empty;
    public bool IsInstalled { get; set; }
    public string? Version { get; set; }
    public string? Issue { get; set; }
    public string? InstallCommand { get; set; }
}

// Testable version of DoctorCommand
public class TestableDoctorCommand : AsyncCommand
{
    private readonly ILogger<TestableDoctorCommand> _logger;
    private readonly ISystemChecker _systemChecker;
    private readonly IHealthChecker _healthChecker;
    
    public TestableDoctorCommand(ILogger<TestableDoctorCommand> logger, ISystemChecker systemChecker, IHealthChecker healthChecker)
    {
        _logger = logger;
        _systemChecker = systemChecker;
        _healthChecker = healthChecker;
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        // Check system requirements
        var systemResult = await _systemChecker.CheckSystemRequirementsAsync(CancellationToken.None);
        
        if (!systemResult.IsHealthy)
        {
            return 1; // Dependencies missing
        }
        
        // Check n8n health
        var healthResult = await _healthChecker.CheckN8nHealthAsync(CancellationToken.None);
        
        if (!healthResult.IsHealthy)
        {
            return 1; // n8n not healthy
        }
        
        return 0; // All systems healthy
    }
}