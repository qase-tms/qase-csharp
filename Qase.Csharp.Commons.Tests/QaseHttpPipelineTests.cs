using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;

namespace Qase.Csharp.Commons.Tests;

/// <summary>
/// Exercises the policies through a real HttpClient pipeline, which is the only
/// way to prove a POST body survives being sent more than once.
/// </summary>
public class QaseHttpPipelineTests
{
    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode[] _statuses;

        public RecordingHandler(params HttpStatusCode[] statuses) => _statuses = statuses;

        public List<string> ReceivedBodies { get; } = new();

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ReceivedBodies.Add(request.Content == null
                ? string.Empty
                : await request.Content.ReadAsStringAsync());

            var status = _statuses[Math.Min(ReceivedBodies.Count - 1, _statuses.Length - 1)];
            return new HttpResponseMessage(status);
        }
    }

    private static (HttpClient Client, RecordingHandler Handler) BuildClient(
        ApiConfig api, params HttpStatusCode[] statuses)
    {
        var handler = new RecordingHandler(statuses);
        var services = new ServiceCollection();
        services.AddHttpClient("qase")
            .ConfigurePrimaryHttpMessageHandler(() => handler)
            .AddQaseResultsPolicies(api, NullLogger.Instance);

        var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("qase");
        client.BaseAddress = new Uri("https://api.qase.test/v2/");
        return (client, handler);
    }

    private static ApiConfig FastConfig(int retries = 3) =>
        new() { Timeout = 30, Retries = retries, RetryBackoff = 0.001 };

    [Fact]
    public async Task Pipeline_ShouldResendTheRequestBodyOnEveryRetry()
    {
        var (client, handler) = BuildClient(
            FastConfig(), (HttpStatusCode)429, HttpStatusCode.ServiceUnavailable, HttpStatusCode.OK);

        var response = await client.PostAsync("results", new StringContent("{\"results\":[]}"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        handler.ReceivedBodies.Should().HaveCount(3);
        handler.ReceivedBodies.Should().AllBe("{\"results\":[]}");
    }

    [Fact]
    public async Task Pipeline_ShouldNotResendOnAPermanentFailure()
    {
        var (client, handler) = BuildClient(FastConfig(), (HttpStatusCode)422);

        var response = await client.PostAsync("results", new StringContent("{}"));

        response.StatusCode.Should().Be((HttpStatusCode)422);
        handler.ReceivedBodies.Should().HaveCount(1);
    }

    [Fact]
    public async Task Pipeline_ShouldStopAfterTheConfiguredNumberOfRetries()
    {
        var (client, handler) = BuildClient(FastConfig(retries: 2), HttpStatusCode.BadGateway);

        var response = await client.PostAsync("results", new StringContent("{}"));

        response.StatusCode.Should().Be(HttpStatusCode.BadGateway);
        handler.ReceivedBodies.Should().HaveCount(3);
    }

    [Fact]
    public void Pipeline_ShouldLeaveTheOuterHttpClientTimeoutOpen()
    {
        // The per-attempt timeout is the Polly one. HttpClient's own timeout spans
        // the whole retry chain, so it must not cut a Retry-After wait short.
        var (client, _) = BuildClient(FastConfig());

        client.Timeout.Should().Be(Timeout.InfiniteTimeSpan);
    }

    [Fact]
    public async Task Pipeline_ShouldRetryAnAttemptThatStallsPastTheTimeout()
    {
        var attempts = 0;
        var services = new ServiceCollection();
        services.AddHttpClient("qase")
            .ConfigurePrimaryHttpMessageHandler(() => new StallingHandler(() => attempts++))
            .AddQaseResultsPolicies(
                new ApiConfig { Timeout = 1, Retries = 1, RetryBackoff = 0.001 },
                NullLogger.Instance);

        var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("qase");
        client.BaseAddress = new Uri("https://api.qase.test/v2/");

        var act = async () => await client.PostAsync("results", new StringContent("{}"));

        await act.Should().ThrowAsync<Polly.Timeout.TimeoutRejectedException>();
        attempts.Should().Be(2);
    }

    private sealed class StallingHandler : HttpMessageHandler
    {
        private readonly Action _onSend;

        public StallingHandler(Action onSend) => _onSend = onSend;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _onSend();
            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
