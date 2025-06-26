using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class ClientTests
    {
        [Fact]
        public async Task CreateTestRunAsync_ShouldReturnRunId()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var expectedRunId = 12345L;
            mockClient.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(expectedRunId);

            // Act
            var result = await mockClient.Object.CreateTestRunAsync();

            // Assert
            result.Should().Be(expectedRunId);
            mockClient.Verify(x => x.CreateTestRunAsync(), Times.Once);
        }

        [Fact]
        public async Task CompleteTestRunAsync_ShouldCompleteRun()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var runId = 12345L;
            mockClient.Setup(x => x.CompleteTestRunAsync(runId)).Returns(Task.CompletedTask);

            // Act
            await mockClient.Object.CompleteTestRunAsync(runId);

            // Assert
            mockClient.Verify(x => x.CompleteTestRunAsync(runId), Times.Once);
        }

        [Fact]
        public async Task UploadResultsAsync_ShouldUploadResults()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var runId = 12345L;
            var results = new List<TestResult>
            {
                new TestResult
                {
                    Id = "1",
                    Title = "Test 1",
                    Message = "Test passed"
                },
                new TestResult
                {
                    Id = "2",
                    Title = "Test 2",
                    Message = "Test failed"
                }
            };
            mockClient.Setup(x => x.UploadResultsAsync(runId, results)).Returns(Task.CompletedTask);

            // Act
            await mockClient.Object.UploadResultsAsync(runId, results);

            // Assert
            mockClient.Verify(x => x.UploadResultsAsync(runId, results), Times.Once);
        }

        [Fact]
        public async Task GetTestCaseIdsForExecutionAsync_ShouldReturnTestCaseIds()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var expectedIds = new List<long> { 1, 2, 3, 4, 5 };
            mockClient.Setup(x => x.GetTestCaseIdsForExecutionAsync()).ReturnsAsync(expectedIds);

            // Act
            var result = await mockClient.Object.GetTestCaseIdsForExecutionAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedIds);
            mockClient.Verify(x => x.GetTestCaseIdsForExecutionAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateTestRunAsync_WhenExceptionOccurs_ShouldThrowQaseException()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var exception = new QaseException("API Error");
            mockClient.Setup(x => x.CreateTestRunAsync()).ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<QaseException>(() => mockClient.Object.CreateTestRunAsync());
        }

        [Fact]
        public async Task CompleteTestRunAsync_WhenExceptionOccurs_ShouldThrowQaseException()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var runId = 12345L;
            var exception = new QaseException("API Error");
            mockClient.Setup(x => x.CompleteTestRunAsync(runId)).ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<QaseException>(() => mockClient.Object.CompleteTestRunAsync(runId));
        }

        [Fact]
        public async Task UploadResultsAsync_WhenExceptionOccurs_ShouldThrowQaseException()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var runId = 12345L;
            var results = new List<TestResult>();
            var exception = new QaseException("API Error");
            mockClient.Setup(x => x.UploadResultsAsync(runId, results)).ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<QaseException>(() => mockClient.Object.UploadResultsAsync(runId, results));
        }

        [Fact]
        public async Task GetTestCaseIdsForExecutionAsync_WhenExceptionOccurs_ShouldThrowQaseException()
        {
            // Arrange
            var mockClient = new Mock<IClient>();
            var exception = new QaseException("API Error");
            mockClient.Setup(x => x.GetTestCaseIdsForExecutionAsync()).ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<QaseException>(() => mockClient.Object.GetTestCaseIdsForExecutionAsync());
        }
    }
} 
