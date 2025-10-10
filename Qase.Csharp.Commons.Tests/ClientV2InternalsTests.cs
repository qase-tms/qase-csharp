using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V2.Api;
using Qase.ApiClient.V2.Model;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class ClientV2InternalsTests
    {
        private readonly Mock<ILogger<ClientV2>> _loggerMock = new();
        private readonly Mock<ILogger<ClientV1>> _clientV1LoggerMock = new();
        private readonly Mock<IRunsApi> _runsApiMock = new();
        private readonly Mock<IAttachmentsApi> _attachmentsApiMock = new();
        private readonly Mock<IConfigurationsApi> _configurationsApiMock = new();
        private readonly Mock<Qase.ApiClient.V2.Api.IResultsApi> _resultsApiMock = new();
        private readonly QaseConfig _config;
        private readonly ClientV1 _clientV1;
        private readonly ClientV2 _client;

        public ClientV2InternalsTests()
        {
            _config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Project = "PRJ",
                    Defect = true,
                    Api = new ApiConfig
                    {
                        Host = "qase.io"
                    }
                }
            };
            
            _clientV1 = new ClientV1(_clientV1LoggerMock.Object, _config, _runsApiMock.Object, _attachmentsApiMock.Object, _configurationsApiMock.Object);
            _client = new ClientV2(_loggerMock.Object, _config, _clientV1, _resultsApiMock.Object);
        }

        [Fact]
        public void ConvertResult_ShouldConvertAllFieldsCorrectly()
        {
            // Arrange
            var testResult = new TestResult
            {
                Id = "id123",
                Title = "Test Title",
                Signature = "sig",
                TestopsIds = new List<long> { 1, 2 },
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Passed,
                    StartTime = 10000,
                    EndTime = 20000,
                    Duration = 10000,
                    Stacktrace = "stack",
                    Thread = "thread"
                },
                Fields = new Dictionary<string, string>
                {
                    {"author", "auth"},
                    {"description", "desc"},
                    {"preconditions", "pre"},
                    {"postconditions", "post"},
                    {"layer", "l"},
                    {"severity", "sev"},
                    {"priority", "prio"},
                    {"behavior", "beh"},
                    {"type", "t"},
                    {"muted", "m"},
                    {"isFlaky", "f"},
                    {"custom", "customValue"}
                },
                Attachments = new List<Attachment> { new Attachment { FilePath = "file1" }, new Attachment { FilePath = "file2" } },
                Steps = new List<StepResult> { new StepResult { Data = new Data { Action = "act" } } },
                Params = new Dictionary<string, string> { { "p1", "v1" } },
                ParamGroups = new List<List<string>> { new List<string> { "g1", "g2" } },
                Relations = new Relations
                {
                    Suite = new Suite
                    {
                        Data = new List<SuiteData> { new SuiteData { Title = "suiteTitle" } }
                    }
                },
                Message = "msg"
            };

            // Act
            var result = _client.ConvertResult(testResult);

            // Assert
            result.Title.Should().Be("Test Title");
            result.Id.Should().Be("id123");
            result.Signature.Should().Be("sig");
            result.TestopsIds.Should().BeEquivalentTo(new List<long> { 1, 2 });
            result.Execution.Status.Should().Be("passed");
            result.Execution.StartTime.Should().Be(10);
            result.Execution.EndTime.Should().Be(20);
            result.Execution.Duration.Should().Be(10000);
            result.Execution.Stacktrace.Should().Be("stack");
            result.Execution.Thread.Should().Be("thread");
            result.Fields!.Author.Should().Be("auth");
            result.Fields.Description.Should().Be("desc");
            result.Fields.Preconditions.Should().Be("pre");
            result.Fields.Postconditions.Should().Be("post");
            result.Fields.Layer.Should().Be("l");
            result.Fields.Severity.Should().Be("sev");
            result.Fields.Priority.Should().Be("prio");
            result.Fields.Behavior.Should().Be("beh");
            result.Fields.Type.Should().Be("t");
            result.Fields.Muted.Should().Be("m");
            result.Fields.IsFlaky.Should().Be("f");
            result.Fields.AdditionalProperties.Should().ContainKey("custom");
            result.Attachments.Should().BeEmpty(); // No attachments uploaded in test
            result.Steps.Should().HaveCount(1);
            result.Params.Should().ContainKey("p1");
            result.ParamGroups.Should().NotBeNull();
            result.Relations!.Suite!.Data!.First().Title.Should().Be("suiteTitle");
            result.Message.Should().Be("msg");
            result.Defect.Should().BeTrue();
        }

        [Fact]
        public void ConvertResult_ShouldHandleNullAndEmptyCollections()
        {
            // Arrange
            var testResult = new TestResult
            {
                Title = "Test",
                Execution = null,
                Fields = new Dictionary<string, string>(),
                Attachments = new List<Attachment>(),
                Steps = new List<StepResult>(),
                Params = new Dictionary<string, string>(),
                ParamGroups = new List<List<string>>(),
                Relations = new Relations()
            };

            // Act
            var result = _client.ConvertResult(testResult);

            // Assert
            result.Title.Should().Be("Test");
            result.Execution.Status.Should().Be("failed"); // default for null
            result.Fields.Should().NotBeNull();
            result.Attachments.Should().BeEmpty();
            result.Steps.Should().BeEmpty();
            result.Params.Should().NotBeNull();
            result.ParamGroups.Should().NotBeNull();
            result.Relations.Should().NotBeNull();
        }

        [Theory]
        [InlineData(TestResultStatus.Passed, "passed")]
        [InlineData(TestResultStatus.Failed, "failed")]
        [InlineData(TestResultStatus.Skipped, "skipped")]
        [InlineData(null, "failed")]
        public void ConvertResult_ShouldMapStatusCorrectly(TestResultStatus? status, string expected)
        {
            var testResult = new TestResult
            {
                Title = "Test",
                Execution = status.HasValue ? new TestResultExecution { Status = status.Value } : null
            };
            var result = _client.ConvertResult(testResult);
            result.Execution.Status.Should().Be(expected);
        }

        [Fact]
        public void ConvertStepResult_ShouldConvertAllFieldsCorrectly()
        {
            // Arrange
            var step = new StepResult
            {
                Data = new Data { Action = "act", ExpectedResult = "exp", InputData = "inp" },
                Execution = new StepExecution
                {
                    Status = StepResultStatus.Passed,
                    StartTime = 10000,
                    EndTime = 20000,
                    Duration = 10000,
                    Attachments = new List<Attachment> { new Attachment { FilePath = "file1" } }
                },
                Steps = new List<StepResult> { new StepResult { Data = new Data { Action = "child" } } }
            };

            // Act
            var result = _client.ConvertStepResult(step);

            // Assert
            result.Data!.Action.Should().Be("act");
            result.Data!.ExpectedResult.Should().Be("exp");
            result.Data!.InputData.Should().Be("inp");
            result.Execution!.Status.Should().Be(ResultStepStatus.Passed);
            result.Execution!.StartTime.Should().Be(10);
            result.Execution!.EndTime.Should().Be(20);
            result.Execution!.Duration.Should().Be(10000);
            result.Execution!.Attachments.Should().BeEmpty(); // No attachments uploaded in test
            result.Steps.Should().HaveCount(1);
            result.Steps!.First().Data!.Action.Should().Be("child");
        }

        [Fact]
        public void ConvertStepResult_ShouldHandleNullsAndEmptyCollections()
        {
            // Arrange
            var step = new StepResult
            {
                Data = new Data(),
                Execution = new StepExecution(),
                Steps = new List<StepResult>()
            };

            // Act
            var result = _client.ConvertStepResult(step);

            // Assert
            result.Data!.Action.Should().Be("");
            result.Data!.ExpectedResult.Should().BeNull();
            result.Data!.InputData.Should().BeNull();
            result.Execution!.Status.Should().Be(ResultStepStatus.Failed);
            result.Execution!.StartTime.Should().BeGreaterThan(0);
            result.Execution!.EndTime.Should().BeNull();
            result.Execution!.Duration.Should().BeNull();
            result.Execution!.Attachments.Should().BeEmpty();
            result.Steps.Should().BeEmpty();
        }

        [Theory]
        [InlineData(StepResultStatus.Passed, ResultStepStatus.Passed)]
        [InlineData(StepResultStatus.Failed, ResultStepStatus.Failed)]
        [InlineData(StepResultStatus.Skipped, ResultStepStatus.Skipped)]
        [InlineData(StepResultStatus.Blocked, ResultStepStatus.Blocked)]
        [InlineData(null, ResultStepStatus.Skipped)]
        public void ConvertStepResult_ShouldMapStatusCorrectly(StepResultStatus? status, ResultStepStatus expected)
        {
            var step = new StepResult
            {
                Data = new Data { Action = "a" },
                Execution = status.HasValue ? new StepExecution { Status = status.Value } : null
            };
            var result = _client.ConvertStepResult(step);
            result.Execution!.Status.Should().Be(expected);
        }

        [Fact]
        public async Task EnablePublicReportAsync_ShouldDelegateToClientV1()
        {
            // Arrange
            var runId = 12345L;
            var expectedUrl = "https://app.qase.io/public/report/abc123";
            
            var mockClientV1 = new Mock<ClientV1>(_clientV1LoggerMock.Object, _config, _runsApiMock.Object, _attachmentsApiMock.Object, _configurationsApiMock.Object);
            mockClientV1.Setup(x => x.EnablePublicReportAsync(runId)).ReturnsAsync(expectedUrl);
            
            var client = new ClientV2(_loggerMock.Object, _config, mockClientV1.Object, _resultsApiMock.Object);

            // Act
            var result = await client.EnablePublicReportAsync(runId);

            // Assert
            result.Should().Be(expectedUrl);
        }

        [Fact]
        public async Task EnablePublicReportAsync_WithClientV1Exception_ShouldPropagateException()
        {
            // Arrange
            var runId = 12345L;
            var expectedException = new QaseException("API Error");
            
            var mockClientV1 = new Mock<ClientV1>(_clientV1LoggerMock.Object, _config, _runsApiMock.Object, _attachmentsApiMock.Object, _configurationsApiMock.Object);
            mockClientV1.Setup(x => x.EnablePublicReportAsync(runId)).ThrowsAsync(expectedException);
            
            var client = new ClientV2(_loggerMock.Object, _config, mockClientV1.Object, _resultsApiMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QaseException>(() => client.EnablePublicReportAsync(runId));
            exception.Should().Be(expectedException);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(long.MaxValue)]
        public async Task EnablePublicReportAsync_WithDifferentRunIds_ShouldDelegateCorrectly(long runId)
        {
            // Arrange
            var expectedUrl = "https://app.qase.io/public/report/test";
            
            var mockClientV1 = new Mock<ClientV1>(_clientV1LoggerMock.Object, _config, _runsApiMock.Object, _attachmentsApiMock.Object, _configurationsApiMock.Object);
            mockClientV1.Setup(x => x.EnablePublicReportAsync(runId)).ReturnsAsync(expectedUrl);
            
            var client = new ClientV2(_loggerMock.Object, _config, mockClientV1.Object, _resultsApiMock.Object);

            // Act
            var result = await client.EnablePublicReportAsync(runId);

            // Assert
            result.Should().Be(expectedUrl);
        }
    }
} 
