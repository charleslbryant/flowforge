using FlowForge.Console.Services.ProcessManagement;
using Xunit;

namespace FlowForge.Console.Tests.Services.ProcessManagement;

public class ProcessOperationResultTests
{
    [Fact]
    public void CreateSuccess_ShouldReturnSuccessResult_WithMessage()
    {
        // Arrange
        const string expectedMessage = "Process started successfully";
        const int expectedProcessId = 12345;
        
        // Act
        var result = ProcessOperationResult.CreateSuccess(
            ProcessOperationType.Start, 
            expectedMessage, 
            expectedProcessId);
        
        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Equal(ProcessOperationType.Start, result.OperationType);
        Assert.Equal(expectedProcessId, result.ProcessId);
        Assert.Null(result.ErrorDetails);
        Assert.Empty(result.SuggestedActions);
    }
    
    [Fact]
    public void CreateSuccess_WithoutProcessId_ShouldReturnSuccessResult()
    {
        // Arrange
        const string expectedMessage = "Process stopped successfully";
        
        // Act
        var result = ProcessOperationResult.CreateSuccess(
            ProcessOperationType.Stop, 
            expectedMessage);
        
        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Equal(ProcessOperationType.Stop, result.OperationType);
        Assert.Null(result.ProcessId);
        Assert.Null(result.ErrorDetails);
        Assert.Empty(result.SuggestedActions);
    }
    
    [Fact]
    public void CreateFailure_WithErrorDetails_ShouldReturnFailureResult()
    {
        // Arrange
        const string expectedMessage = "Failed to stop process";
        const string expectedErrorDetails = "Access denied";
        var expectedSuggestedActions = new[] 
        { 
            "Run with elevated permissions",
            "Check if process is system-protected"
        };
        
        // Act
        var result = ProcessOperationResult.CreateFailure(
            ProcessOperationType.Stop,
            expectedMessage,
            expectedErrorDetails,
            expectedSuggestedActions);
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Equal(expectedErrorDetails, result.ErrorDetails);
        Assert.Equal(ProcessOperationType.Stop, result.OperationType);
        Assert.Null(result.ProcessId);
        Assert.Equal(2, result.SuggestedActions.Count);
        Assert.Contains("Run with elevated permissions", result.SuggestedActions);
        Assert.Contains("Check if process is system-protected", result.SuggestedActions);
    }
    
    [Fact]
    public void CreateFailure_WithoutErrorDetails_ShouldReturnFailureResult()
    {
        // Arrange
        const string expectedMessage = "Process not found";
        
        // Act
        var result = ProcessOperationResult.CreateFailure(
            ProcessOperationType.StatusCheck,
            expectedMessage);
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Null(result.ErrorDetails);
        Assert.Equal(ProcessOperationType.StatusCheck, result.OperationType);
        Assert.Empty(result.SuggestedActions);
    }
    
    [Fact]
    public void DefaultConstructor_ShouldInitializeEmptyCollections()
    {
        // Act
        var result = new ProcessOperationResult();
        
        // Assert
        Assert.NotNull(result.SuggestedActions);
        Assert.Empty(result.SuggestedActions);
        Assert.Equal(string.Empty, result.Message);
    }
}