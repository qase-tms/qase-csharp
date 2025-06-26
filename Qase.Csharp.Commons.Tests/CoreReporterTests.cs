using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class CoreReporterTests
    {
        private readonly Mock<ILogger<CoreReporter>> _loggerMock;
        private readonly Mock<IInternalReporter> _primaryReporterMock;
        private readonly Mock<IInternalReporter> _fallbackReporterMock;
        private readonly CoreReporter _coreReporter;

        public CoreReporterTests()
        {
            _loggerMock = new Mock<ILogger<CoreReporter>>();
            _primaryReporterMock = new Mock<IInternalReporter>();
            _fallbackReporterMock = new Mock<IInternalReporter>();
            _coreReporter = new CoreReporter(_loggerMock.Object, _primaryReporterMock.Object, _fallbackReporterMock.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithLogger()
        {
            // Arrange & Act
            var reporter = new CoreReporter(_loggerMock.Object);

            // Assert
            reporter.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithLoggerAndPrimaryReporter()
        {
            // Arrange & Act
            var reporter = new CoreReporter(_loggerMock.Object, _primaryReporterMock.Object);

            // Assert
            reporter.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithLoggerAndPrimaryAndFallbackReporter()
        {
            // Arrange & Act
            var reporter = new CoreReporter(_loggerMock.Object, _primaryReporterMock.Object, _fallbackReporterMock.Object);

            // Assert
            reporter.Should().NotBeNull();
        }

        [Fact]
        public async Task StartTestRun_ShouldCallPrimaryReporter_WhenPrimaryReporterExists()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.startTestRun()).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.startTestRun(), Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldNotThrow_WhenPrimaryReporterIsNull()
        {
            // Arrange
            var reporter = new CoreReporter(_loggerMock.Object, null, _fallbackReporterMock.Object);

            // Act & Assert
            await reporter.Invoking(x => x.startTestRun()).Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartTestRun_ShouldUseFallback_WhenPrimaryReporterThrowsQaseException()
        {
            // Arrange
            var testResults = new List<TestResult> { new TestResult { Id = "test1" } };
            
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _primaryReporterMock.Setup(r => r.getResults())
                .ReturnsAsync(testResults);
            _fallbackReporterMock.Setup(r => r.startTestRun()).Returns(Task.CompletedTask);
            _fallbackReporterMock.Setup(r => r.setResults(testResults)).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _primaryReporterMock.Verify(r => r.getResults(), Times.Once);
            _fallbackReporterMock.Verify(r => r.startTestRun(), Times.Exactly(2));
            _fallbackReporterMock.Verify(r => r.setResults(testResults), Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldDisableReporter_WhenBothPrimaryAndFallbackFail()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _fallbackReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Fallback reporter failed"));

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _fallbackReporterMock.Verify(r => r.startTestRun(), Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_ShouldCallPrimaryReporter_WhenPrimaryReporterExists()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.completeTestRun()).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.completeTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.completeTestRun(), Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_ShouldNotThrow_WhenPrimaryReporterIsNull()
        {
            // Arrange
            var reporter = new CoreReporter(_loggerMock.Object, null, _fallbackReporterMock.Object);

            // Act & Assert
            await reporter.Invoking(x => x.completeTestRun()).Should().NotThrowAsync();
        }

        [Fact]
        public async Task CompleteTestRun_ShouldUseFallback_WhenPrimaryReporterThrowsQaseException()
        {
            // Arrange
            var testResults = new List<TestResult> { new TestResult { Id = "test1" } };
            
            _primaryReporterMock.Setup(r => r.completeTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _primaryReporterMock.Setup(r => r.getResults())
                .ReturnsAsync(testResults);
            _fallbackReporterMock.Setup(r => r.startTestRun()).Returns(Task.CompletedTask);
            _fallbackReporterMock.Setup(r => r.setResults(testResults)).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.completeTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.completeTestRun(), Times.Once);
            _primaryReporterMock.Verify(r => r.getResults(), Times.Once);
            _fallbackReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _fallbackReporterMock.Verify(r => r.setResults(testResults), Times.Once);
        }

        [Fact]
        public async Task AddResult_ShouldCallPrimaryReporter_WhenPrimaryReporterExists()
        {
            // Arrange
            var testResult = new TestResult { Id = "test1", Title = "Test 1" };
            _primaryReporterMock.Setup(r => r.addResult(testResult)).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.addResult(testResult);

            // Assert
            _primaryReporterMock.Verify(r => r.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_ShouldNotThrow_WhenPrimaryReporterIsNull()
        {
            // Arrange
            var reporter = new CoreReporter(_loggerMock.Object, null, _fallbackReporterMock.Object);
            var testResult = new TestResult { Id = "test1", Title = "Test 1" };

            // Act & Assert
            await reporter.Invoking(x => x.addResult(testResult)).Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddResult_ShouldUseFallback_WhenPrimaryReporterThrowsQaseException()
        {
            // Arrange
            var testResult = new TestResult { Id = "test1", Title = "Test 1" };
            var testResults = new List<TestResult> { testResult };
            
            _primaryReporterMock.Setup(r => r.addResult(testResult))
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _primaryReporterMock.Setup(r => r.getResults())
                .ReturnsAsync(testResults);
            _fallbackReporterMock.Setup(r => r.startTestRun()).Returns(Task.CompletedTask);
            _fallbackReporterMock.Setup(r => r.setResults(testResults)).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.addResult(testResult);

            // Assert
            _primaryReporterMock.Verify(r => r.addResult(testResult), Times.Once);
            _primaryReporterMock.Verify(r => r.getResults(), Times.Once);
            _fallbackReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _fallbackReporterMock.Verify(r => r.setResults(testResults), Times.Once);
        }

        [Fact]
        public async Task UploadResults_ShouldCallPrimaryReporter_WhenPrimaryReporterExists()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.uploadResults()).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.uploadResults();

            // Assert
            _primaryReporterMock.Verify(r => r.uploadResults(), Times.Once);
        }

        [Fact]
        public async Task UploadResults_ShouldNotThrow_WhenPrimaryReporterIsNull()
        {
            // Arrange
            var reporter = new CoreReporter(_loggerMock.Object, null, _fallbackReporterMock.Object);

            // Act & Assert
            await reporter.Invoking(x => x.uploadResults()).Should().NotThrowAsync();
        }

        [Fact]
        public async Task UploadResults_ShouldUseFallback_WhenPrimaryReporterThrowsQaseException()
        {
            // Arrange
            var testResults = new List<TestResult> { new TestResult { Id = "test1" } };
            
            _primaryReporterMock.Setup(r => r.uploadResults())
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _primaryReporterMock.Setup(r => r.getResults())
                .ReturnsAsync(testResults);
            _fallbackReporterMock.Setup(r => r.startTestRun()).Returns(Task.CompletedTask);
            _fallbackReporterMock.Setup(r => r.setResults(testResults)).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.uploadResults();

            // Assert
            _primaryReporterMock.Verify(r => r.uploadResults(), Times.Once);
            _primaryReporterMock.Verify(r => r.getResults(), Times.Once);
            _fallbackReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _fallbackReporterMock.Verify(r => r.setResults(testResults), Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldDisableReporter_WhenFallbackIsNullAndPrimaryFails()
        {
            // Arrange
            var reporter = new CoreReporter(_loggerMock.Object, _primaryReporterMock.Object, null);
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));

            // Act
            await reporter.startTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.startTestRun(), Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldDisableReporter_WhenFallbackStartTestRunFails()
        {
            // Arrange
            var testResults = new List<TestResult> { new TestResult { Id = "test1" } };
            
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _primaryReporterMock.Setup(r => r.getResults())
                .ReturnsAsync(testResults);
            _fallbackReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Fallback startTestRun failed"));

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _primaryReporterMock.Verify(r => r.getResults(), Times.Never);
            _fallbackReporterMock.Verify(r => r.startTestRun(), Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldDisableReporter_WhenFallbackSetResultsFails()
        {
            // Arrange
            var testResults = new List<TestResult> { new TestResult { Id = "test1" } };
            
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _primaryReporterMock.Setup(r => r.getResults())
                .ReturnsAsync(testResults);
            _fallbackReporterMock.Setup(r => r.startTestRun()).Returns(Task.CompletedTask);
            _fallbackReporterMock.Setup(r => r.setResults(testResults))
                .ThrowsAsync(new QaseException("Fallback setResults failed"));

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _primaryReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _primaryReporterMock.Verify(r => r.getResults(), Times.Once);
            _fallbackReporterMock.Verify(r => r.startTestRun(), Times.Once);
            _fallbackReporterMock.Verify(r => r.setResults(testResults), Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldNotRetry_WhenNonQaseExceptionIsThrown()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new InvalidOperationException("Non-Qase exception"));

            // Act & Assert
            await _coreReporter.Invoking(x => x.startTestRun()).Should().ThrowAsync<InvalidOperationException>();

            // Assert
            _primaryReporterMock.Verify(r => r.startTestRun(), Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_ShouldNotRetry_WhenNonQaseExceptionIsThrown()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.completeTestRun())
                .ThrowsAsync(new InvalidOperationException("Non-Qase exception"));

            // Act & Assert
            await _coreReporter.Invoking(x => x.completeTestRun()).Should().ThrowAsync<InvalidOperationException>();

            // Assert
            _primaryReporterMock.Verify(r => r.completeTestRun(), Times.Once);
        }

        [Fact]
        public async Task AddResult_ShouldNotRetry_WhenNonQaseExceptionIsThrown()
        {
            // Arrange
            var testResult = new TestResult { Id = "test1", Title = "Test 1" };
            _primaryReporterMock.Setup(r => r.addResult(testResult))
                .ThrowsAsync(new InvalidOperationException("Non-Qase exception"));

            // Act & Assert
            await _coreReporter.Invoking(x => x.addResult(testResult)).Should().ThrowAsync<InvalidOperationException>();

            // Assert
            _primaryReporterMock.Verify(r => r.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task UploadResults_ShouldNotRetry_WhenNonQaseExceptionIsThrown()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.uploadResults())
                .ThrowsAsync(new InvalidOperationException("Non-Qase exception"));

            // Act & Assert
            await _coreReporter.Invoking(x => x.uploadResults()).Should().ThrowAsync<InvalidOperationException>();

            // Assert
            _primaryReporterMock.Verify(r => r.uploadResults(), Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldLogInformation_WhenCalled()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.startTestRun()).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Starting test run")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_ShouldLogInformation_WhenCalled()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.completeTestRun()).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.completeTestRun();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Completing test run")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AddResult_ShouldLogDebug_WhenCalled()
        {
            // Arrange
            var testResult = new TestResult { Id = "test1", Title = "Test 1" };
            _primaryReporterMock.Setup(r => r.addResult(testResult)).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.addResult(testResult);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Adding result")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UploadResults_ShouldLogInformation_WhenCalled()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.uploadResults()).Returns(Task.CompletedTask);

            // Act
            await _coreReporter.uploadResults();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Uploading results")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldLogError_WhenPrimaryReporterFails()
        {
            // Arrange
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to start test run")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task StartTestRun_ShouldLogError_WhenFallbackFails()
        {
            // Arrange
            var testResults = new List<TestResult> { new TestResult { Id = "test1" } };
            
            _primaryReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Primary reporter failed"));
            _primaryReporterMock.Setup(r => r.getResults())
                .ReturnsAsync(testResults);
            _fallbackReporterMock.Setup(r => r.startTestRun())
                .ThrowsAsync(new QaseException("Fallback reporter failed"));

            // Act
            await _coreReporter.startTestRun();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to start test run with fallback reporter")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
} 
