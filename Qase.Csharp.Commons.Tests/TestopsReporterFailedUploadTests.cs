using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;

namespace Qase.Csharp.Commons.Tests;

/// <summary>
/// What happens to the buffer and to the test run when an upload cannot be recovered.
/// </summary>
public class TestopsReporterFailedUploadTests
{
    private readonly Mock<IClient> _client = new();
    private readonly QaseConfig _config;
    private readonly TestopsReporter _reporter;

    public TestopsReporterFailedUploadTests()
    {
        _config = new QaseConfig
        {
            TestOps = new TestOpsConfig
            {
                Project = "TEST",
                Api = new ApiConfig { Host = "qase.io" },
                Batch = new BatchConfig { Size = 10 },
                Run = new RunConfig { Id = 42 }
            }
        };
        _reporter = new TestopsReporter(NullLogger<TestopsReporter>.Instance, _config, _client.Object);
    }

    private static TestResult Result(string title) => new()
    {
        Title = title,
        Execution = new TestResultExecution { Status = TestResultStatus.Passed }
    };

    private async Task BufferAsync(int count)
    {
        await _reporter.startTestRun();
        for (var i = 0; i < count; i++)
        {
            await _reporter.addResult(Result($"test {i}"));
        }
    }

    [Fact]
    public async Task UploadResults_ShouldKeepTheBufferWhenTheUploadThrows()
    {
        _client.Setup(c => c.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()))
            .ThrowsAsync(new QaseException("retries exhausted"));
        await BufferAsync(3);

        var act = async () => await _reporter.uploadResults();

        await act.Should().ThrowAsync<QaseException>();
        var kept = await _reporter.getResults();
        kept.Should().HaveCount(3, "results that were never delivered must stay available to the fallback reporter");
    }

    [Fact]
    public async Task AddResult_ShouldKeepTheBufferWhenTheBatchUploadThrows()
    {
        _client.Setup(c => c.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()))
            .ThrowsAsync(new QaseException("retries exhausted"));
        await _reporter.startTestRun();

        // The tenth result trips the configured batch size of 10.
        var act = async () =>
        {
            for (var i = 0; i < 10; i++)
            {
                await _reporter.addResult(Result($"test {i}"));
            }
        };

        await act.Should().ThrowAsync<QaseException>();
        (await _reporter.getResults()).Should().HaveCount(10);
    }

    [Fact]
    public async Task UploadResults_ShouldClearTheBufferOnSuccess()
    {
        await BufferAsync(3);

        await _reporter.uploadResults();

        (await _reporter.getResults()).Should().BeEmpty();
    }

    [Fact]
    public async Task CompleteTestRun_ShouldNotCompleteARunThatLostResults()
    {
        _client.Setup(c => c.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()))
            .ThrowsAsync(new QaseException("retries exhausted"));
        await BufferAsync(3);
        await Assert.ThrowsAsync<QaseException>(() => _reporter.uploadResults());

        await _reporter.completeTestRun();

        _client.Verify(c => c.CompleteTestRunAsync(It.IsAny<long>()), Times.Never,
            "a completed run over partial data looks trustworthy and is not");
    }

    [Fact]
    public async Task CompleteTestRun_ShouldCompleteARunThatLostNothing()
    {
        await BufferAsync(3);
        await _reporter.uploadResults();

        await _reporter.completeTestRun();

        _client.Verify(c => c.CompleteTestRunAsync(42), Times.Once);
    }

    [Fact]
    public async Task UploadResults_ShouldReportHowManyResultsWereLost()
    {
        var logger = new Mock<ILogger<TestopsReporter>>();
        var reporter = new TestopsReporter(logger.Object, _config, _client.Object);
        _client.Setup(c => c.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()))
            .ThrowsAsync(new QaseException("retries exhausted"));
        await reporter.startTestRun();
        for (var i = 0; i < 3; i++)
        {
            await reporter.addResult(Result($"test {i}"));
        }

        await Assert.ThrowsAsync<QaseException>(() => reporter.uploadResults());

        logger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("3")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task UploadResults_ShouldOnlyDropChunksTheServerAccepted()
    {
        _config.TestOps.Batch.Size = 2;
        var sent = new List<List<TestResult>>();
        _client.Setup(c => c.UploadResultsAsync(It.IsAny<long>(), It.IsAny<List<TestResult>>()))
            .Returns((long _, List<TestResult> batch) =>
            {
                sent.Add(batch.ToList());
                // The first chunk lands, the second one does not.
                return sent.Count == 1 ? Task.CompletedTask : Task.FromException(new QaseException("boom"));
            });
        await _reporter.startTestRun();
        await _reporter.setResults(Enumerable.Range(0, 6).Select(i => Result($"test {i}")).ToList());

        await Assert.ThrowsAsync<QaseException>(() => _reporter.uploadResults());

        var kept = await _reporter.getResults();
        kept.Should().HaveCount(4, "the two results already delivered must not be handed to the fallback again");
        kept.Select(r => r.Title).Should().Equal("test 2", "test 3", "test 4", "test 5");
    }
}
