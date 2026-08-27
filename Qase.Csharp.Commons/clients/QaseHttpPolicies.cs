using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using Qase.Csharp.Commons.Config;

namespace Qase.Csharp.Commons.Clients
{
    /// <summary>
    /// Builds the HTTP resilience policies used by the Qase API clients.
    /// </summary>
    internal static class QaseHttpPolicies
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan DefaultBackoff = TimeSpan.FromSeconds(1);

        // Doubling without a ceiling reaches days of waiting — and eventually
        // overflows — if someone configures a large retry count.
        private static readonly TimeSpan MaxComputedBackoff = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Applies the retry and timeout policies used for the test results upload path.
        /// </summary>
        /// <param name="builder">The HTTP client builder to configure</param>
        /// <param name="api">The API connection settings</param>
        /// <param name="logger">The logger used to report each retry</param>
        /// <returns>The builder, for chaining</returns>
        internal static IHttpClientBuilder AddQaseResultsPolicies(
            this IHttpClientBuilder builder, ApiConfig api, ILogger logger)
        {
            // HttpClient's own timeout spans the entire handler chain. Left at its
            // 100 second default it would cut short a retry ladder that honours a
            // 60 second Retry-After, so the per-attempt limit is Polly's instead.
            builder.ConfigureHttpClient(client => client.Timeout = Timeout.InfiniteTimeSpan);

            // Order matters: the first handler added is the outermost one, so retry
            // wraps timeout and each attempt gets its own deadline.
            builder.AddPolicyHandler(CreateRetryPolicy(
                ResolveRetries(api.Retries), ResolveBackoff(api.RetryBackoff), logger));
            builder.AddPolicyHandler(CreateTimeoutPolicy(ResolveTimeout(api.Timeout)));

            return builder;
        }

        /// <summary>
        /// Turns the configured timeout into a usable value.
        /// </summary>
        /// <param name="seconds">The configured timeout in seconds</param>
        /// <returns>The timeout to apply</returns>
        internal static TimeSpan ResolveTimeout(int seconds)
        {
            return seconds > 0 ? TimeSpan.FromSeconds(seconds) : DefaultTimeout;
        }

        /// <summary>
        /// Turns the configured retry count into a usable value.
        /// </summary>
        /// <param name="retries">The configured number of retries</param>
        /// <returns>The retry count to apply</returns>
        internal static int ResolveRetries(int retries)
        {
            return retries > 0 ? retries : 0;
        }

        /// <summary>
        /// Turns the configured backoff into a usable value.
        /// </summary>
        /// <param name="seconds">The configured base backoff in seconds</param>
        /// <returns>The base backoff to apply</returns>
        internal static TimeSpan ResolveBackoff(double seconds)
        {
            return seconds > 0 ? TimeSpan.FromSeconds(seconds) : DefaultBackoff;
        }

        /// <summary>
        /// Builds the retry policy used for the test results upload path.
        /// </summary>
        /// <param name="retries">How many times to retry after the first attempt</param>
        /// <param name="baseBackoff">The base delay the exponential backoff grows from</param>
        /// <param name="logger">The logger used to report each retry</param>
        /// <returns>The retry policy</returns>
        internal static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(
            int retries, TimeSpan baseBackoff, ILogger logger)
        {
            return Policy
                .HandleResult<HttpResponseMessage>(response => IsRetryableStatus(response.StatusCode))
                .Or<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    retries,
                    (attempt, outcome, _) => ComputeDelay(attempt, baseBackoff, outcome.Result),
                    (outcome, delay, attempt, _) =>
                    {
                        if (outcome.Exception != null)
                        {
                            logger.LogWarning(
                                "Qase API request failed ({Error}); retry {Attempt} of {Retries} in {Delay}",
                                outcome.Exception.Message, attempt, retries, delay);
                        }
                        else
                        {
                            logger.LogWarning(
                                "Qase API responded {StatusCode}; retry {Attempt} of {Retries} in {Delay}",
                                (int)outcome.Result.StatusCode, attempt, retries, delay);
                        }

                        return Task.CompletedTask;
                    });
        }

        /// <summary>
        /// Builds the per-attempt timeout policy.
        /// </summary>
        /// <param name="timeout">How long a single attempt may take</param>
        /// <returns>The timeout policy</returns>
        internal static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(TimeSpan timeout)
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(timeout);
        }

        /// <summary>
        /// Determines whether a response status is worth retrying.
        /// </summary>
        /// <param name="statusCode">The status code returned by the server</param>
        /// <returns>True when a retry may succeed, false when the failure is permanent</returns>
        internal static bool IsRetryableStatus(HttpStatusCode statusCode)
        {
            var code = (int)statusCode;

            // 507 is a 5xx, but the server is telling us it has no room for this
            // payload. A second identical batch fails the same way.
            if (code == 507)
            {
                return false;
            }

            return code == 408 || code == 429 || code >= 500;
        }

        /// <summary>
        /// Computes how long to wait before the next attempt.
        /// </summary>
        /// <param name="attempt">The 1-based number of the attempt that just failed</param>
        /// <param name="baseBackoff">The base delay the exponential backoff grows from</param>
        /// <param name="response">The response that triggered the retry, if there was one</param>
        /// <returns>The delay before the next attempt</returns>
        internal static TimeSpan ComputeDelay(int attempt, TimeSpan baseBackoff, HttpResponseMessage? response)
        {
            var retryAfter = ReadRetryAfter(response);
            if (retryAfter.HasValue)
            {
                return retryAfter.Value;
            }

            var multiplier = Math.Pow(2, attempt - 1);
            var ticks = baseBackoff.Ticks * multiplier;

            return ticks >= MaxComputedBackoff.Ticks
                ? MaxComputedBackoff
                : TimeSpan.FromTicks((long)ticks);
        }

        /// <summary>
        /// Reads the Retry-After header, in either of the two forms the spec allows.
        /// </summary>
        /// <param name="response">The response to read the header from</param>
        /// <returns>The delay the server asked for, or null when it did not ask</returns>
        private static TimeSpan? ReadRetryAfter(HttpResponseMessage? response)
        {
            var retryAfter = response?.Headers.RetryAfter;
            if (retryAfter == null)
            {
                return null;
            }

            if (retryAfter.Delta.HasValue && retryAfter.Delta.Value > TimeSpan.Zero)
            {
                return retryAfter.Delta.Value;
            }

            if (retryAfter.Date.HasValue)
            {
                // Prefer the server's own clock when it sent one, so clock skew
                // between us and Qase does not stretch or shrink the wait.
                var now = response!.Headers.Date ?? DateTimeOffset.UtcNow;
                var wait = retryAfter.Date.Value - now;
                if (wait > TimeSpan.Zero)
                {
                    return wait;
                }
            }

            return null;
        }
    }
}
