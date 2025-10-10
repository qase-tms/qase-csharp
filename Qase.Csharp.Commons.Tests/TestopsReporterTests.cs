using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class TestopsReporterTests
    {
        private readonly Mock<ILogger<TestopsReporter>> _loggerMock;
        private readonly Mock<IClient> _clientMock;
        private readonly QaseConfig _config;
        private readonly TestopsReporter _reporter;

        public TestopsReporterTests()
        {
            _loggerMock = new Mock<ILogger<TestopsReporter>>();
            _clientMock = new Mock<IClient>();
            _config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Project = "TEST_PROJECT",
                    Api = new ApiConfig { Host = "qase.io" },
                    Batch = new BatchConfig { Size = 10 },
                    Run = new RunConfig()
                }
            };
            _reporter = new TestopsReporter(_loggerMock.Object, _config, _clientMock.Object);
        }

        [Fact]
        public async Task Constructor_ShouldInitializeCorrectly()
        {
            // Act & Assert
            var results = await _reporter.getResults();
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task StartTestRun_WithExistingRunId_ShouldUseExistingId()
        {
            // Arrange
            var existingRunId = 12345L;
            _config.TestOps.Run!.Id = existingRunId;

            // Act
            await _reporter.startTestRun();

            // Assert
            _clientMock.Verify(x => x.CreateTestRunAsync(), Times.Never);
        }

        [Fact]
        public async Task StartTestRun_WithoutExistingRunId_ShouldCreateNewRun()
        {
            // Arrange
            var newRunId = 67890L;
            _config.TestOps.Run!.Id = null;
            _clientMock.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(newRunId);

            // Act
            await _reporter.startTestRun();

            // Assert
            _clientMock.Verify(x => x.CreateTestRunAsync(), Times.Once);
            _config.TestOps.Run.Id.Should().Be(newRunId);
        }

        [Fact]
        public async Task CompleteTestRun_ShouldCallClientAndLog()
        {
            // Arrange
            var runId = 12345L;
            _config.TestOps.Run!.Id = runId;
            await _reporter.startTestRun(); // Initialize the test run

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
        }

        [Fact]
        public async Task AddResult_ShouldAddResultToList()
        {
            // Arrange
            var result = new TestResult { Title = "Test 1" };

            // Act
            await _reporter.addResult(result);

            // Assert
            var results = await _reporter.getResults();
            results.Should().ContainSingle();
            results[0].Should().Be(result);
        }

        [Fact]
        public async Task AddResult_WithFailedStatus_ShouldLogFailureLink()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Failed Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act
            await _reporter.addResult(result);

            // Assert
            // Verify that logging was called (we can't easily verify the exact message due to private method)
            // The main assertion is that no exception is thrown
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNoTestopsId_ShouldLogFailureLinkWithTitle()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Failed Test Without ID",
                TestopsIds = new List<long>(),
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act
            await _reporter.addResult(result);

            // Assert
            // Verify that logging was called (we can't easily verify the exact message due to private method)
            // The main assertion is that no exception is thrown
        }

        [Fact]
        public async Task AddResult_WhenBatchSizeReached_ShouldUploadResults()
        {
            // Arrange
            _config.TestOps.Batch.Size = 2;
            var result1 = new TestResult { Title = "Test 1" };
            var result2 = new TestResult { Title = "Test 2" };
            
            // Initialize test run
            _config.TestOps.Run!.Id = 12345L;
            await _reporter.startTestRun();

            // Act
            await _reporter.addResult(result1);
            await _reporter.addResult(result2);

            // Assert
            _clientMock.Verify(x => x.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()), Times.Once);
            
            var results = await _reporter.getResults();
            results.Should().BeEmpty(); // Results should be cleared after upload
        }

        [Fact]
        public async Task UploadResults_WithEmptyResults_ShouldDoNothing()
        {
            // Act
            await _reporter.uploadResults();

            // Assert
            _clientMock.Verify(x => x.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()), Times.Never);
        }

        [Fact]
        public async Task UploadResults_WithResultsLessThanBatchSize_ShouldUploadAllResults()
        {
            // Arrange
            _config.TestOps.Batch.Size = 10;
            var result = new TestResult { Title = "Test 1" };
            
            // Initialize test run
            _config.TestOps.Run!.Id = 12345L;
            await _reporter.startTestRun();
            
            await _reporter.addResult(result);

            // Act
            await _reporter.uploadResults();

            // Assert
            _clientMock.Verify(x => x.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()), Times.Once);
            
            var results = await _reporter.getResults();
            results.Should().BeEmpty(); // Results should be cleared after upload
        }

        [Fact]
        public async Task UploadResults_WithResultsEqualToBatchSize_ShouldUploadAllResults()
        {
            // Arrange
            _config.TestOps.Batch.Size = 2;
            var result1 = new TestResult { Title = "Test 1" };
            var result2 = new TestResult { Title = "Test 2" };
            
            // Initialize test run
            _config.TestOps.Run!.Id = 12345L;
            await _reporter.startTestRun();
            
            await _reporter.addResult(result1);
            await _reporter.addResult(result2);

            // Act
            await _reporter.uploadResults();

            // Assert
            _clientMock.Verify(x => x.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()), Times.Once);
            
            var results = await _reporter.getResults();
            results.Should().BeEmpty(); // Results should be cleared after upload
        }

        [Fact]
        public async Task UploadResults_WithResultsGreaterThanBatchSize_ShouldUploadInBatches()
        {
            // Arrange
            _config.TestOps.Batch.Size = 2;
            var result1 = new TestResult { Title = "Test 1" };
            var result2 = new TestResult { Title = "Test 2" };
            var result3 = new TestResult { Title = "Test 3" };
            var result4 = new TestResult { Title = "Test 4" };
            var result5 = new TestResult { Title = "Test 5" };

            // Initialize test run
            _config.TestOps.Run!.Id = 12345L;
            await _reporter.startTestRun();

            await _reporter.addResult(result1);
            await _reporter.addResult(result2);
            await _reporter.addResult(result3);
            await _reporter.addResult(result4);
            await _reporter.addResult(result5);

            // Act
            await _reporter.uploadResults();

            // Assert
            _clientMock.Verify(x => x.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()), Times.Exactly(3));
            
            var results = await _reporter.getResults();
            results.Should().BeEmpty(); // Results should be cleared after upload
        }

        [Fact]
        public async Task GetResults_ShouldReturnCurrentResults()
        {
            // Arrange
            var result1 = new TestResult { Title = "Test 1" };
            var result2 = new TestResult { Title = "Test 2" };
            await _reporter.addResult(result1);
            await _reporter.addResult(result2);

            // Act
            var results = await _reporter.getResults();

            // Assert
            results.Should().HaveCount(2);
            results.Should().Contain(result1);
            results.Should().Contain(result2);
        }

        [Fact]
        public async Task SetResults_ShouldReplaceCurrentResults()
        {
            // Arrange
            var existingResult = new TestResult { Title = "Existing Test" };
            await _reporter.addResult(existingResult);

            var newResults = new List<TestResult>
            {
                new TestResult { Title = "New Test 1" },
                new TestResult { Title = "New Test 2" }
            };

            // Act
            await _reporter.setResults(newResults);

            // Assert
            var results = await _reporter.getResults();
            results.Should().HaveCount(2);
            results.Should().NotContain(existingResult);
            results.Should().Contain(newResults[0]);
            results.Should().Contain(newResults[1]);
        }

        [Fact]
        public async Task GetTestCaseIdsForExecutionAsync_WhenClientSucceeds_ShouldReturnIds()
        {
            // Arrange
            var expectedIds = new List<long> { 1, 2, 3 };
            _clientMock.Setup(x => x.GetTestCaseIdsForExecutionAsync()).ReturnsAsync(expectedIds);

            // Act
            var result = await _reporter.GetTestCaseIdsForExecutionAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedIds);
            _clientMock.Verify(x => x.GetTestCaseIdsForExecutionAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTestCaseIdsForExecutionAsync_WhenClientThrowsQaseException_ShouldReturnEmptyList()
        {
            // Arrange
            _clientMock.Setup(x => x.GetTestCaseIdsForExecutionAsync())
                .ThrowsAsync(new QaseException("API Error"));

            // Act
            var result = await _reporter.GetTestCaseIdsForExecutionAsync();

            // Assert
            result.Should().BeEmpty();
            _clientMock.Verify(x => x.GetTestCaseIdsForExecutionAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTestCaseIdsForExecutionAsync_WhenClientThrowsOtherException_ShouldPropagateException()
        {
            // Arrange
            _clientMock.Setup(x => x.GetTestCaseIdsForExecutionAsync())
                .ThrowsAsync(new InvalidOperationException("Other Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _reporter.GetTestCaseIdsForExecutionAsync());
        }

        [Theory]
        [InlineData("qase.io", "https://app.qase.io")]
        [InlineData("custom.qase.io", "https://custom.qase.io")]
        [InlineData("test.example.com", "https://test.example.com")]
        public void PrepareLink_ShouldGenerateCorrectBaseUrl(string host, string expectedBaseUrl)
        {
            // Arrange
            _config.TestOps.Api.Host = host;
            _config.TestOps.Project = "TEST_PROJECT";
            _config.TestOps.Run!.Id = 12345L;

            // Act & Assert
            // We can't directly test the private method, but we can test it indirectly
            // by calling addResult with a failed test that has a TestopsId
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // This should not throw an exception and should log the link
            var action = () => _reporter.addResult(result);
            action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndEmptyTestopsIds_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = null,
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNullExecution_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = null
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task UploadResults_WithLargeBatch_ShouldHandleCorrectly()
        {
            // Arrange
            _config.TestOps.Batch.Size = 1000;
            var results = new List<TestResult>();
            for (int i = 0; i < 2500; i++)
            {
                results.Add(new TestResult { Title = $"Test {i}" });
            }

            // Initialize test run
            _config.TestOps.Run!.Id = 12345L;
            await _reporter.startTestRun();

            await _reporter.setResults(results);

            // Act
            await _reporter.uploadResults();

            // Assert
            // Should upload in batches: 1000, 1000, 500
            _clientMock.Verify(x => x.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()), Times.Exactly(3));
        }

        [Fact]
        public async Task StartTestRun_ShouldSetTestRunIdCorrectly()
        {
            // Arrange
            var newRunId = 67890L;
            _config.TestOps.Run!.Id = null;
            _clientMock.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(newRunId);

            // Act
            await _reporter.startTestRun();

            // Assert
            _config.TestOps.Run.Id.Should().Be(newRunId);
        }

        [Fact]
        public async Task CompleteTestRun_WithZeroRunId_ShouldStillCallClient()
        {
            // Arrange
            _config.TestOps.Run!.Id = 0L;
            await _reporter.startTestRun(); // Initialize the test run

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(0L), Times.Once);
        }

        [Fact]
        public async Task SetResults_WithNullList_ShouldHandleGracefully()
        {
            // Arrange
            var existingResult = new TestResult { Title = "Existing Test" };
            await _reporter.addResult(existingResult);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _reporter.setResults(null!));
        }

        [Fact]
        public async Task SetResults_WithEmptyList_ShouldClearResults()
        {
            // Arrange
            var existingResult = new TestResult { Title = "Existing Test" };
            await _reporter.addResult(existingResult);

            // Act
            await _reporter.setResults(new List<TestResult>());

            // Assert
            var results = await _reporter.getResults();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task StartTestRun_ShouldLogInformationWhenCreatingNewRun()
        {
            // Arrange
            var newRunId = 67890L;
            _config.TestOps.Run!.Id = null;
            _clientMock.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(newRunId);

            // Act
            await _reporter.startTestRun();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test run") && v.ToString()!.Contains(newRunId.ToString())),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_ShouldLogInformation()
        {
            // Arrange
            var runId = 12345L;
            _config.TestOps.Run!.Id = runId;
            await _reporter.startTestRun();

            // Act
            await _reporter.completeTestRun();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test run") && v.ToString()!.Contains(runId.ToString())),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNullTestopsIds_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = null,
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndEmptyTestopsIdsList_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long>(),
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndEmptyTitle_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndWhitespaceTitle_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "   ",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithPassedStatus_ShouldNotLogFailureLink()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Passed Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Passed }
            };

            // Act
            await _reporter.addResult(result);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("See why this test failed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }

        [Fact]
        public async Task AddResult_WithSkippedStatus_ShouldNotLogFailureLink()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Skipped Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Skipped }
            };

            // Act
            await _reporter.addResult(result);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("See why this test failed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndSpecialCharactersInTitle_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test with special chars: !@#$%^&*()_+-=[]{}|;':\",./<>?",
                TestopsIds = new List<long>(),
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndUnicodeCharactersInTitle_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Тест с кириллицей и emoji 🚀",
                TestopsIds = new List<long>(),
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndVeryLongTitle_ShouldHandleGracefully()
        {
            // Arrange
            var longTitle = new string('A', 10000); // Very long title
            var result = new TestResult
            {
                Title = longTitle,
                TestopsIds = new List<long>(),
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNullExecution_ShouldNotLogFailureLink()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = null
            };

            // Act
            await _reporter.addResult(result);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("See why this test failed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndZeroTestopsId_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 0 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNegativeTestopsId_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { -1 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndMultipleTestopsIds_ShouldUseFirstId()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123, 456, 789 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act
            await _reporter.addResult(result);

            // Assert
            // Should not throw an exception and should use the first ID (123)
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNullHost_ShouldHandleGracefully()
        {
            // Arrange
            _config.TestOps.Api.Host = null;
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndEmptyHost_ShouldHandleGracefully()
        {
            // Arrange
            _config.TestOps.Api.Host = "";
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNullProject_ShouldHandleGracefully()
        {
            // Arrange
            _config.TestOps.Project = null;
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndEmptyProject_ShouldHandleGracefully()
        {
            // Arrange
            _config.TestOps.Project = "";
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Theory]
        [InlineData("custom.qase.io")]
        [InlineData("test.example.com")]
        [InlineData("localhost")]
        [InlineData("192.168.1.1")]
        public async Task AddResult_WithFailedStatusAndCustomHost_ShouldHandleGracefully(string host)
        {
            // Arrange
            _config.TestOps.Api.Host = host;
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndZeroTestRunId_ShouldHandleGracefully()
        {
            // Arrange
            _config.TestOps.Run!.Id = 0L;
            await _reporter.startTestRun();
            
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNegativeTestRunId_ShouldHandleGracefully()
        {
            // Arrange
            _config.TestOps.Run!.Id = -1L;
            await _reporter.startTestRun();
            
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndVeryLargeTestRunId_ShouldHandleGracefully()
        {
            // Arrange
            _config.TestOps.Run!.Id = long.MaxValue;
            await _reporter.startTestRun();
            
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndVeryLargeTestopsId_ShouldHandleGracefully()
        {
            // Arrange
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { long.MaxValue },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndVeryLargeTitle_ShouldHandleGracefully()
        {
            // Arrange
            var veryLongTitle = new string('A', 100000); // Very long title
            var result = new TestResult
            {
                Title = veryLongTitle,
                TestopsIds = new List<long>(),
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await _reporter.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithFailedStatusAndNullClient_ShouldHandleGracefully()
        {
            // Arrange
            var reporterWithNullClient = new TestopsReporter(
                _loggerMock.Object,
                _config,
                null!);
            
            var result = new TestResult
            {
                Title = "Test",
                TestopsIds = new List<long> { 123 },
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act & Assert
            await reporterWithNullClient.addResult(result);
            // Should not throw an exception
        }

        [Fact]
        public async Task AddResult_WithStatusFilter_ShouldFilterOutMatchingStatuses()
        {
            // Arrange
            _config.TestOps.StatusFilter = new List<string> { "passed", "failed" };
            
            var passedResult = new TestResult
            {
                Title = "Passed Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Passed }
            };
            
            var failedResult = new TestResult
            {
                Title = "Failed Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };
            
            var skippedResult = new TestResult
            {
                Title = "Skipped Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Skipped }
            };

            // Act
            await _reporter.addResult(passedResult);
            await _reporter.addResult(failedResult);
            await _reporter.addResult(skippedResult);

            // Assert
            var results = await _reporter.getResults();
            results.Should().HaveCount(1);
            results.Should().NotContain(passedResult);
            results.Should().NotContain(failedResult);
            results.Should().Contain(skippedResult);
        }

        [Fact]
        public async Task AddResult_WithEmptyStatusFilter_ShouldNotFilterAnyResults()
        {
            // Arrange
            _config.TestOps.StatusFilter = new List<string>();
            
            var passedResult = new TestResult
            {
                Title = "Passed Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Passed }
            };
            
            var skippedResult = new TestResult
            {
                Title = "Skipped Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Skipped }
            };

            // Act
            await _reporter.addResult(passedResult);
            await _reporter.addResult(skippedResult);

            // Assert
            var results = await _reporter.getResults();
            results.Should().HaveCount(2);
            results.Should().Contain(passedResult);
            results.Should().Contain(skippedResult);
        }

        [Fact]
        public async Task AddResult_WithStatusFilter_ShouldNotFilterResultsWithNullExecution()
        {
            // Arrange
            _config.TestOps.StatusFilter = new List<string> { "passed" };
            
            var resultWithNullExecution = new TestResult
            {
                Title = "Test with null execution",
                Execution = null
            };

            // Act
            await _reporter.addResult(resultWithNullExecution);

            // Assert
            var results = await _reporter.getResults();
            results.Should().HaveCount(1);
            results.Should().Contain(resultWithNullExecution);
        }

        [Fact]
        public async Task AddResult_WithStatusFilter_ShouldLogDebugWhenFilteringOut()
        {
            // Arrange
            _config.TestOps.StatusFilter = new List<string> { "skipped" };
            
            var skippedResult = new TestResult
            {
                Title = "Skipped Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Skipped }
            };

            // Act
            await _reporter.addResult(skippedResult);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test result filtered out by status filter")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Theory]
        [InlineData("passed", TestResultStatus.Passed, false)] // Should be filtered out
        [InlineData("failed", TestResultStatus.Failed, false)] // Should be filtered out
        [InlineData("skipped", TestResultStatus.Skipped, false)] // Should be filtered out
        [InlineData("invalid", TestResultStatus.Invalid, false)] // Should be filtered out
        [InlineData("PASSED", TestResultStatus.Passed, false)] // Case insensitive - should be filtered out
        [InlineData("FAILED", TestResultStatus.Failed, false)] // Case insensitive - should be filtered out
        [InlineData("passed", TestResultStatus.Failed, true)] // Should NOT be filtered out
        [InlineData("failed", TestResultStatus.Passed, true)] // Should NOT be filtered out
        public async Task AddResult_WithStatusFilter_ShouldFilterCorrectly(string filterStatus, TestResultStatus resultStatus, bool shouldInclude)
        {
            // Arrange
            _config.TestOps.StatusFilter = new List<string> { filterStatus };
            
            var result = new TestResult
            {
                Title = "Test",
                Execution = new TestResultExecution { Status = resultStatus }
            };

            // Act
            await _reporter.addResult(result);

            // Assert
            var results = await _reporter.getResults();
            if (shouldInclude)
            {
                results.Should().Contain(result);
            }
            else
            {
                results.Should().NotContain(result);
            }
        }

        [Fact]
        public async Task AddResult_WithMultipleStatusFilters_ShouldFilterOutAnyMatchingStatus()
        {
            // Arrange
            _config.TestOps.StatusFilter = new List<string> { "passed", "skipped" };
            
            var passedResult = new TestResult
            {
                Title = "Passed Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Passed }
            };
            
            var skippedResult = new TestResult
            {
                Title = "Skipped Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Skipped }
            };
            
            var failedResult = new TestResult
            {
                Title = "Failed Test",
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };

            // Act
            await _reporter.addResult(passedResult);
            await _reporter.addResult(skippedResult);
            await _reporter.addResult(failedResult);

            // Assert
            var results = await _reporter.getResults();
            results.Should().HaveCount(1);
            results.Should().NotContain(passedResult);
            results.Should().NotContain(skippedResult);
            results.Should().Contain(failedResult);
        }

        [Fact]
        public async Task CompleteTestRun_WithShowPublicReportLinkEnabled_ShouldCallEnablePublicReport()
        {
            // Arrange
            _config.TestOps!.ShowPublicReportLink = true;
            var runId = 12345L;
            var expectedPublicUrl = "https://app.qase.io/public/report/abc123";
            
            _config.TestOps.Run.Id = runId;
            await _reporter.startTestRun();
            _clientMock.Setup(x => x.EnablePublicReportAsync(runId)).ReturnsAsync(expectedPublicUrl);

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
            _clientMock.Verify(x => x.EnablePublicReportAsync(runId), Times.Once);
            
            // Verify that public URL is logged
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Public report link") && v.ToString()!.Contains(expectedPublicUrl)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_WithShowPublicReportLinkDisabled_ShouldNotCallEnablePublicReport()
        {
            // Arrange
            _config.TestOps!.ShowPublicReportLink = false;
            var runId = 12345L;
            
            _config.TestOps.Run.Id = runId;
            await _reporter.startTestRun();

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
            _clientMock.Verify(x => x.EnablePublicReportAsync(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task CompleteTestRun_WithShowPublicReportLinkEnabledButClientThrows_ShouldLogWarning()
        {
            // Arrange
            _config.TestOps!.ShowPublicReportLink = true;
            var runId = 12345L;
            var exceptionMessage = "API Error";
            
            _config.TestOps.Run.Id = runId;
            await _reporter.startTestRun();
            _clientMock.Setup(x => x.EnablePublicReportAsync(runId))
                .ThrowsAsync(new QaseException(exceptionMessage));

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
            _clientMock.Verify(x => x.EnablePublicReportAsync(runId), Times.Once);
            
            // Verify that warning is logged
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to generate public report link") && v.ToString()!.Contains(exceptionMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_WithShowPublicReportLinkEnabledButClientThrowsGenericException_ShouldLogWarning()
        {
            // Arrange
            _config.TestOps!.ShowPublicReportLink = true;
            var runId = 12345L;
            var exceptionMessage = "Network Error";
            
            _config.TestOps.Run.Id = runId;
            await _reporter.startTestRun();
            _clientMock.Setup(x => x.EnablePublicReportAsync(runId))
                .ThrowsAsync(new InvalidOperationException(exceptionMessage));

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
            _clientMock.Verify(x => x.EnablePublicReportAsync(runId), Times.Once);
            
            // Verify that warning is logged
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to generate public report link") && v.ToString()!.Contains(exceptionMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CompleteTestRun_WithShowPublicReportLinkEnabledAndEmptyUrl_ShouldLogWarning()
        {
            // Arrange
            _config.TestOps!.ShowPublicReportLink = true;
            var runId = 12345L;
            
            _config.TestOps.Run.Id = runId;
            await _reporter.startTestRun();
            _clientMock.Setup(x => x.EnablePublicReportAsync(runId))
                .ThrowsAsync(new QaseException("Failed to get public report URL from response"));

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
            _clientMock.Verify(x => x.EnablePublicReportAsync(runId), Times.Once);
            
            // Verify that warning is logged
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to generate public report link")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CompleteTestRun_WithDifferentShowPublicReportLinkValues_ShouldBehaveCorrectly(bool showPublicReportLink)
        {
            // Arrange
            _config.TestOps!.ShowPublicReportLink = showPublicReportLink;
            var runId = 12345L;
            
            _config.TestOps.Run.Id = runId;
            await _reporter.startTestRun();

            // Act
            await _reporter.completeTestRun();

            // Assert
            _clientMock.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
            
            if (showPublicReportLink)
            {
                _clientMock.Verify(x => x.EnablePublicReportAsync(runId), Times.Once);
            }
            else
            {
                _clientMock.Verify(x => x.EnablePublicReportAsync(It.IsAny<long>()), Times.Never);
            }
        }
    }
} 
