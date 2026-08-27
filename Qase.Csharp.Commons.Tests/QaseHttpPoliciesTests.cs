using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using Polly.Timeout;
using Qase.Csharp.Commons.Clients;

namespace Qase.Csharp.Commons.Tests;

public class QaseHttpPoliciesTests
{
    [Theory]
    [InlineData(HttpStatusCode.RequestTimeout)]        // 408
    [InlineData((HttpStatusCode)429)]                  // 429 Too Many Requests
    [InlineData(HttpStatusCode.InternalServerError)]   // 500
    [InlineData(HttpStatusCode.BadGateway)]            // 502
    [InlineData(HttpStatusCode.ServiceUnavailable)]    // 503
    [InlineData(HttpStatusCode.GatewayTimeout)]        // 504
    public void IsRetryableStatus_ShouldRetryTransientStatuses(HttpStatusCode status)
    {
        QaseHttpPolicies.IsRetryableStatus(status).Should().BeTrue();
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]            // 400
    [InlineData(HttpStatusCode.Unauthorized)]          // 401
    [InlineData(HttpStatusCode.Forbidden)]             // 403
    [InlineData(HttpStatusCode.NotFound)]              // 404
    [InlineData(HttpStatusCode.RequestEntityTooLarge)] // 413
    [InlineData((HttpStatusCode)422)]                  // 422 Unprocessable Content
    [InlineData((HttpStatusCode)507)]                  // 507 Insufficient Storage
    public void IsRetryableStatus_ShouldNotRetryPermanentFailures(HttpStatusCode status)
    {
        QaseHttpPolicies.IsRetryableStatus(status).Should().BeFalse();
    }

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.Created)]
    [InlineData(HttpStatusCode.NoContent)]
    public void IsRetryableStatus_ShouldNotRetrySuccessfulResponses(HttpStatusCode status)
    {
        QaseHttpPolicies.IsRetryableStatus(status).Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 1.0)]
    [InlineData(2, 2.0)]
    [InlineData(3, 4.0)]
    [InlineData(4, 8.0)]
    public void ComputeDelay_ShouldGrowExponentiallyFromTheBaseBackoff(int attempt, double expectedSeconds)
    {
        using var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);

        var delay = QaseHttpPolicies.ComputeDelay(attempt, TimeSpan.FromSeconds(1), response);

        delay.Should().Be(TimeSpan.FromSeconds(expectedSeconds));
    }

    [Fact]
    public void ComputeDelay_ShouldScaleWithTheConfiguredBaseBackoff()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);

        var delay = QaseHttpPolicies.ComputeDelay(3, TimeSpan.FromMilliseconds(500), response);

        delay.Should().Be(TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void ComputeDelay_ShouldFallBackToBackoffWhenThereIsNoResponse()
    {
        var delay = QaseHttpPolicies.ComputeDelay(2, TimeSpan.FromSeconds(1), null);

        delay.Should().Be(TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void ComputeDelay_ShouldHonourRetryAfterDeltaSecondsOverTheComputedBackoff()
    {
        using var response = new HttpResponseMessage((HttpStatusCode)429);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(60));

        // The computed backoff for the first attempt would be 1 second.
        var delay = QaseHttpPolicies.ComputeDelay(1, TimeSpan.FromSeconds(1), response);

        delay.Should().Be(TimeSpan.FromSeconds(60));
    }

    [Fact]
    public void ComputeDelay_ShouldHonourRetryAfterAsHttpDate()
    {
        using var response = new HttpResponseMessage((HttpStatusCode)429);
        response.Headers.Date = DateTimeOffset.UtcNow;
        response.Headers.RetryAfter =
            new RetryConditionHeaderValue(DateTimeOffset.UtcNow.AddSeconds(45));

        var delay = QaseHttpPolicies.ComputeDelay(1, TimeSpan.FromSeconds(1), response);

        delay.Should().BeCloseTo(TimeSpan.FromSeconds(45), TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void ComputeDelay_ShouldIgnoreRetryAfterDateAlreadyInThePast()
    {
        using var response = new HttpResponseMessage((HttpStatusCode)429);
        response.Headers.RetryAfter =
            new RetryConditionHeaderValue(DateTimeOffset.UtcNow.AddSeconds(-30));

        var delay = QaseHttpPolicies.ComputeDelay(2, TimeSpan.FromSeconds(1), response);

        delay.Should().Be(TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void ComputeDelay_ShouldUseRetryAfterEvenWhenItIsShorterThanTheBackoff()
    {
        using var response = new HttpResponseMessage((HttpStatusCode)429);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(2));

        // The computed backoff for the fourth attempt would be 8 seconds.
        var delay = QaseHttpPolicies.ComputeDelay(4, TimeSpan.FromSeconds(1), response);

        delay.Should().Be(TimeSpan.FromSeconds(2));
    }

    private static readonly TimeSpan FastBackoff = TimeSpan.FromMilliseconds(1);

    /// <summary>
    /// Counts how many times the policy asked for the operation, and replays
    /// the given responses in order (repeating the last one once exhausted).
    /// </summary>
    private sealed class ResponseSequence
    {
        private readonly HttpStatusCode[] _statuses;

        public ResponseSequence(params HttpStatusCode[] statuses) => _statuses = statuses;

        public int Attempts { get; private set; }

        public Task<HttpResponseMessage> NextAsync()
        {
            var status = _statuses[Math.Min(Attempts, _statuses.Length - 1)];
            Attempts++;
            return Task.FromResult(new HttpResponseMessage(status));
        }
    }

    [Theory]
    [InlineData(HttpStatusCode.RequestTimeout)]
    [InlineData((HttpStatusCode)429)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task RetryPolicy_ShouldExhaustAllAttemptsOnRetryableStatuses(HttpStatusCode status)
    {
        var sequence = new ResponseSequence(status);
        var policy = QaseHttpPolicies.CreateRetryPolicy(3, FastBackoff, NullLogger.Instance);

        var response = await policy.ExecuteAsync(_ => sequence.NextAsync(), new Context());

        // One initial attempt plus three retries.
        sequence.Attempts.Should().Be(4);
        response.StatusCode.Should().Be(status);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.RequestEntityTooLarge)]
    [InlineData((HttpStatusCode)422)]
    [InlineData((HttpStatusCode)507)]
    public async Task RetryPolicy_ShouldNotRetryPermanentFailures(HttpStatusCode status)
    {
        var sequence = new ResponseSequence(status);
        var policy = QaseHttpPolicies.CreateRetryPolicy(3, FastBackoff, NullLogger.Instance);

        await policy.ExecuteAsync(_ => sequence.NextAsync(), new Context());

        sequence.Attempts.Should().Be(1);
    }

    [Fact]
    public async Task RetryPolicy_ShouldStopAsSoonAsAnAttemptSucceeds()
    {
        var sequence = new ResponseSequence(
            (HttpStatusCode)429, HttpStatusCode.ServiceUnavailable, HttpStatusCode.OK);
        var policy = QaseHttpPolicies.CreateRetryPolicy(5, FastBackoff, NullLogger.Instance);

        var response = await policy.ExecuteAsync(_ => sequence.NextAsync(), new Context());

        sequence.Attempts.Should().Be(3);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RetryPolicy_ShouldRetryTransportFailures()
    {
        var attempts = 0;
        var policy = QaseHttpPolicies.CreateRetryPolicy(2, FastBackoff, NullLogger.Instance);

        var act = async () => await policy.ExecuteAsync(_ =>
        {
            attempts++;
            throw new HttpRequestException("connection reset");
        }, new Context());

        await act.Should().ThrowAsync<HttpRequestException>();
        attempts.Should().Be(3);
    }

    [Fact]
    public async Task RetryPolicy_ShouldWaitForRetryAfterInsteadOfTheComputedBackoff()
    {
        var attempts = 0;
        var policy = QaseHttpPolicies.CreateRetryPolicy(1, FastBackoff, NullLogger.Instance);
        var stopwatch = Stopwatch.StartNew();

        await policy.ExecuteAsync(_ =>
        {
            attempts++;
            var response = new HttpResponseMessage((HttpStatusCode)429);
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(1));
            return Task.FromResult(response);
        }, new Context());

        stopwatch.Stop();
        attempts.Should().Be(2);
        // The 1 ms computed backoff would have made this return almost instantly.
        stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(900));
    }

    [Fact]
    public async Task TimeoutPolicy_ShouldAbortARequestThatStallsPastTheTimeout()
    {
        var policy = QaseHttpPolicies.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(100));

        var act = async () => await policy.ExecuteAsync(async token =>
        {
            await Task.Delay(TimeSpan.FromSeconds(10), token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }, CancellationToken.None);

        await act.Should().ThrowAsync<TimeoutRejectedException>();
    }

    [Fact]
    public async Task TimeoutPolicy_ShouldLetAFastRequestThrough()
    {
        var policy = QaseHttpPolicies.CreateTimeoutPolicy(TimeSpan.FromSeconds(5));

        var response = await policy.ExecuteAsync(
            _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)), CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RetryPolicy_ShouldRetryAttemptsKilledByTheTimeoutPolicy()
    {
        var attempts = 0;
        var retry = QaseHttpPolicies.CreateRetryPolicy(2, FastBackoff, NullLogger.Instance);
        var timeout = QaseHttpPolicies.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(100));

        // Retry wraps timeout, exactly as the handler chain orders them.
        var act = async () => await retry.WrapAsync(timeout).ExecuteAsync(async token =>
        {
            attempts++;
            await Task.Delay(TimeSpan.FromSeconds(10), token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }, CancellationToken.None);

        await act.Should().ThrowAsync<TimeoutRejectedException>();
        attempts.Should().Be(3);
    }

    [Theory]
    [InlineData(45, 45)]
    [InlineData(1, 1)]
    [InlineData(0, 30)]
    [InlineData(-5, 30)]
    public void ResolveTimeout_ShouldFallBackToTheDefaultForNonPositiveValues(int configured, int expected)
    {
        QaseHttpPolicies.ResolveTimeout(configured).Should().Be(TimeSpan.FromSeconds(expected));
    }

    [Theory]
    [InlineData(3, 3)]
    [InlineData(0, 0)]
    [InlineData(-2, 0)]
    public void ResolveRetries_ShouldClampNegativeValuesToZero(int configured, int expected)
    {
        QaseHttpPolicies.ResolveRetries(configured).Should().Be(expected);
    }

    [Theory]
    [InlineData(2.5, 2.5)]
    [InlineData(0.0, 1.0)]
    [InlineData(-1.0, 1.0)]
    public void ResolveBackoff_ShouldFallBackToTheDefaultForNonPositiveValues(double configured, double expected)
    {
        QaseHttpPolicies.ResolveBackoff(configured).Should().Be(TimeSpan.FromSeconds(expected));
    }

    [Theory]
    [InlineData(20)]
    [InlineData(40)]
    [InlineData(64)]
    public void ComputeDelay_ShouldCapTheBackoffInsteadOfOverflowing(int attempt)
    {
        using var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);

        var delay = QaseHttpPolicies.ComputeDelay(attempt, TimeSpan.FromSeconds(1), response);

        delay.Should().BePositive();
        delay.Should().BeLessThanOrEqualTo(TimeSpan.FromMinutes(10));
    }
}
