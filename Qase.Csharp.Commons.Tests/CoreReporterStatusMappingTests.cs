using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    /// <summary>
    /// Tests for CoreReporter status mapping functionality
    /// </summary>
    public class CoreReporterStatusMappingTests
    {
        private readonly ILogger<CoreReporter> _logger = NullLogger<CoreReporter>.Instance;

        [Fact]
        public async Task AddResult_WithStatusMapping_AppliesMapping()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = new Dictionary<string, string>
                {
                    ["invalid"] = "failed",
                    ["skipped"] = "passed"
                }
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.Equal(TestResultStatus.Failed, testResult.Execution.Status);
            mockReporter.Verify(x => x.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_WithStatusMapping_MultipleMappings()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = new Dictionary<string, string>
                {
                    ["invalid"] = "failed",
                    ["skipped"] = "passed",
                    ["blocked"] = "skipped"
                }
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Blocked
                }
            };

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.Equal(TestResultStatus.Skipped, testResult.Execution.Status);
            mockReporter.Verify(x => x.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_WithStatusMapping_NoApplicableMapping()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = new Dictionary<string, string>
                {
                    ["invalid"] = "failed"
                }
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Passed
                }
            };

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.Equal(TestResultStatus.Passed, testResult.Execution.Status);
            mockReporter.Verify(x => x.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_WithoutStatusMapping_NoChanges()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = new Dictionary<string, string>()
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.Equal(TestResultStatus.Invalid, testResult.Execution.Status);
            mockReporter.Verify(x => x.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_WithNullStatusMapping_NoChanges()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = null!
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.Equal(TestResultStatus.Invalid, testResult.Execution.Status);
            mockReporter.Verify(x => x.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_WithNullExecution_NoChanges()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = new Dictionary<string, string>
                {
                    ["invalid"] = "failed"
                }
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = null
            };

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.Null(testResult.Execution);
            mockReporter.Verify(x => x.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_WithFallbackReporter_AppliesMappingToBoth()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var mockFallback = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = new Dictionary<string, string>
                {
                    ["invalid"] = "failed"
                }
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object, mockFallback.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };

            // Simulate reporter failure
            mockReporter.Setup(x => x.addResult(testResult))
                .Throws(new QaseException("Test failure"));

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.Equal(TestResultStatus.Failed, testResult.Execution.Status);
            mockReporter.Verify(x => x.addResult(testResult), Times.Once);
            mockFallback.Verify(x => x.addResult(testResult), Times.Once);
        }

        [Fact]
        public async Task AddResult_StatusMappingAppliedBeforeReporterCall()
        {
            // Arrange
            var mockReporter = new Mock<IInternalReporter>();
            var config = new QaseConfig
            {
                StatusMapping = new Dictionary<string, string>
                {
                    ["invalid"] = "failed"
                }
            };

            var coreReporter = new CoreReporter(_logger, config, mockReporter.Object);
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };

            TestResult? capturedResult = null;
            mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>()))
                .Callback<TestResult>(result => capturedResult = result);

            // Act
            await coreReporter.addResult(testResult);

            // Assert
            Assert.NotNull(capturedResult);
            Assert.Equal(TestResultStatus.Failed, capturedResult.Execution.Status);
            mockReporter.Verify(x => x.addResult(It.IsAny<TestResult>()), Times.Once);
        }
    }
}
